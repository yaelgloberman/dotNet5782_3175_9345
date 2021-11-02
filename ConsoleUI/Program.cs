using System;
namespace ConsoleUI
{
    public enum MainChoice { add = 1, update, display, list, exit };
    public enum CategoryChoice { STATION = 1, DRONE, CUSTOMER, PARCEL};
    public enum update { attribute = 1, PickUpPackageByDrone, DeliveryPackageCustomer, SendToCharge , releasingDrone };
    public enum  printLists{ STATION = 1, DRONE, CUSTOMER, PARCEL,DISMATCHED_PACKAGES, AVAILABLE_SLOTS }
  
    class Program
    {
       static  DalObject.DalObject Data;
        /// <summary>
        /// the program conatins the data of a drones the user enters a number form the menue and  based on his choice could access or change the data about the drones,station,customers or the parcels
        /// </summary>
     
        
        static void Main(string[] args)
        {
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
                                        int.TryParse(Console.ReadLine(),out id);
                                        temp.id = id;
                                        Console.WriteLine("enter station name");
                                        int.TryParse(Console.ReadLine(),out name);
                                        temp.name = name;
                                        Console.WriteLine("enter longitude");
                                        double.TryParse(Console.ReadLine(),out longitude);
                                        temp.longitude = longitude;
                                        Console.WriteLine("enter latitude");
                                        double.TryParse(Console.ReadLine(), out latitude);
                                        temp.latitude = latitude;
                                        Console.WriteLine("enter charge slots");
                                        int.TryParse(Console.ReadLine(), out chargeSlots );
                                        temp.chargeSlots = chargeSlots;
                                        Data.addStation(temp);
                                    }
                                    break;
                                case CategoryChoice.DRONE:
                                    {
                                        IDAL.DO.Drone temp = new IDAL.DO.Drone();
                                        Console.WriteLine("enter a drone  id");
                                        temp.id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter Model");
                                        temp.model = Console.ReadLine();
                                        Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
                                        temp.maxWeight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the batery status");
                                        temp.bateryStatus = double.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the current drone status");//have to go over to see if i did it right
                                        temp.status = (IDAL.DO.DroneStatuses)int.Parse(Console.ReadLine());
                                        Data.addDrone(temp);

      
                                    }
                                    break;
                                case CategoryChoice.CUSTOMER:
                                    {
                                        IDAL.DO.customer temp = new IDAL.DO.customer();
                                        Console.WriteLine("enter the customers id");
                                        temp.id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the customers name");
                                        temp.name = Console.ReadLine();
                                        Console.WriteLine("enter the customers phonenumber");
                                        temp.phoneNumber = int.Parse(Console.ReadLine());//might have to check validity of 05 
                                        Console.WriteLine("enter the customers longitude");
                                        temp.longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("enter latitude");
                                        temp.latitude = double.Parse(Console.ReadLine());
                                        Data.addCustomer(temp);

                                    }
                                    break;
                                case CategoryChoice.PARCEL:
                                    {
                                        IDAL.DO.Parcel temp = new IDAL.DO.Parcel();
                                        Console.WriteLine("enter the parcel id");
                                        temp.id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the senders id");
                                        temp.senderId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the target id");
                                        temp.targetId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enetr its urgency: press 1 for regular press 2 for fast and press 3 for emergency");                                   
                                        temp.priority = (IDAL.DO.Proirities)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the weight of the package");//not sure if i should do it with enum or have to do tkinut kelet
                                        temp.weight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the drone id ");
                                        temp.droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time the packge was requested");
                                        temp.requested = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was schedrules ");
                                        temp.scheduled = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was Picked up  ");
                                        temp.pickedUp = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was date time ");
                                        temp.delivered = DateTime.Parse(Console.ReadLine());
                                        Data.addParcel(temp);
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
                                        Data.attribute(droneId, parcelId);  
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
                                        Data.PickUpPackageByDrone(droneId, parcelId);
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
                                        Data.DeliveryPackageCustomer(Cid,pId, (IDAL.DO.Proirities)proirity);
                                    }
                                    break;
                                case update.SendToCharge:
                                    {
                                        int droneId, idStation;
                                        Console.WriteLine("enter your drone id:");
                                        int.TryParse(Console.ReadLine(),out droneId);
                                        Console.WriteLine("enter the station id that you want to charge your drone at  from the list below:");
                                        Data.stationList().ForEach(s => { if (s.chargeSlots > 0) Console.WriteLine(s.id+"\n"); });                                       
                                        int.TryParse(Console.ReadLine(),out idStation);
                                        Data.SendToCharge(droneId,idStation);
                                    }
                                    break;
                                case update.releasingDrone:
                                    {
                                        Console.WriteLine("please enter the dones ID number");
                                        int dID = int.Parse(Console.ReadLine());
                                        Data.releasingDrone(Data.findChargedDrone(dID));

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
                                        IDAL.DO.Station currentStation = new IDAL.DO.Station();
                                        currentStation = Data.findStation(stationID);
                                        if (currentStation.id == 0)
                                            Console.WriteLine("this station dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentStation.ToString());
                                        }
                                    }
                                    break;
                                case CategoryChoice.DRONE:
                                    {
                                        int droneID;
                                        Console.WriteLine("enter the drone's id");                                        
                                        int.TryParse(Console.ReadLine(), out droneID);
                                        IDAL.DO.Drone currentDrone = new IDAL.DO.Drone();
                                        currentDrone = Data.findDrone(droneID);
                                        if (currentDrone.id == 0)
                                            Console.WriteLine("this drone dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentDrone.ToString());
                                        }
                                    }
                                    break;
                                case CategoryChoice.CUSTOMER:

