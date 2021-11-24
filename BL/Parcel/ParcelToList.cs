using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList
    {
        public int id { set; get; }
        public int senderName { set; get; }
        public int reciveName { set; get; }
        public Weight weight { set; get; }
        public Priority priority { set; get; }
        public ParcelStatus parcelStatus { set; get; }

    }
}
