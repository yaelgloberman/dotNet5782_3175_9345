using System;

namespace IDAL
{
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
                return string.Format($"id: {id}, Name: {name},  Longitude: { IDAL.DO.help.getBase60Lng(longitude)}, Latitude: {IDAL.DO.help.getBase60Lat(latitude)}, charge Slots: {chargeSlots} ");
            }
        }
  

    }
}
