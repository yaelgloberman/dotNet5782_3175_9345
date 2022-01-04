//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//namespace DalApi
//{
//    namespace DalXml
//    {
//        class xmlTools
//        {
//            static string dir = @"xml\";
//            static xmlTools()
//            {
//                if (!Directory.Exists(dir))
//                    Directory.CreateDirectory(dir);
//            }
//            public static void SaveLi
//        }


using System;
using System.Runtime.Serialization;

namespace Dal
{
    [Serializable]
    internal class LoadingException : Exception
    {
        private string filePath;
        private string v;
        private Exception ex;

        public LoadingException()
        {
        }

        public LoadingException(string message) : base(message)
        {
        }

        public LoadingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LoadingException(string filePath, string v, Exception ex)
        {
            this.filePath = filePath;
            this.v = v;
            this.ex = ex;
        }

        protected LoadingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}