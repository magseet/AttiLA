using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleDA
{
    public class Action
    {
        public Operation Operation { get; set; }

        public String ActionName { get; set; }

        public String Param1 { get; set; }

        public String Param2 { get; set; }
    }
}
