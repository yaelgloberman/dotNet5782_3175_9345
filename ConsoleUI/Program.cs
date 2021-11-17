using System;
using IDAL.DO;
using DAL.DalObject;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
namespace ConsoleUI
{
    public enum MainChoice { add = 1, update, display, list, exit };
    public enum CategoryChoice { STATION = 1, DRONE, CUSTOMER, PARCEL};
    public enum update { attribute = 1, PickUpPackageByDrone, DeliveryPackageCustomer, SendToCharge , releasingDrone };
    public enum  printLists{ STATION = 1, DRONE, CUSTOMER, PARCEL,DISMATCHED_PACKAGES, AVAILABLE_SLOTS }
    class Program
    {
      
        //IDAL.IDAl dal = new dalObject();
        static DalObject.DalObject Data;
        /// <summary>
        /// the program conatins the data of a drones the user enters a number form the menue and  based on his choice could access or change the data about the drones,station,customers or the parcels
        /// </summary>
     
        static void Main(string[] args)
        {
           //   = new DalObject.DalObject();       //how do i write this??

           	//dal.interface dal = new DalObject
            Data = new DalObject.DalObject();
            MainChoice choice;
            bool b;
            string s;
           
            do
            {
                
                Console.WriteLine("Menue: ");
                Console.WriteLine("press 1 to add an object");
                Console.WriteLine("press 2 to update");
                Console.WriteLine("press 3 for display");
                Console.WriteLine("press 4 to see lists ");
                Console.WriteLine("press 5 to exit");
                Console.WriteLine("enter a number between 1-5");
                s = Console.ReadLine();
     
                b = int.TryParse(s, out int error);
                int num;
                if (b)
                    num = int.Parse(s);
                else
                    num = -1;
                choice = (MainChoice)num;
                switch (choice)
                {

                    case MainChoice.add:
                        {
                            Data.MenuPrint("add");
                            bool isB;
                            string str = Console.ReadLine();
                            isB = int.TryParse(str, out int error1);
                            int num1;
                            CategoryChoice choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (CategoryChoice)num1;
                            switch (choice1) //add
                            {
                                
                                case CategoryChoice.STATION:
                                    {
                                        IDAL.DO.Station temp = new IDAL.DO.Station();
                                        int id, name, chargeSlots;
                                        double longitude, latitude;
                                        Console.WriteLine("enter a station id");
                                        int.TryParse(Console.ReadLine(), out id);
                                        temp.id = id;
                                        Console.WriteLine("enter station name");
                                        int.TryParse(Console.ReadLine(), out name);
                                        temp.name = name;
                                        Console.WriteLine("enter longitude");
                                        double.TryParse(Console.ReadLine(), out longitude);
                                        temp.longitude = longitude;
                                        Console.WriteLine("enter latitude");
                                        double.TryParse(Console.ReadLine(), out latitude);
                                        temp.latitude = latitude;
                                        Console.WriteLine("enter charge slots");
                                        int.TryParse(Console.ReadLine(), out chargeSlots);
                                        temp.chargeSlots = chargeSlots;
                                        try { Data.addStation(temp);}
                                        catch (IDAL.DO.AddException add) { Console.WriteLine(add.Message); }
                                        break;
                                    }
                                case CategoryChoice.DRONE:
                                    {
                                        int id;
                                        double bateryStatus;
                                        IDAL.DO.Drone temp = new IDAL.DO.Drone();
                                        Console.WriteLine("enter a drone  id");
                                        int.TryParse(Console.ReadLine(),out id);
                                        temp.id = id;
                                        Console.WriteLine("enter Model");
                                        temp.model = Console.ReadLine();
                                        Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
                                        temp.maxWeight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the batery status");
                                        double.TryParse(Console.ReadLine(),out bateryStatus);
                                        //temp.bateryStatus = bateryStatus;
                                        Console.WriteLine("enter the current drone status");//have to go over to see if i did it right
                                        //temp.status = (IDAL.DO.DroneStatuses)int.Parse(Console.ReadLine());
                                        try { Data.addDrone(temp); }
                                        catch (IDAL.DO.AddException add) { Console.WriteLine(add.Message); }
                                    }
                                    break;
                                case CategoryChoice.CUSTOMER:
                                    {
                                        int id, phoneNumber;
                                        double longitude, latitude;
                                        IDAL.DO.customer temp = new IDAL.DO.customer();
                                        Console.WriteLine("enter the customers id");
                                        int.TryParse(Console.ReadLine(),out id);
                                        temp.id = id;
                                        Console.WriteLine("enter the customers name");
                                        temp.name = Console.ReadLine();
                                        Console.WriteLine("enter the customers phonenumber");
                                        int.TryParse(Console.ReadLine(),out phoneNumber);
                                        temp.phoneNumber = phoneNumber;
                                        Console.WriteLine("enter the customers longitude");
                                        double.TryParse(Console.ReadLine(),out longitude);
                                        temp.longitude = longitude;
                                        Console.WriteLine("enter latitude");
                                        double.TryParse(Console.ReadLine(),out latitude);
                                        temp.latitude = latitude;
                                        try { Data.addCustomer(temp); }
                                        catch (IDAL.DO.AddException add) { Console.WriteLine(add.Message); }

                                    }
                                    break;
                                case CategoryChoice.PARCEL:
                                    {
                                        int parcelId,senderId, targetId, droneId;
                                        IDAL.DO.Parcel temp = new IDAL.DO.Parcel();
                                        Console.WriteLine("enter the parcel id");
                                        int.TryParse(Console.ReadLine(),out parcelId);
                                        temp.id= parcelId;
                                        Console.WriteLine("enter the senders id");
                                        int.TryParse(Console.ReadLine(),out senderId);
                                        temp.senderId = senderId;
                                        Console.WriteLine("enter the target id");
                                        int.TryParse(Console.ReadLine(),out targetId);
                                        temp.targetId = targetId;
                                        Console.WriteLine("enetr its urgency: press 1 for regular press 2 for fast and press 3 for emergency");                                   
                                        temp.priority = (IDAL.DO.Proirities)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the weight of the package");//not sure if i should do it with enum or have to do tkinut kelet
                                        temp.weight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the drone id ");
                                        int.TryParse(Console.ReadLine(),out droneId);
                                        temp.droneId = droneId;
                                       Console.WriteLine("enter the time the packge was requested");
                                        temp.requested = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was schedrules ");
                                        temp.scheduled = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was Picked up  ");
                                        temp.pickedUp = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was date time ");
                                        temp.delivered = DateTime.Parse(Console.ReadLine());
                                        try { Data.addParcel(temp); }
                                        catch (IDAL.DO.AddException add) { Console.WriteLine(add.Message); }
                                       
                                    }
                                    break;
                            }
                        }
                        break;
                    case MainChoice.update:
                        {
                            Console.WriteLine($"what would you like to update?");
                            Console.WriteLine($"enter 1 to match the parcel to a drone");
                            Console.WriteLine($"enter 2 to update parcel when it was picked up");
                            Console.WriteLine($"enter 3 to deliver parcel to the customer ");
                            Console.WriteLine($"enter 4 to send a drone to be charged ");
                            Console.WriteLine($"enter 5 to release a drone from a charging station");
                            bool isB;
                            string str = Console.ReadLine();
                            isB = int.TryParse(str, out int error1);
                            int num1;
                            update choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (update)num1;
                            switch (choice1)
                            {
                                case update.attribute:
                                    {
                                        Console.WriteLine("Enter the drone ID:\n");
                                        int droneId;
                                        int parcelId;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out droneId);
                                        Console.WriteLine("Enter the parcel ID to attribute: ");
                                        input = Console.ReadLine();
                                        int.TryParse(input, out parcelId);
                                        try { Data.attribute(droneId, parcelId); }
                                        catch (IDAL.DO.UpdateException update) { Console.WriteLine(update.Message); }   
                                    }
                                    break;
                                case update.PickUpPackageByDrone:
                                    {
                                        Console.WriteLine("Enter the drones ID:\n");
                                        int droneId;
                                        int parcelId;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out droneId);
                                        Console.WriteLine("Enter the parcel ID: ");
                                        input = Console.ReadLine();
                                        int.TryParse(input, out parcelId);
                                        try { Data.PickUpPackageByDrone(droneId, parcelId); }
                                        catch (IDAL.DO.UpdateException update) { Console.WriteLine(update.Message); }
                                    }
                                    break;
                                case update.DeliveryPackageCustomer:
                                    {
                                        int Cid, pId, proirity;
                                        Console.WriteLine("please enter your customer ID number");
                                        int.TryParse(Console.ReadLine(), out Cid);
                                        Console.WriteLine("please enter the ID number of your parcel");
                                        int.TryParse(Console.ReadLine(), out pId);
                                        Console.WriteLine("eneter level of priority");
                                        int.TryParse(Console.ReadLine(),out proirity);
                                        try { Data.DeliveryPackageCustomer(Cid, pId,(IDAL.DO.Proirities)proirity); }
                                        catch (IDAL.DO.UpdateException update) { Console.WriteLine(update.Message); }
                                    }
                                    break;
                                case update.SendToCharge:
                                    {
                                        int droneId, idStation;
                                        Console.WriteLine("enter your drone id:");
                                        int.TryParse(Console.ReadLine(),out droneId);
                                        Console.WriteLine("enter the station id that you want to charge your drone at  from the list below:");
                                        foreach (IDAL.DO.Station st in Data.stationList()) { if (st.chargeSlots > 0) Console.WriteLine(st.id + "\n"); }
                                        int.TryParse(Console.ReadLine(),out idStation);
                                        try { Data.SendToCharge(droneId, idStation); }
                                        catch (IDAL.DO.UpdateException update) { Console.WriteLine(update.Message); }
    
                                    }
                                    break;
                                case update.releasingDrone:
                                    {
                                        int dId;
                                        Console.WriteLine("please enter the drone ID number");
                                        int.TryParse(Console.ReadLine(),out dId);
                                        try { Data.releasingDrone(Data.findChargedDrone(dId));}
                                        catch (IDAL.DO.UpdateException update) { Console.WriteLine(update.Message); }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                        
                    case MainChoice.display:
                        {
                            Data.MenuPrint("display");
                            bool isB;
                            string str = Console.ReadLine();
                            isB = int.TryParse(str, out int error1);
                            int num1;
                            CategoryChoice choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (CategoryChoice)num1;
                            switch (choice1)
                            {
                                case CategoryChoice.STATION:
                                    {
                                        int stationID;
                                        Console.WriteLine("enter the station's id");
                                        int.TryParse(Console.ReadLine(),out stationID);
                                        IDAL.DO.Station? currentStation = null; 
                                        try { currentStation=Data.Station(stationID); }
                                        catch (IDAL.DO.findException find) { Console.WriteLine(find.Message); }
                                        Console.WriteLine(currentStation.ToString());
                                    }
                                    break;
                                case CategoryChoice.DRONE:
                                    {
                                        int droneID;
                                        Console.WriteLine("enter the drone's id");                                        
                                        int.TryParse(Console.ReadLine(), out droneID);
                                        IDAL.DO.Drone? currentDrone = null;
                                        try { currentDrone = Data.GetDrone(droneID); }
                                        catch (IDAL.DO.findException find) { Console.WriteLine(find.Message); }
                                        Console.WriteLine(currentDrone.ToString());
                                    }
                                    break;
                                case CategoryChoice.CUSTOMER:

                                    {
                                        int customerID;
                                        Console.WriteLine("enter the customer's id");
                                        int.TryParse(Console.ReadLine(), out customerID);
                                        IDAL.DO.customer? currentCustomer = null;
                                        currentCustomer = Data.GetCustomer(customerID);
                                        try { currentCustomer = Data.GetCustomer(customerID); }
                                        catch (IDAL.DO.findException find) { Console.WriteLine(find.Message); }
                                        Console.WriteLine(currentCustomer.ToString());
                                    }
                                    break;
                                case CategoryChoice.PARCEL:
                                    {
                                        int parcelID;
                                        Console.WriteLine("enter the parcel's id");
                                        int.TryParse(Console.ReadLine(), out parcelID);
                                        IDAL.DO.Parcel? currentParcel = null;
                                        try { currentParcel = Data.GetParcel(parcelID); }
                                        catch (IDAL.DO.findException find) { Console.WriteLine(find.Message); }
                                        Console.WriteLine(currentParcel.ToString());
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case MainChoice.list:
                        {
                            Data.MenuPrint("to print the list of");
                            Console.WriteLine($"enter 5 to o print the list of the parcels that arn't matched with a drone");
                            Console.WriteLine($"enter 6 to o print the list of the stations that have available charging slots");
                            Console.WriteLine("");
                            bool isB;
                            string str = Console.ReadLine();
                            isB = int.TryParse(str, out int error1);
                            int num1;
                            printLists choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (printLists)num1;
                            switch (choice1)
                            {
                                case printLists.STATION:
                                    {
                                        foreach (IDAL.DO.Station st in Data.stationList()) { Console.WriteLine(st.ToString() + "\n"); }
                                    }
                                    break;
                                case printLists.DRONE:
                                    {
                                        foreach (IDAL.DO.Drone d in Data.droneList()){ Console.WriteLine(d.ToString() + "\n"); }
                                    }
                                    break;
                                case printLists.CUSTOMER:
                                    {
                                        foreach (IDAL.DO.customer c in Data.customerList()) { Console.WriteLine(c.ToString() + "\n"); }                                    }
                                    break;
                                case printLists.PARCEL:
                                    {
                                        foreach (IDAL.DO.Parcel p in Data.parcelList()) { Console.WriteLine(p.ToString() + "\n"); }
                                    }
                                    break;
                                case printLists.DISMATCHED_PACKAGES:
                                    {
                                        foreach (IDAL.DO.Parcel p in Data.parcelList()) { if(p.droneId==0)Console.WriteLine(p.id.ToString() + "\n"); }                                    }
                                    break;
                                case printLists.AVAILABLE_SLOTS:
                                    {
                                        foreach (IDAL.DO.Station st in Data.stationList()) { if (st.chargeSlots > 0) Console.WriteLine(st.ToString() + "\n"); }
                                    }
                                    break;
                                default:
                                    break;
                            }     

                        }
                        break;
                    case MainChoice.exit:
                        break;
                    default:
                        break;
                 
                } 
            } while (choice != (MainChoice.exit));
        }
    }
}