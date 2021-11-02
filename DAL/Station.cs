﻿using System;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int id { set; get; }
            public int name{ set; get; }
            public double longitude { set; get; }
            public double latitude { set; get; }
            public int chargeSlots { set; get; }
            public override string ToString()
            {
                return string.Format($"id: {id}, Name: {name},  Longitude: { IDAL.DO.help.getBase60Lng(longitude)}  Latitude: {IDAL.DO.help.getBase60Lat(latitude)} ");
            }
        }
  

    }
}
