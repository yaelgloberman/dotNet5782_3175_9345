using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int id { set; get; }
            public int senderId { set; get; }
            public int targetId { set; get; }
            public Proirities priority { set; get; }
            public WeightCatigories weight { set; get; }
            public int droneId { set; get; }
            public DateTime requested { set; get; }
            public DateTime scheduled { set; get; }
            public DateTime pickedUp { set; get; }
            public DateTime delivered { set; get; }
            public bool isRecived { set; get; }
            public bool isShipped { get; set; }
            //public bool isDelivered { get; set; }//not sure if im aloud to add this feature for convienience
            public override string ToString()
            {
                return string.Format($"Id: {id}, Sender Id:{senderId}, Target Id:{targetId}, Priority: {priority},  Weight Catigory: {weight},Priority: {priority}, Drone Id: {droneId}, Requested: {requested}, Scheduled: {scheduled}, PickedUp: {pickedUp}, Datetime: {delivered}  ");

            }
        }
           
           
    }
}
