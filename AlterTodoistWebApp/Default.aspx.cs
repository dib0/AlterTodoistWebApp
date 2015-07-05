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
    public partial class Default : BasePage
    {
        #region Private properties
        List<QueryDataResult> todoistItems=null;
        private int selectedItemId = -1;
        #endregion

        #region Protected method
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (Session["tmpItemId"] != null)
            {
                selectedItemId = int.Parse(Session["tmpItemId"].ToString());
                Session["tmpItemId"] = null;
            }

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
            ddlProject.Items.Add(new ListItem("Tomorrow", "-2"));

            foreach (Project p in Projects)
            {
                string txt = p.name;
                if (p.indent > 1)
                    txt = txt.PadLeft(txt.Length + p.indent, '-');

                ddlProject.Items.Add(new ListItem(txt, p.id));
            }
        }

        private void SelectRightItem()
        {
            ListItem li = ddlProject.Items.FindByValue(selectedItemId.ToString());
            if (li != null)
                li.Selected = true;
        }

        private void LoadTodoItems()
        {
            if (selectedItemId == -2)
                todoistItems = todoist.QueryItems("over due", "tomorrow");
            else
                todoistItems = todoist.QueryItems("over due", "today");
        }

        private void BuildTodoList()
        {
            itemList.Controls.Clear();

            if (todoistItems != null && todoistItems.Count > 0)
            {
                DateTime lastDate = DateTime.MinValue;
                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                foreach (QueryDataResult r in todoistItems)
                {
                    DateTime checkDate = new DateTime(r.due_date.Year, r.due_date.Month, r.due_date.Day);
                    
                    // Check title
                    if (checkDate > lastDate)
                    {
                        HtmlGenericControl titleDiv = new HtmlGenericControl("div");
                        titleDiv.Attributes["class"] = "titletext title";
                        HtmlGenericControl subTitleDiv = new HtmlGenericControl("div");
                        subTitleDiv.Attributes["class"] = "subtitletext subtitle";

                        // Different days, so a title is needed
                        if (checkDate == today)
                        {
                            titleDiv.InnerHtml = "Today";
                            subTitleDiv.InnerHtml = checkDate.DayOfWeek.ToString() + " " + checkDate.ToString("d MMM");
                        }
                        else if (checkDate == today.AddDays(1))
                        {
                            titleDiv.InnerHtml = "Tomorrow";
                            subTitleDiv.InnerHtml = checkDate.DayOfWeek.ToString() + " " + checkDate.ToString("d MMM");
                        }
                        else if (checkDate < today)
                        {
                            titleDiv.InnerHtml = "Over due";
                            subTitleDiv.InnerHtml = checkDate.ToString("d MMM");
                        }
                        else
                        {
                            titleDiv.InnerHtml = checkDate.DayOfWeek.ToString();
                            subTitleDiv.InnerHtml = checkDate.ToString("d MMM");
                        }

                        titleDiv.Controls.Add(subTitleDiv);

                        itemList.Controls.Add(titleDiv);
                        lastDate = checkDate;
                    }

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
                    if (r.due_date.ToString("HH:mm") != "23:59")
                        singleItemDiv.InnerHtml += "&nbsp;(" + r.due_date.ToString("HH:mm") + ")";
                    todoItemDiv.Controls.Add(singleItemDiv);

                    itemList.Controls.Add(todoItemDiv);
                }
            }
            else
            {
                HtmlGenericControl titleDiv = new HtmlGenericControl("div");
                titleDiv.Attributes["class"] = "titletext title";
                titleDiv.InnerHtml = "Today";
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
                QueryDataResult item = todoistItems.FirstOrDefault(i => i.id == itemId);
                if (item != null)
                {
                    if (item.IsRecurring)
                        todoist.CompleteRecurringItem(item.id);
                    else
                        todoist.CompleteItem(item.project_id, item.id);
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
                QueryDataResult item = todoistItems.FirstOrDefault(i => i.id == itemId);
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
            {
                selectedItemId = int.Parse(ddlProject.SelectedValue);

                LoadTodoItems();
                BuildTodoList();
                SelectRightItem();
            }
            else
            {
                Session["viewproject"] = selected;
                Response.Redirect("ViewProject.aspx");
            }
        }
        #endregion
    }
}