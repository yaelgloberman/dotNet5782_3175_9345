using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL
{
    namespace DO
    {
        public struct Customer   
        {
            public int id { set; get; }
            public string name { set; get; }
            public int phoneNumber { set; get; }
            public double longitude { set; get; }
            public double latitude { set; get; }
            public override string ToString()
            { 
                return string.Format($"id: {id}, Name: {name}, Phone Number: 05{phoneNumber}, Longitude: { IDAL.DO.help.getBase60Lng(longitude)}  Latitude: {IDAL.DO.help.getBase60Lat(latitude)} ");

            }
        }
        
    }
}
