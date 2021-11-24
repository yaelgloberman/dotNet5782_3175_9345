using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class DoesntExistException : Exception
    {
        public DoesntExistException()
        {
        }

        public DoesntExistException(string message) : base(message)
        {
        }

        public DoesntExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}