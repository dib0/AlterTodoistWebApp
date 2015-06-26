using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoistAPI.Business
{
    public class SyncArgsRequest
    {
        #region
        public int project_id;
        public List<int> ids = new List<int>();
        #endregion
    }
}
