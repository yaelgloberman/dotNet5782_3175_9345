using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class ExistException : Exception
    {
        public ExistException()
        {
        }

        public ExistException(string message) : base(message)
        {
        }

        public ExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}