using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data
{

    /// <summary>
    /// Generic data exception
    /// </summary>
    public class DataServiceExceptions : Exception
    {
        public DataServiceExceptions() : base() { }
        public DataServiceExceptions(string message) : base(message) { }
        public DataServiceExceptions(string message, Exception inner) : base(message,inner) { }
    }

    public class DatabaseException : DataServiceExceptions
    {
        public DatabaseException() : base() { }
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner) : base(message, inner) { }
    }

}
