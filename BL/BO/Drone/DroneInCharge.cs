using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BO
{
    public class DroneInCharge
    {
        public int id { get; set; }
        public double batteryStatus { get; set; }
        public override string ToString()
        {
            return String.Format($"Drone Id: {id}\nBattery: {batteryStatus}%\n");
        }
    }
}
