using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoistAPI.Business
{
    public class SyncAddItemArgsRequest
    {
        #region
        public string content;
        public string date_string;
        public long project_id;
        #endregion
    }
}
