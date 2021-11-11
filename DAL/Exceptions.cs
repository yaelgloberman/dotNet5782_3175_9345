using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace IDAL.DO
{
    
        public class customerException : Exception
        {
            public customerException()
            {
            }

            public customerException(string message) : base("Customer exeption:" + message)
            {
            }

            public customerException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected customerException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        [Serializable]
        public class DroneException : Exception
        {
            public DroneException()
            {
            }

            public DroneException(string message) : base(message)
            {
            }

            public DroneException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected DroneException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

    public class AddException : Exception
    {
        public AddException()
        {
        }

        public AddException(string message) : base(message)
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

        public UpdateException(string message) : base("Update Exception: "+message)
        {
        }

        public UpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    //public class AddException : Exception
    //{
    //    public AddException()
    //    {
    //    }

    //    public AddException(string message) : base(message)
    //    {
    //    }

    //    public AddException(string message, Exception innerException) : base(message, innerException)
    //    {
    //    }

    //    protected AddException(SerializationInfo info, StreamingContext context) : base(info, context)
    //    {
    //    }
    }
    //public class UpdateException : Exception
    //{
    //    public UpdateException()
    //    {
    //    }

    //    public UpdateException(string message) : base(message)
    //    {
    //    }

    //    public UpdateException(string message, Exception innerException) : base(message, innerException)
    //    {
    //    }

    //    protected UpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
    //    {
    //    }
    //}

//}