using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;

namespace AttiLA.LocalizationService
{
    public class Localizer
    {
        /// <summary>
        /// Samples supplier module.
        /// </summary>
        private WlanScanner wlanScanner = WlanScanner.Instance;

        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private ScenarioService scenarioService = new ScenarioService();

        public Localizer()
        {

        }

    }
}
