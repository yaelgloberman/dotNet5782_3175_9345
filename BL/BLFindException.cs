using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class BLFindException : Exception
    {
        public BLFindException()
        {
        }

        public BLFindException(string message) : base("BLFindException:"+message+"not found\n")
        {
        }

        public BLFindException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLFindException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}