using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIDE_Client.Models
{
    public class Command
    {
        public string FileName { get; set; }

        public string Arguments { get; set; }

        public int TimeBetweenChecks { get; set; }

        public int Times { get; set; }
    }
}
