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
            public int id { set; get; }
            public string model { set; get; }
            public WeightCatigories maxWeight { set; get; }
            public double bateryStatus { set; get; }    
            public DroneStatuses status { set; get; }
            public override string ToString()
            {
                return string.Format($"id: {id}, Model: {model}, the max weight: {maxWeight}, Batery Status: {bateryStatus}  status: {status} ");

            }
        }
    }
}
