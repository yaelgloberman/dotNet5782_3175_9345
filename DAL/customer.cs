using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct customer
        {
            public int Id { set; get; }
            public string Name { set; get; }
            public int PhoneNumber { set; get; }
            public double Longitude { set; get; }
            public double Latitude { set; get; }
            public override string ToString()
            {
                return string.Format($"id: {Id}, Name: {Name}, Phone Number: 05{PhoneNumber}, Longitude: {Longitude}  Latitude: {Latitude} ");

            }
        }
    }
}
