using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class GetDetailsException : Exception
    {
        public GetDetailsException()
        {
        }

        public GetDetailsException(string message) : base(message)
        {
        }

        public GetDetailsException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected GetDetailsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}