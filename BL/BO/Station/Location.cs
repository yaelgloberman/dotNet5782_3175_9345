using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Formats;
namespace BO
{
    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public override string ToString()
        {
            return string.Format($" latitude: {DO.help.getBase60Lng(latitude)}, longitude: {DO.help.getBase60Lng(longitude)} ");
        }

    }
}