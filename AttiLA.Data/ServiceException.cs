using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data
{
    public class DataException : Exception, ISerializable
    {

        public DataException() { }
        public DataException(string message) { }
        public DataException(string message, Exception inner) { }
        public DataException(SerializationInfo info, StreamingContext context) { }
    }
}
