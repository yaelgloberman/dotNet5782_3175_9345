using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInList
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string  PhoneNumber { get; set; }
        public int Parcles_Delivered_Recieved { get; set; }
        public int Parcels_Delivered_unrecieved { get; set; }
        public int Recieved_Parcels { get; set; }
        public int ParcelsInDeliver { get; set; }
        public string Password { get; set; }
        public bool  isCustomer { get; set; }
        public override string ToString()
        {
            return string.Format($"Id:{id} , name:{Name}, phonenumber: 05{PhoneNumber}, parceles delivered recived: {Parcles_Delivered_Recieved}, parcel unrecived: {Parcels_Delivered_unrecieved} , recived parcels: {Recieved_Parcels} , parcel in deliver: {ParcelsInDeliver}");
        }
    }
}
