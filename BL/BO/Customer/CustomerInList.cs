using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerInList
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int  PhoneNumber { get; set; }
        public int Parcles_Delivered_Recieved { get; set; }
        public int Parcels_unrecieved { get; set; }
        public int Recieved_Parcels { get; set; }
        public int ParcelsInDeliver { get; set; }
        public override string ToString()
        {
            return string.Format($"Id:{id} , name:{Name}, phonenumber: {PhoneNumber}, parceles delivered recived: {Parcles_Delivered_Recieved}, parcel unrecived: {Parcels_unrecieved} , recived parcels: {Recieved_Parcels} , parcel in deliver: {ParcelsInDeliver}");
        }
    }
}
