using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    class ParcelByCustomer
    {
        public int id { set; get; }
        public Weight weight { set; get; }
        public Priority priority { set; get; }
        public ParcelStatus parcelStatus { set; get; }
        public CustomerInParcel CustomerInParcel { get; set; }

    }
}