                                    {
                                        int customerID;
                                        Console.WriteLine("enter the customer's id");
                                        int.TryParse(Console.ReadLine(), out customerID);
                                        IDAL.DO.customer currentCustomer = new IDAL.DO.customer();
                                        currentCustomer = Data.findCustomer(customerID);
                                        if (currentCustomer.id == 0)
                                            Console.WriteLine("this customer dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentCustomer.ToString());
                                        }
                                    }
                                    break;
                                case CategoryChoice.PARCEL:
                                    {
                                        int parcelID;
                                        Console.WriteLine("enter the parcel's id");
                                        int.TryParse(Console.ReadLine(), out parcelID);
                                        IDAL.DO.Parcel currentParcel = new IDAL.DO.Parcel();
                                        currentParcel = Data.findParcel(parcelID);
                                        if (currentParcel.id == 0)
                                            Console.WriteLine("this customer dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentParcel.ToString());
                                        }
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
                                        Data.stationList().ForEach(s => { Console.WriteLine(s.ToString()+ "\n"); });
                                    }
                                    break;
                                case printLists.DRONE:
                                    {
                                        Data.droneList().ForEach(s => { Console.WriteLine(s.ToString()+ "\n"); });
                                    }
                                    break;
                                case printLists.CUSTOMER:
                                    {
                                        Data.customerList().ForEach(s => { Console.WriteLine(s.ToString()+ "\n"); });
                                    }
                                    break;
                                case printLists.PARCEL:
                                    {
                                        Data.parcelList().ForEach(s => { Console.WriteLine(s.ToString()+ "\n"); });
                                    }
                                    break;
                                case printLists.DISMATCHED_PACKAGES:
                                    {
                                        Data.parcelList().ForEach(p => { if (p.droneId == 0) Console.WriteLine(p.id + "\n"); });
                                    }
                                    break;
                                case printLists.AVAILABLE_SLOTS:
                                    {
                                        Data.stationList().ForEach(s => { if (s.chargeSlots > 0) Console.WriteLine(s.id+ "\n"); });
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