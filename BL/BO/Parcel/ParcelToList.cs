using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelToList
    {
        public int id { set; get; }
        public string senderName { set; get; }
        public string receiveName { set; get; }
        public Weight weight { set; get; }
        public Priority priority { set; get; }
        public ParcelStatus parcelStatus { set; get; }
        public override string ToString()
        {
            return string.Format($"Id:{id} sender name:{senderName}, reciver name: {receiveName}, weight: {weight}, priority: {priority} , parcel status: {parcelStatus}");
        }

    }
}
