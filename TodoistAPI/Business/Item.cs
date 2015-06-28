using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoistAPI.Business
{
    public class Item
    {
        #region
        public int id;
        public DateTime? due_date;
        public string content;
        public string date_string;
        public string project_id;
        public int @checked;

        public bool IsRecurring
        {
            get
            {
                return date_string.Contains("every");
            }
        }
        #endregion
    }
}
