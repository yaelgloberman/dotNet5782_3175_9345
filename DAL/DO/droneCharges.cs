using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DO
{
        public struct droneCharges
        {
            public int droneId { set; get; }

            public int stationId { set; get; }
            public DateTime enterToCharge { set; get; }
        }
    }

