using AlterTodoistWebApp.Util;
using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using TodoistAPI;

namespace AlterTodoistWebApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            string gmail  = Request.Params["googleEmail"];
            string gToken = Request.Params["googleToken"];

            if (!String.IsNullOrEmpty(gmail) && !String.IsNullOrEmpty(gToken))
            {
                // dit token is blijkbaar een access token en geen oauth2 token. Ik kom er niet uit hoe je die vertaling doen moet
                TodoistRequest api = new TodoistRequest();
                if ( api.LoginWithGoogle(gmail, gToken) )
                {
                    CreateCookie(api.Token);
                    Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
                }
            }

            base.OnLoad(e);
        }

        #region Protected methods
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string uname = tbUsername.Text;
            string pw = tbPassword.Text;

            if (!string.IsNullOrEmpty(uname) && !string.IsNullOrEmpty(pw))
            {
                TodoistRequest tr = new TodoistRequest();
                if (tr.Login(uname, pw))
                {
                    CreateCookie(tr.Token);
                    Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
                }
            }
            else
            {
                string gmail = Request.Params["googleEmail"];
                string gToken = Request.Params["googleToken"];

                if (!string.IsNullOrEmpty(gmail) && !string.IsNullOrEmpty(gToken))
                {

                }
            }
        }
        #endregion

        #region Private methods
        private void CreateCookie(string token)
        {
            string CookieUserId = "TDATKN1";

            HttpCookie cookie = new HttpCookie(CookieUserId);
            cookie.Value = StringCipher.Encrypt(token);
            cookie.Expires = DateTime.Now.AddYears(2);

            Response.Cookies.Add(cookie);
        }
        #endregion
    }
}