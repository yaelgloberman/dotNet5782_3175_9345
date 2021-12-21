using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace DO
{

    [Serializable]
    public class findException : Exception
    {
        public findException()
        {
        }

        public findException(string message) : base(message)
        {
        }

        public findException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected findException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class AddException : Exception
    {
        public AddException()
        {
        }

        public AddException(string message) : base("add exception:"+message)
        {
        }

        public AddException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AddException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class UpdateException : Exception
    {
        public UpdateException()
        {
        }

        public UpdateException(string message) : base("Update Exception: " + message)
        {
        }

        public UpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}