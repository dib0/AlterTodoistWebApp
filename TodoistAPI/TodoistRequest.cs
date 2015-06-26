﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Script.Serialization;
using TodoistAPI.Business;

namespace TodoistAPI
{
    public class TodoistRequest
    {
        #region Private properties
        private const string baseUri = "todoist.com/API/v6";
        private const string loginUri = "/login?email={0}&password={1}";
        private const string tokenUri = "token={0}";
        private const string queryUri = "/query?";
        private const string queryFilterUri = "&queries=\"{0}\"";
        private const string commandUri = "&commands={0}";
        private const string syncUri = "/sync";

        private bool UseSecureConnection
        { get; set; }

        private string ContentType
        { get; set; }

        private int last_seq_no = 0;
        private int last_seq_no_global = 0;
        #endregion

        #region Public properties
        public string Token
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public TodoistRequest()
        {
            UseSecureConnection = true;
            ContentType = "application/json";
        }

        public TodoistRequest(string token) : this()
        {
            Token = token;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Login using username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if login is a success</returns>
        public bool Login(string username, string password)
        {
            bool loggedIn = false;

            string uri = baseUri + string.Format(loginUri, username, password);
            try
            {
                LoginResult result = PerformGetRequest<LoginResult>(uri);
                Token = result.token;
                loggedIn = true;
            }
            catch
            {
                loggedIn = false;
            }

            return loggedIn;
        }

        public List<QueryDataResult> GetItemsToday()
        {
            return QueryItems("today");
        }

        public List<QueryDataResult> QueryItems(params string[] queries)
        {
            if (string.IsNullOrEmpty(Token))
                throw new Exception("Not logged in.");
            if ((queries == null) || (queries.Count() == 0))
                throw new Exception("No query specified.");

            string uri = baseUri + queryUri + string.Format(tokenUri, Token);
            // Add filters
            string filters = string.Empty;
            for (int i=0; i< queries.Count(); i++)
            {
                if (i > 0)
                    filters += "," + queries[i];
                else
                    filters += queries[i];
            }
            uri += string.Format(queryFilterUri, filters);

            List<QueryResult> qResult = PerformGetRequest<List<QueryResult>>(uri);
            List<QueryDataResult> result = new List<QueryDataResult>();
            foreach(QueryResult q in qResult)
            {
                foreach(QueryDataResult qd in q.data)
                {
                    QueryDataResult i = result.FirstOrDefault(j => j.id == qd.id);
                    if (i == null)
                        result.Add(qd);
                }
            }

            result.Sort(new Comparison<QueryDataResult>(CompareDate));
            return result;
        }

        public void CompleteItem(int projectId, params int[] itemIds)
        {
            if (itemIds == null || itemIds.Count() == 0)
                throw new ArgumentException("No project id(s) are given.");

            string uri = baseUri + syncUri;

            SyncRequest<SyncArgsRequest> req = new SyncRequest<SyncArgsRequest>();
            req.type = "item_complete";
            req.args = new SyncArgsRequest();
            req.args.project_id = projectId;
            foreach (int iId in itemIds)
                req.args.ids.Add(iId);

            List<SyncRequest<SyncArgsRequest>> requests = new List<SyncRequest<SyncArgsRequest>>();
            requests.Add(req);

            SyncResult result = PerformPostRequest<List<SyncRequest<SyncArgsRequest>>, SyncResult>(requests, uri);
        }

        public void CompleteRecurringItem(int itemId)
        {
            string uri = baseUri + syncUri;

            SyncRequest<SyncRecurringArgsRequest> req = new SyncRequest<SyncRecurringArgsRequest>();
            req.type = "item_update_date_complete";
            req.args = new SyncRecurringArgsRequest();
            req.args.id = itemId;

            List<SyncRequest<SyncRecurringArgsRequest>> requests = new List<SyncRequest<SyncRecurringArgsRequest>>();
            requests.Add(req);

            SyncResult result = PerformPostRequest<List<SyncRequest<SyncRecurringArgsRequest>>, SyncResult>(requests, uri);
        }
        #endregion

        #region Private methods
        private static int CompareDate(QueryDataResult a, QueryDataResult b)
        {
            return a.due_date.CompareTo(b.due_date);
        }

        private string AddProtocol(string uri)
        {
            return UseSecureConnection ? "https://" + uri : "http://" + uri;
        }

        private HttpWebRequest GetWebRequest(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(AddProtocol(uri));
            request.ContentType = ContentType;

            if (UseSecureConnection)
            {
                // Override automatic validation of SSL server certificates.
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertficate;
                }
                catch { }
            }

            return request;
        }

        private I PerformPostRequest<T, I>(T input, string uri)
        {
            HttpWebRequest request = GetWebRequest(uri);

            JavaScriptSerializer js = new JavaScriptSerializer();
            string inputString = string.Format(tokenUri, Token);
            inputString += string.Format(commandUri, js.Serialize(input));

            // Add the input object
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = inputString.Count();
            using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                sw.Write(inputString);

            // Get the result
            string resultString = PerformRequest(request);

            // Serialize to object
            I result = js.Deserialize<I>(resultString);
            return result;
        }

        private T PerformGetRequest<T>(string uri)
        {
            HttpWebRequest request = GetWebRequest(uri);
            request.Method = "GET";
            string resultString = PerformRequest(request);

            // Serialize to object
            JavaScriptSerializer js = new JavaScriptSerializer();
            T result = js.Deserialize<T>(resultString);
            return result;
        }

        private string PerformRequest(HttpWebRequest request)
        {
            // Get the result
            HttpWebResponse wr = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(wr.GetResponseStream());
            string resultString = sr.ReadToEnd();

            return resultString;
        }

        private bool ValidateServerCertficate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion
    }
}
