using System;

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
        }
        
    }
}
