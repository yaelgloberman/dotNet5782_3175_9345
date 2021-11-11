using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class customerException : Exception
    {
        public customerException()
        {
        }

        public customerException(string message) : base("Customer exeption:" +message)
        {
        }

        public customerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected customerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}