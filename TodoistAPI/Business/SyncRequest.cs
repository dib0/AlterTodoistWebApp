using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoistAPI.Business
{
    public class SyncRequest<T>
    {
        #region
        public string type;
        public T args;
        public string uuid = Guid.NewGuid().ToString();
        #endregion
    }
}
