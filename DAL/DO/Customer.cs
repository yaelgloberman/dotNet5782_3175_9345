using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
    namespace DO
    {
        public struct Customer   
        {
            public int id { set; get; }
            public string name { set; get; }
            public string phoneNumber { set; get; }
            public double longitude { set; get; }
            public double latitude { set; get; }
            public bool isActive { get; set; }
        public string Password { get; set; }
        public bool isCustomer { get; set; }

        public override string ToString()
            { 
                return string.Format($"id: {id}, Name: {name}, Phone Number: 05{phoneNumber}, Longitude: { DO.help.getBase60Lng(longitude)}  Latitude: {DO.help.getBase60Lat(latitude)} ");

            }
        }
        
    }