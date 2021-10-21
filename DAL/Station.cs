using System;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { set; get; }
            public int name{ set; get; }
            public double Longitude { set; get; }
            public double Latitude { set; get; }
            public int ChargeSlots { set; get; }
        }
    }
}
