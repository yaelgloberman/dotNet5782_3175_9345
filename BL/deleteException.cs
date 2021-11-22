using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class deleteException : Exception
    {
        public deleteException()
        {
        }

        public deleteException(string message) : base(message)
        {
        }

        public deleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected deleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}