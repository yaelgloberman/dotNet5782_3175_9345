using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelCustomer
    {
        public int id { get; set; }
        public Weight Weight { get; set; }
        public Priority Priority { get; set; }
        public CustomerInParcel CustomerInParcel { get; set; }
    }
}
