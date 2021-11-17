﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Drone
    {
        public int id { set; get; }
        public int droneModel { get; set; }
        public Weight weight { set; get; }
        public BatteryStatus batteryStatus { set; get; }
        public DroneStatus droneStatus { set; get; }
        חבילה בהעברה
        public Location currentLocation { get; set; }
    }
}
