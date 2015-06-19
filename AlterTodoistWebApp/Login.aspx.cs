using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
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
                Response.Redirect("~/Default.aspx");
            }
        }
        #endregion

        #region Private methods
        private void CreateCookie(string token)
        {
            string CookieUserId = "TDATKN1";

            HttpCookie cookie = new HttpCookie(CookieUserId);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(token, true, -1);
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            cookie.Expires = DateTime.MaxValue;

            Response.Cookies.Add(cookie);
        }
        #endregion
    }
}