﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace IBL.BO
{
    [Serializable]
    public class validException : Exception
    {
        public validException()
        {
        }

        public validException(string message) : base("valid Exception:" + message)
        {
        }

        public validException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected validException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class unavailableException : Exception
    {
        public unavailableException()
        {
        }

        public unavailableException(string message) : base("add exception:" + message)
        {
        }

        public unavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected unavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class dosntExisetException : Exception
    {
        public dosntExisetException()
        {
        }

        public dosntExisetException(string message) : base("do not exiset Exception: " + message)
        {
        }

        public dosntExisetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected dosntExisetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException()
        {
        }

        public AlreadyExistException(string message) : base("Already Exist Exception" + message)
        {
        }

        public AlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class ExecutionTheDroneIsntAvilablle : Exception
    {
        public ExecutionTheDroneIsntAvilablle()
        {
        }

        public ExecutionTheDroneIsntAvilablle(string message) : base("Execution The Drone Isnt Avilablle" + message)
        {
        }

        public ExecutionTheDroneIsntAvilablle(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExecutionTheDroneIsntAvilablle(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class deleteException : Exception
    {
        public deleteException()
        {
        }

        public deleteException(string message) : base("delete Exception:" + message)
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

