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
        public int stationName { set; get; }
        public Location location { get; set; }
        public int avilableChargeSlots { get; set; }
        public List<DroneInCharge> DroneInChargeList { get; set; }
        
    }
}
