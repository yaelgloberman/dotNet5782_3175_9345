using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class CustomerInList
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int  PhoneNumber { get; set; }
        public int Parcles_Delivered_Recieved { get; set; }
        public int Parcels_unrecieved { get; set; }
        public int Recieved_Parcels { get; set; }
        public int ParcelsInDeliver { get; set; }
    }
}
