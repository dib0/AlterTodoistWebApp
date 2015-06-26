using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoistAPI.Business
{
    public class QueryDataResult
    {
        #region Public properties
        public int id;
        public DateTime due_date;
        public string content;
        public int priority;
        public string date_string;
        public int project_id;

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
