using System;

namespace IBL.BO
{
    public class Parcel
    {
        public int id { get; set; }
        public CustomerInParcel sender { get; set; }
        public CustomerInParcel recive{ get; set; }
        public  Weight weightCategorie { get; set; }
        public Priority priority { get; set; }
        public DroneInParcel droneInParcel { get; set; }
        public DateTime requested { set; get; }
        public DateTime scheduled { set; get; }
        public DateTime pickedUp { set; get; }
        public DateTime delivered { set; get; }





    }
}
