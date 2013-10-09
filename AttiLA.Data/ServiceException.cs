using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data
{
    public class ServiceException : Exception, ISerializable
    {

        public ServiceException() { }
        public ServiceException(string message) { }
        public ServiceException(string message, Exception inner) { }
        public ServiceException(SerializationInfo info, StreamingContext context) { }
    }
}
