using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
{
    public class DroneInParcel
    {
        public int id { get; set; }
        public double battery { get; set; }
        public Location location { get; set; }
        public override string ToString()
        {
            return string.Format($"drone id:{id}, battery: {battery}$, location: {location} ");
        }
    }
}
