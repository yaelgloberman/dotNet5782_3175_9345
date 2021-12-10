using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStation
    {
        public int id { get; set; }
        public string stationName { set; get; }
        public Location location { get; set; }
        public int avilableChargeSlots { get; set; }
        public void decreasingChargeSlots() { avilableChargeSlots--; BL.BL.unavailableChargeSlots++; }
        public void addingChargeSlots() { avilableChargeSlots++; }
        public IEnumerable<DroneInCharge> DroneInChargeList { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, station Name: {stationName}, location: {location},avilable Charging Slots:{avilableChargeSlots}");
        }
    }
}
