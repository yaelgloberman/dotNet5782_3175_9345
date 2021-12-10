using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationToList
    {
        public int id { get; set; }
        public string stationName { set; get; }
        public int avilableChargeSlots { get; set; }
        public int unavilableChargeSlots { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, station Name: {stationName}, avilable ChargeSlots: {avilableChargeSlots}, unavilable Charge Slots:{unavilableChargeSlots}");
        }
    }
}

