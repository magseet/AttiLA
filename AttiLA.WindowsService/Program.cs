using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.WindowsService
{

    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        static void Main()
        {           
#if DEBUG
            AttiLAWindowsService svc = new AttiLAWindowsService();
            svc.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AttiLAWindowsService() 
            };
            ServiceBase.Run(ServicesToRun);

#endif
        }
    }
}
