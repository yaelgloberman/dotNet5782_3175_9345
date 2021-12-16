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
        public string droneModel { get; set; }
        public Weight weight { set; get; }
        public double batteryStatus { get; set; }
        public DroneStatus droneStatus { set; get; }
        public Location location { get; set; }
        public int parcelId { get; set; }
        public override string ToString()
        {
            return String.Format($"drone id:{id}, drone Model: {droneModel}, weight: {weight}, battery Status:{Math.Round(batteryStatus)}%, drone Status: {droneStatus}, location: {location}, delivery ID: {parcelId}");
        }

    }

}
