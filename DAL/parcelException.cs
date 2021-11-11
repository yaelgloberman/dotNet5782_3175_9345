using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class parcelException : Exception
    {
        public parcelException()
        {
        }

        public parcelException(string message) : base("Parcel exeption:"+ message)
        {
        }

        public parcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected parcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}