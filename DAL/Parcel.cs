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
            public int Id { set; get; }
            public int SenderId { set; get; }
            public int TargetId { set; get; }
            public Proirities Priority { set; get; }
            public WeightCatigories Weight { set; get; }
            public int DroneId { set; get; }
            public DateTime Requested { set; get; }
            public DateTime Scheduled { set; get; }
            public DateTime PickedUp { set; get; }
            public DateTime Datetime { set; get; }
            public override string ToString()
            {
                return string.Format($"Id: {Id}, Sender Id: {SenderId}, Target Id: {TargetId}, Priority: {Priority},  Weight Catigory: {Weight},Priority: {Priority}, Drone Id: {DroneId}, Requested: {Requested}, Scheduled: {Scheduled}, PickedUp: {PickedUp}, Datetime: {Datetime}  ");

            }
        }
           
           
    }
}
