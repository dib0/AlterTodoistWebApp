using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Script.Serialization;
using TodoistAPI.Business;

namespace TodoistAPI
{
    public class TodoistRequest
    {
        #region Private properties
        private const string baseUri = "todoist.com";
        private const string loginUri = "/API/login?email={0}&password={1}";
        private const string tokenUri = "token={0}";
        private const string queryUri = "/API/query?";
        private const string queryFilterUri = "&queries=\"{0}\"";

        private bool UseSecureConnection
        { get; set; }

        private string ContentType
        { get; set; }
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

        public void GetItemsToday()
        {
            QueryItems("today");
        }

        public void QueryItems(params string[] queries)
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

            QueryResult[] result = PerformGetRequest<QueryResult[]>(uri);
        }
        #endregion

        #region Private methods
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
                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertficate;
            }

            return request;
        }

        private I PerformPostRequest<T, I>(T input, string uri)
        {
            HttpWebRequest request = GetWebRequest(uri);
            JavaScriptSerializer js = new JavaScriptSerializer();
            string inputString = js.Serialize(input);

            // Add the input object
            request.Method = "POST";
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
