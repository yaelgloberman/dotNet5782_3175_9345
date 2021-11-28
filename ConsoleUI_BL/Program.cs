using System;
using IBL.BO;
namespace ConsoleUI_BL
{
    public enum MainChoice { station,drone,customer,parcel}
    class Program
    {
         static IBL.BO.IBl bl = new BL.Blobject();
         static IDAL.DO.IDal dal = new DalObject.DalObject();
        static void main(string[] args)
        {
            bool b;
            string s;
            MainChoice choice;
            b = int.TryParse(s, out int error);
            int num;
            if (b)
                num = int.Parse(s);
            else
                num = -1;
            choice = (MainChoice)num;
            switch (choice)
            {
                case MainChoice.station:
                    {
                        IBL.BO.BaseStation temp = new IBL.BO.BaseStation();
                        int id, name, chargeSlots;
                        double longitude, latitude;
                        IBL.BO.Location stationLoc = new Location();
                        Console.WriteLine("enter a station id");
                        int.TryParse(Console.ReadLine(), out id);
                        temp.id = id;
                        Console.WriteLine("enter station name");
                        int.TryParse(Console.ReadLine(), out name);
                        temp.stationName = name;
                        Console.WriteLine("enter longitude");
                        double.TryParse(Console.ReadLine(), out longitude);
                        stationLoc.longitude = longitude;
                        Console.WriteLine("enter latitude");
                        double.TryParse(Console.ReadLine(), out latitude);
                        stationLoc.latitude = latitude;
                        temp.location = stationLoc;
                        Console.WriteLine("enter charge slots");
                        int.TryParse(Console.ReadLine(), out chargeSlots);
                        temp.avilableChargeSlots = chargeSlots;
                        try { bl.addStation(temp); }
                        catch (IBL.BO.AddException add) { Console.WriteLine(add.Message); }
                        break;
                    }

                    break;
                case MainChoice.drone:
                    {
                        int id;
                        double bateryStatus;
                        IBL.BO.Drone temp = new IBL.BO.Drone();
                        Console.WriteLine("enter a drone  id");
                        int.TryParse(Console.ReadLine(), out id);
                        temp.id = id;
                        Console.WriteLine("enter Model");
                        temp.droneModel = int.Parse(Console.ReadLine());
                        Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
                        temp.weight = (IBL.BO.Weight)int.Parse(Console.ReadLine());
                        Console.WriteLine("enter the batery status");
                        double.TryParse(Console.ReadLine(), out bateryStatus);
                        temp.batteryStatus = bateryStatus;
                        Console.WriteLine("enter the current drone status");//have to go over to see if i did it right
                                                                            //temp.status = (IDAL.DO.DroneStatuses)int.Parse(Console.ReadLine());
                        try {bl.addDrone(temp); }
                        catch (IDAL.DO.AddException add) { Console.WriteLine(add.Message); }
                    }
                    break;
                case MainChoice.customer:
                    {
                        IBL.BO.Customer temp=new IBL.BO.Customer();
                        int id, phoneNumber;
                        double longitude, latitude;
                        string name;
                        IBL.BO.Location customerLoc = new Location();
                        Console.WriteLine("enter the Customers id");
                        int.TryParse(Console.ReadLine(), out id);
                        temp.id= id;
                        Console.WriteLine("enter the Customers name");
                        temp.Name = Console.ReadLine();
                        Console.WriteLine("enter the Customers phonenumber");
                        int.TryParse(Console.ReadLine(), out phoneNumber);
                        temp.phoneNumber = phoneNumber;
                        Console.WriteLine("enter the Customers longitude");
                        double.TryParse(Console.ReadLine(), out longitude);
                        customerLoc.longitude = longitude;
                        Console.WriteLine("enter latitude");
                        double.TryParse(Console.ReadLine(), out latitude);
                        customerLoc.latitude = latitude;
                        temp.location = customerLoc;
                        try { bl.addDrone(temp); }
                        catch (IBL.BO.AddException add) { Console.WriteLine(add.Message); }
                    }
                    break;
                case MainChoice.parcel:
                    {
                        int parcelId, senderId, targetId, droneId;
                        
                        IBL.BO.Parcel temp = new IBL.BO.Parcel();
                        IBL.BO.CustomerInParcel CustomerInParcel_sender = new CustomerInParcel();
                        IBL.BO.CustomerInParcel CustomerInParcel_receiver = new CustomerInParcel();
                        IBL.BO.DroneInParcel DroneInParcel_Status = new DroneInParcel();
                        Console.WriteLine("enter the parcel id");
                        int.TryParse(Console.ReadLine(), out parcelId);
                        temp.id = parcelId;
                        Console.WriteLine("enter the senders id");
                        int.TryParse(Console.ReadLine(), out senderId);
                        CustomerInParcel_sender.id = senderId;
                        Console.WriteLine("enter the senders name");
                        CustomerInParcel_sender.name = Console.ReadLine();                       
                        Console.WriteLine("enter the reciever id");
                        int.TryParse(Console.ReadLine(), out targetId);
                        CustomerInParcel_sender.id = targetId;
                        temp.sender = CustomerInParcel_sender;
                        temp.receive = CustomerInParcel_receiver;
                        Console.WriteLine("enetr its urgency: press 1 for regular press 2 for fast and press 3 for emergency");
                        temp.priority = (IBL.BO.Priority)int.Parse(Console.ReadLine());
                        Console.WriteLine("enter the weight of the package");//not sure if i should do it with enum or have to do tkinut kelet
                        temp.weightCategorie = (IBL.BO.Weight)int.Parse(Console.ReadLine());
                        Console.WriteLine("enter the drone id ");
                        int.TryParse(Console.ReadLine(), out droneId);
                        DroneInParcel_Status.id = droneId;
                        DroneInParcel_Status.battery = bl.getDrone(droneId).battery;
                        DroneInParcel_Status.location = bl.getDrone(droneId).location;
                        temp.droneInParcel = DroneInParcel_Status;
                        Console.WriteLine("enter the time the packge was requested");
                        temp.requested = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("enter the time it was schedrules ");
                        temp.scheduled = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("enter the time it was Picked up  ");
                        temp.pickedUp = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("enter the time it was date time ");
                        temp.delivered = DateTime.Parse(Console.ReadLine());
                        try { bl.addParcel(temp);}
                        catch (IBL.BO.AddException add) { Console.WriteLine(add.Message); }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
