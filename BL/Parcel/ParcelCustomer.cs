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
        public Weight weight { get; set; }
        public Priority priority { get; set; }
        public ParcelStatus parcelStatus { get; set; }
        public CustomerInParcel CustomerInParcel { get; set; }
    }
}
