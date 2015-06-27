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
    public partial class EditItem : BasePage
    {
        #region Overridden methods
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                LoadProjects();
                ShowCurrentValues();
            }
        }
        #endregion

        #region Private methods
        private void LoadProjects()
        {
            List<Project> pr = todoist.GetProjects();
            foreach (Project p in pr)
            {
                string txt = p.name;
                if (p.indent > 1)
                    txt = txt.PadLeft(txt.Length + p.indent, '-');

                ddlProject.Items.Add(new ListItem(txt, p.id));
            }
        }

        private void ShowCurrentValues()
        {
            QueryDataResult item = Session["editableItem"] as QueryDataResult;
            if (item != null)
            {
                tbTodoText.Text = item.content;
                tbDueDate.Text = item.date_string;
                
                foreach (ListItem ddlItem in ddlProject.Items)
                {
                    if (ddlItem.Value == item.project_id.ToString())
                        ddlItem.Selected = true;
                }
            }
            else
                Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
        }
        #endregion

        #region Events
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string content = tbTodoText.Text;
            string dueDate = tbDueDate.Text;
            long pId = long.Parse(ddlProject.SelectedValue);

            QueryDataResult item = Session["editableItem"] as QueryDataResult;
            SyncUpdateItemArgsRequest request = new SyncUpdateItemArgsRequest();
            request.id = item.id;
            request.content = content;
            request.date_string = dueDate;
            request.project_id = pId;
            todoist.UpdateItem(request);

            Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
        }
        #endregion
    }
}