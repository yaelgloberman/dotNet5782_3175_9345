using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
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
            return string.Format($"id: {id}, parcelStatus: {parcelStatus}, the max weight: {priority}, drones battery Status: {weight}, " +
                $" drones  Status: {sender}parcel in transfer:\n {receive}, Location: {collection}   ");
        }
    }
}