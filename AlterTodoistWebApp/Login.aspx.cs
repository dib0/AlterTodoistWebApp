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
        #region Protected methods
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string uname = tbUsername.Text;
            string pw = tbPassword.Text;

            TodoistRequest tr = new TodoistRequest();
            if (tr.Login(uname, pw))
            {
                CreateCookie(tr.Token);
                Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
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