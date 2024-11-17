using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Console.Model
{
    public class SecurityUpdateLog
    {
        public int LogId { get; set; }
        public int SecurityId { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string FieldUpdated { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UpdateStatus { get; set; }
        public string ErrorMessage { get; set; }

        public string tableType { get; set; }
    }

}
