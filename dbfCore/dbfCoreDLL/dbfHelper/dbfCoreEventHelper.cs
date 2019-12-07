using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbfCoreDLL.dbfHelper
{
    public delegate void dbfCoreEventHelper(string o, dbfCoreEventArgs e);
    public class dbfCoreEventArgs
    {
        public string message;
        public string step;
        public dbfCoreEventArgs(string _mesage, string _step)
        {
            message = _mesage;
            step = _step;
        }
    }
}
