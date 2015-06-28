using AlterTodoistWebApp.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Security;
using TodoistAPI;
using TodoistAPI.Business;

namespace AlterTodoistWebApp
{
    public partial class BasePage : System.Web.UI.Page
    {
        #region Protected properties
        protected TodoistRequest todoist;

        protected List<Project> Projects
        {
            get
            {
                List<Project> projects = Session["projects"] as List<Project>;
                if (projects == null)
                {
                    projects = LoadProjects();
                    Session["projects"] = projects;
                }

                return projects;
            }
        }
        #endregion

        #region Protected methods
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            string token = Authenticate();
            todoist = new TodoistRequest(token);
        }
        #endregion

        #region Private methods
        private string Authenticate()
        {
            string returnValue = string.Empty;

            string CookieUserId = "TDATKN1";
            if (Request.Cookies[CookieUserId] != null)
            {
                try
                {
                    returnValue = StringCipher.Decrypt(Request.Cookies[CookieUserId].Value);

                    if (string.IsNullOrEmpty(returnValue))
                    {
                        // Redirect to the login page
                        Response.Redirect(ConfigurationManager.AppSettings["LoginPage"]);
                    }
                    else  // Reset the expires time
                        Request.Cookies[CookieUserId].Expires = DateTime.Now.AddYears(2);
                }
                catch
                {
                    Response.Redirect(ConfigurationManager.AppSettings["LoginPage"]);
                }
            }
            else
                Response.Redirect(ConfigurationManager.AppSettings["LoginPage"]);

            return returnValue;
        }
        #endregion

        #region Private methods
        private List<Project> LoadProjects()
        {
            return todoist.GetProjects();
        }
        #endregion
    }
}