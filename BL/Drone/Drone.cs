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
        public double batteryStatus { set; get; }
        public DroneStatus droneStatus { set; get; }
        public CustomerInParcel CustomerInParcel { get; set; }//not sure if i meant drone in parcel
        public Location location { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, Model: {droneModel}, the max weight: {weight}, drones battery Status: {batteryStatus}, " +
                $" drones  Status: {droneStatus} Customer who is sigened to this drone:\n {CustomerInParcel}, Location: {location}  ");
        }




    }

}
