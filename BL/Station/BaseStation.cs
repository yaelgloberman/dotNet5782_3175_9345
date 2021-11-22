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
        public Location currentLocation { get; set; }
        public int avilableChargeSlots { get; set; }
        public List<DroneInCharge> chargeDroneList { get; set; }
    }
}
