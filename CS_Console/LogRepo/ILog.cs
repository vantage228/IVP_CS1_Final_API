using CS_Console.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Console.LogRepo
{
    public interface ILog
    {
        public List<SecurityUpdateLog> GetAllSecurityUpdateLogs();

    }
}
