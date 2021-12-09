using System;

namespace IBL.BO
{
    public class Parcel
    {
        public int id { get; set; }
        public CustomerInParcel sender { get; set; }
        public CustomerInParcel receive{ get; set; }
        public  Weight weightCategorie { get; set; }
        public Priority priority { get; set; }
        public DroneInParcel droneInParcel { get; set; }
        public DateTime requested { set; get; }//1-יצירה
        public DateTime scheduled { set; get; }//2-שיוך???
        public DateTime delivered { set; get; }//3-איסוף
        public DateTime pickedUp { set; get; }//4-אספקה 
        public override string ToString()
        {
            return string.Format($"Id: {id}, Sender Id:\n {sender}, receiver Id:\n {receive}, Priority: {priority}, Drone in parcel: {droneInParcel},  Weight Catigory: {weightCategorie}, Requested: {requested}, Scheduled: {scheduled}, PickedUp: {pickedUp}, Datetime: {delivered}  ");
            
        }
    }
}
