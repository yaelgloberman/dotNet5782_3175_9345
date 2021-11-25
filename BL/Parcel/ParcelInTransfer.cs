﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelInTransfer
    {
        public int id { get; set; }
        public bool parcelStatus { get; set; }
        public Priority priority { get; set; }
        public Weight weight { get; set; }
        public CustomerInParcel sender { set; get; }
        public CustomerInParcel recive { set; get; }
        public Location collection { get; set; }
        public Location DeliveryDestination { get; set; }
        public double TransportDistance { get; set; }
    }
}