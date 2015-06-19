using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TodoistAPI;

namespace AlterTodoistWebApp
{
    public partial class BasePage : System.Web.UI.Page
    {
        #region Protected properties
        protected TodoistRequest todoist;
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
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Request.Cookies[CookieUserId].Value);
                    returnValue = ticket.Name;

                    if (string.IsNullOrEmpty(returnValue))
                    {
                        // Redirect to the login page
                        Response.Redirect("~/Login.aspx");
                    }
                }
                catch
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
            else
                Response.Redirect("~/Login.aspx");

            return returnValue;
        }
        #endregion
    }
}