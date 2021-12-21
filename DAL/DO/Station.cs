using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DO
    {
        public struct Station
        {
            public int id { set; get; }
            public string name{ set; get; }
            public double longitude { set; get; }
            public double latitude { set; get; }
            public int chargeSlots { set; get; }
            public void addingChargeSlot() { chargeSlots++; }
            public override string ToString()
            {
                return string.Format($"id: {id}, Name: {name},  Longitude: { DO.help.getBase60Lng(longitude)}, Latitude: {DO.help.getBase60Lat(latitude)}, charge Slots: {chargeSlots} ");
            }
        }
  

    }

