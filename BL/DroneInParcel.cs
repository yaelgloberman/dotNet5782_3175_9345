﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
{
    public class DroneInParcel
    {
        public int id { get; set; }
        public BatteryStatus battery { get; set; }
        public Location currentLocation { get; set; } //צריך לעבור על הז 
    }
}
