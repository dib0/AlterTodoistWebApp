using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using TodoistAPI.Business;
using System.Collections.Generic;
using System.Configuration;

namespace AlterTodoistWebApp
{
    public partial class ViewProject : BasePage
    {
        #region Private properties
        List<Item> todoistItems=null;
        Project selected;
        #endregion

        #region Protected method
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            selected = Session["viewproject"] as Project;
            if (selected == null)
                Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);

            if (!IsPostBack)
                LoadProjects();

            LoadTodoItems();
            BuildTodoList();
        }
        #endregion

        #region Private methods
        private void LoadProjects()
        {
            ddlProject.Items.Add(new ListItem("Today", "-1"));

            foreach (Project p in Projects)
            {
                string txt = p.name;
                if (p.indent > 1)
                    txt = txt.PadLeft(txt.Length + p.indent, '-');

                ListItem item = new ListItem(txt, p.id);
                if (p.id == selected.id)
                    item.Selected = true;

                ddlProject.Items.Add(item);
            }
        }

        private void LoadTodoItems()
        {
            todoistItems = todoist.GetTasksForProject(selected.id);
        }

        private void BuildTodoList()
        {
            itemList.Controls.Clear();

            if (todoistItems != null && todoistItems.Count > 0)
            {
                HtmlGenericControl titleDiv = new HtmlGenericControl("div");
                titleDiv.Attributes["class"] = "titletext title";
                titleDiv.InnerHtml = selected.name;
                itemList.Controls.Add(titleDiv);

                foreach (Item r in todoistItems)
                {
                    HtmlGenericControl todoItemDiv = new HtmlGenericControl("div");
                    todoItemDiv.Attributes["class"] = "todoitem";

                    ImageButton completeButton = new ImageButton();
                    completeButton.ID = "complete_" + r.id.ToString();
                    completeButton.ImageUrl = "./images/complete.png";
                    completeButton.CssClass = "pure-img graphbutton";
                    completeButton.Click += CompleteButton_Click;
                    todoItemDiv.Controls.Add(completeButton);
                    
                    ImageButton editButton = new ImageButton();
                    editButton.ID = "edit_" + r.id.ToString();
                    editButton.ImageUrl = "./images/edit.png";
                    editButton.CssClass = "pure-img graphbutton";
                    editButton.Click += EditButton_Click;
                    todoItemDiv.Controls.Add(editButton);

                    HtmlGenericControl singleItemDiv = new HtmlGenericControl("div");
                    singleItemDiv.Attributes["class"] = "itemtext";
                    singleItemDiv.InnerHtml = r.content;
                    if (r.due_date.HasValue)
                    {
                        if (r.due_date.Value.ToString("HH:mm") != "23:59")
                            singleItemDiv.InnerHtml += "&nbsp;(" + r.due_date.Value.ToString("HH:mm") + ")";
                    }
                    todoItemDiv.Controls.Add(singleItemDiv);

                    itemList.Controls.Add(todoItemDiv);
                }
            }
            else
            {
                HtmlGenericControl titleDiv = new HtmlGenericControl("div");
                titleDiv.Attributes["class"] = "titletext title";
                titleDiv.InnerHtml = selected.name;
                itemList.Controls.Add(titleDiv);

                HtmlGenericControl singleItemDiv = new HtmlGenericControl("div");
                singleItemDiv.Attributes["class"] = "itemtext";
                singleItemDiv.InnerHtml = "No todo items.";
                itemList.Controls.Add(singleItemDiv);
            }
        }

        private int GetID(string controlId)
        {
            int result = -1;

            if (!int.TryParse(controlId.Split('_')[1], out result))
                result = -1;

            return result;
        }
        #endregion

        #region Events
        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("AddItem.aspx");
        }

        private void CompleteButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = sender as ImageButton;
            if (button != null)
            {
                int itemId = GetID(button.ID);
                Item item = todoistItems.FirstOrDefault(i => i.id == itemId);
                if (item != null)
                {
                    if (item.IsRecurring)
                        todoist.CompleteRecurringItem(item.id);
                    else
                        todoist.CompleteItem(int.Parse(item.project_id), item.id);
                }
            }

            LoadTodoItems();
            BuildTodoList();
        }

        private void EditButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = sender as ImageButton;
            if (button != null)
            {
                int itemId = GetID(button.ID);
                Item item = todoistItems.FirstOrDefault(i => i.id == itemId);
                if (item != null)
                {
                    Session["editableItem"] = item;
                    Response.Redirect("EditItem.aspx");
                }
            }
        }

        protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
        {
            // Does nothing, just refreshes
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            Project selected = Projects.FirstOrDefault(p => p.id == ddlProject.SelectedValue);
            if (selected == null)
                Response.Redirect(ConfigurationManager.AppSettings["DefaultPage"]);
            else
            {
                Session["viewproject"] = selected;
                Response.Redirect("ViewProject.aspx");
            }
        }
        #endregion
    }
}