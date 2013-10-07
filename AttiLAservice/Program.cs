using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Service
{

    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        static void Main()
        {           
#if DEBUG
            AttiLAservice svc = new AttiLAservice();
            svc.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AttiLAsvc() 
            };
            ServiceBase.Run(ServicesToRun);

#endif
        }
    }
}
