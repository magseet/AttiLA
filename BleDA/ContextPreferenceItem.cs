using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BleDA.LocalizationService;
using AttiLA.Data.Entities;
using MongoDB.Bson;

namespace BleDA
{
    public class ContextPreferenceItem
    {
        public String ContextName { get; set; }
        public String Preference { get; set; }
    }
}
