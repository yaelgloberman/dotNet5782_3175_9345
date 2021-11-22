using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneToList
    {
        public int id { set; get; }
        public int droneModel { get; set; }
        public Weight weight { set; get; }
        public double batteryStatus { get; set; }
        public DroneStatus droneStatus { set; get; }
        public Location currentLocation { get; set; }
        public int numOfDeliverdParcels { set; get; }
    }
}
