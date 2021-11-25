using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class ExecutionTheDroneIsntAvilablle : Exception
    {
        private object exe;

        public ExecutionTheDroneIsntAvilablle()
        {
        }

        public ExecutionTheDroneIsntAvilablle(object exe)
        {
            this.exe = exe;
        }

        public ExecutionTheDroneIsntAvilablle(string message) : base(message)
        {
        }

        public ExecutionTheDroneIsntAvilablle(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExecutionTheDroneIsntAvilablle(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}