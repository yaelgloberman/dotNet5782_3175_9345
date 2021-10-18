using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { set; get; }
            public string Model { set; get; }
            public WeightCatigories MaxWeight { set; get; }
            public double BateryStatus { set; get; }     //צריך לחשוב איך עושים רמת טעינה 
            public DroneStatuses Status { set; get; }

        }
    }
}
