using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BleDA.AttiLA;

namespace BleDA
{
    public class Status
    {

        public Status()
        {
            CurrentContextId = "";
            ServiceStatus = null;
            Process = new Process();
        }

        public String CurrentContextId { get; set; }

        public ServiceStatus ServiceStatus { get; set; }

        public Process Process { get; set; }
    }
}
