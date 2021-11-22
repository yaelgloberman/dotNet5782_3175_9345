using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
{
    public class Drone
    {
        public int id { set; get; }
        public int droneModel { get; set; }
        public Weight weight { set; get; }
        public BatteryStatus batteryStatus { set; get; }
        public DroneStatus droneStatus { set; get; }
        public CustomerInParcel CustomerInParcel { get; set; }
        public Location currentLocation { get; set; }
        

    }

}
