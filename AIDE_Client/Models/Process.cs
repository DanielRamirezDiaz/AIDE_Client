using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIDE_Client.Models
{
    public class Process
    {
        public string ProcessName { get; set; }

        public int PID { get; set; }

        public long PrivateMemorySize { get; set; }

        public bool RespondingState { get; set; }
    }
}
