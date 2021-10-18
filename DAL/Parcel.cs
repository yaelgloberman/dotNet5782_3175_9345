using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            int Id;
            int SenderId;
            int TargetId;
            WeightCatigories Weight;
            Proirities Priority;
            datetime Requested;
            int DronrId;
            datetime Scheduled;
            datetime PickedUp;
            datetime Datetime;
        }
    }
}
