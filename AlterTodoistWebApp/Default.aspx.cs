using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TodoistAPI;
using TodoistAPI.Business;

namespace AlterTodoistWebApp
{
    public partial class Default : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            QueryResult result = todoist.QueryItems("overdue", "today");
            if (result.data != null && result.data.Count>0)
            {
                foreach (QueryDataResult r in result.data)
                    Response.Write(r.content + " " + r.due_date + "<br />");
            }
        }
    }
}