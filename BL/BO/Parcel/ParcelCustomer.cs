using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelCustomer
    {
        public int id { get; set; }
        public Weight weight { get; set; }
        public Priority priority { get; set; }
        public ParcelStatus parcelStatus { get; set; }
        public CustomerInParcel CustomerInParcel { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, Name: {weight}, priority:{priority}, ParcelStatus {parcelStatus},CustomerInParcel: {CustomerInParcel} ");/// didnt include the last 2 lists in the 2 string
        }
    }

}
