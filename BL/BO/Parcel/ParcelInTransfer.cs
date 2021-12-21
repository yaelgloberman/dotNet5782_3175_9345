using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInTransfer
    {
        public int id { get; set; }
        public bool parcelStatus { get; set; }
        public Priority priority { get; set; }
        public Weight weight { get; set; }
        public CustomerInParcel sender { set; get; }
        public CustomerInParcel receive { set; get; }
        public Location collection { get; set; }
        public Location DeliveryDestination { get; set; }
        public double TransportDistance { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, parcelStatus: {parcelStatus}, the priority: {priority}, the weight is {weight}, the sender: {sender} the receiver: {receive} , the collection loctation:{collection} , the delivery destination{DeliveryDestination} , transport distance{TransportDistance}   ");
        }
    }
}