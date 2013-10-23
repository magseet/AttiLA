using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BleDA.AttiLA;

namespace BleDA
{
    public sealed class Status
    {
        private static volatile Status _instance;
        private static object syncRoot = new Object();

        private Status() {
            CurrentContextId = "";
            ServiceStatus = null;
            Process = new Process();
        }

        public static Status BleDAStatus
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new Status();
                    }
                }

                return _instance;
            }
        }

        public String CurrentContextId { get; set; }

        public ServiceStatus ServiceStatus { get; set; }

        public Process Process { get; set; }
    }
}
