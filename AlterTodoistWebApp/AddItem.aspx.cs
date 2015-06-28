using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TodoistAPI.Business;

namespace AlterTodoistWebApp
{
    public partial class AddItem : BasePage
    {
        #region Overridden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            LoadProjects();
        }
        #endregion

        #region Private methods
        private void LoadProjects()
        {
            foreach (Project p in Projects)
            {
                string txt = p.name;
                if (p.indent > 1)
                    txt = txt.PadLeft(txt.Length + p.indent, '-');

                ddlProject.Items.Add(new ListItem(txt, p.id));
            }
        }
        #endregion

        #region Events
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string content = tbTodoText.Text;
            string dueDate = tbDueDate.Text;
            long pId = long.Parse(ddlProject.SelectedValue);

            SyncAddItemArgsRequest request = new SyncAddItemArgsRequest();
            request.content = content;
            request.date_string = dueDate;
            request.project_id = pId;
            todoist.AddNewItem(request);

            Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
        }
        #endregion
    }
}