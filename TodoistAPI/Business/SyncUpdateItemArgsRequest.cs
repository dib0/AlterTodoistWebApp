using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoistAPI.Business
{
    public class SyncUpdateItemArgsRequest
    {
        #region
        public int id;
        public string content;
        public string date_string;
        public long project_id;
        #endregion
    }
}
