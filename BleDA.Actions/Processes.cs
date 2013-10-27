using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Management;
using System.Security.Authentication;
using System.Diagnostics;
using AttiLA.Data.Entities;

namespace BleDA.Actions
{
    public class ProcessManagementObject
    {
        public Process Process { get; set; }

        public ManagementObject ManagementObject { get; set; }
    }

    public class Processes
    {
        public static void ExecuteProcess(OpeningAction action)
        {
            if (action == null || action.Path == null)
                throw new ArgumentNullException("action");

            if (action.Arguments == null)
                Process.Start(action.Path);
            else
                Process.Start(action.Path, action.Arguments);

            return;
        }

        /// <summary>
        /// Select the processes for the current user.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ProcessManagementObject> GetUserProcesses()
        {
            var result = new List<ProcessManagementObject>();

            // get user identity
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            if (user == null)
            {
                throw new InvalidCredentialException();
            }
            var userName = user.Name;

            // get all processes
            var wmiQueryString = "SELECT * FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var query = from p in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new ProcessManagementObject
                            {
                                ManagementObject = mo,
                                Process = p
                            };

                var processOwnerInfo = new object[2];
                foreach (var item in query)
                {
                    // get process owner
                    item.ManagementObject.InvokeMethod("GetOwner", processOwnerInfo);
                    var processOwner = (string)processOwnerInfo[0];
                    var net = (string)processOwnerInfo[1];
                    if (!string.IsNullOrEmpty(net))
                    {
                        processOwner = string.Format("{0}\\{1}", net, processOwner);
                    }

                    if (string.CompareOrdinal(processOwner, userName) == 0)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;

        }

    }
}
