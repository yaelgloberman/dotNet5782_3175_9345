﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DalApi;
//using DO;
//namespace ConsoleUI
//{
//    public enum MainChoice { add = 1, update, display, list, exit };
//    public enum CategoryChoice { STATION = 1, DRONE, Customer, PARCEL };
//    public enum update { attribute = 1, PickUpPackageByDrone, DeliveryPackageCustomer, SendToCharge, releasingDrone };
//    public enum printLists { STATION = 1, DRONE, Customer, PARCEL, DISMATCHED_PACKAGES, AVAILABLE_SLOTS }
//    class Program
//    {
//        //static IDal dal = new DalObject();
//        /// <summary>
//        /// the program conatins the dal of a drones the user enters a number form the menue and  based on his choice could access or change the dal about the drones,station,Customers or the parcels
//        /// </summary>
//        static void Main(string[] args)
//        { 
//            MainChoice choice;
//            bool b;
//            string s;

//            do
//            {
//                Console.WriteLine("Menue: ");
//                Console.WriteLine("press 1 to add an object");
//                Console.WriteLine("press 2 to update");
//                Console.WriteLine("press 3 for display");
//                Console.WriteLine("press 4 to see lists ");
//                Console.WriteLine("press 5 to exit");
//                Console.WriteLine("enter a number between 1-5");
//                s = Console.ReadLine();
//                b = int.TryParse(s, out int error);
//                int num;
//                if (b)
//                    num = int.Parse(s);
//                else
//                    num = -1;
//                choice = (MainChoice)num;
//                switch (choice)
//                {

//                    case MainChoice.add:
//                        {
//                            Console.WriteLine($"what would you like to add?");
//                            Console.WriteLine($"enter 1 to add station");
//                            Console.WriteLine($"enter 2 to add drones");
//                            Console.WriteLine($"enter 3 to add Customers");
//                            Console.WriteLine($"enter 4 to add parcel");                          
//                            bool isB;
//                            string str = Console.ReadLine();
//                            isB = int.TryParse(str, out int error1);
//                            int num1;
//                            CategoryChoice choice1;
//                            if (isB)
//                                num1 = int.Parse(str);
//                            else
//                                num1 = -1;
//                            choice1 = (CategoryChoice)num1;
//                            switch (choice1) //add
//                            {

//                                case CategoryChoice.STATION:
//                                    {
//                                        DO.Station temp = new DO.Station();
//                                        int id, chargeSlots;
//                                        string name;
//                                        double longitude, latitude;
//                                        Console.WriteLine("enter a station id");
//                                        int.TryParse(Console.ReadLine(), out id);
//                                        temp.id = id;
//                                        Console.WriteLine("enter station name");
//                                        temp.name = Console.ReadLine();
//                                        Console.WriteLine("enter longitude");
//                                        double.TryParse(Console.ReadLine(), out longitude);
//                                        temp.longitude = longitude;
//                                        Console.WriteLine("enter latitude");
//                                        double.TryParse(Console.ReadLine(), out latitude);
//                                        temp.latitude = latitude;
//                                        Console.WriteLine("enter charge slots");
//                                        int.TryParse(Console.ReadLine(), out chargeSlots);
//                                        temp.chargeSlots = chargeSlots;
//                                        try { dal.addStation(temp); }
//                                        catch (DO.AddException add) { Console.WriteLine(add.Message); }
//                                        break;
//                                    }
//                                case CategoryChoice.DRONE:
//                                    {
//                                        int id;
//                                        double bateryStatus;
//                                        DO.Drone temp = new DO.Drone();
//                                        Console.WriteLine("enter a drone  id");
//                                        int.TryParse(Console.ReadLine(), out id);
//                                        temp.id = id;
//                                        Console.WriteLine("enter Model");
//                                        temp.model = Console.ReadLine();
//                                        Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
//                                        temp.maxWeight = (DO.WeightCatigories)int.Parse(Console.ReadLine());
//                                        Console.WriteLine("enter the batery status");
//                                        double.TryParse(Console.ReadLine(), out bateryStatus);
//                                        //temp.bateryStatus = bateryStatus;
//                                        Console.WriteLine("enter the current drone status");//have to go over to see if i did it right
//                                        //temp.status = (DO.DroneStatuses)int.Parse(Console.ReadLine());
//                                        try { dal.addDrone(temp); }
//                                        catch (DO.AddException add) { Console.WriteLine(add.Message); }
//                                    }
//                                    break;
//                                case CategoryChoice.Customer:
//                                    {
//                                        int id, phoneNumber;
//                                        double longitude, latitude;
//                                        DO.Customer temp = new DO.Customer();
//                                        Console.WriteLine("enter the Customers id");
//                                        int.TryParse(Console.ReadLine(), out id);
//                                        temp.id = id;
//                                        Console.WriteLine("enter the Customers name");
//                                        temp.name = Console.ReadLine();
//                                        Console.WriteLine("enter the Customers phonenumber");
//                                        int.TryParse(Console.ReadLine(), out phoneNumber);
//                                        temp.phoneNumber = phoneNumber;
//                                        Console.WriteLine("enter the Customers longitude");
//                                        double.TryParse(Console.ReadLine(), out longitude);
//                                        temp.longitude = longitude;
//                                        Console.WriteLine("enter latitude");
//                                        double.TryParse(Console.ReadLine(), out latitude);
//                                        temp.latitude = latitude;
//                                        try { dal.addCustomer(temp); }
//                                        catch (DO.AddException add)
//                                        {
//                                            Console.WriteLine(add.Message);
//                                        }
//                                        break;
//                                    }
//                                case CategoryChoice.PARCEL:
//                                    {
//                                        int  senderId, targetId, droneId;
//                                        DO.Parcel temp = new DO.Parcel();
//                                        temp.id = 0;
//                                        Console.WriteLine("enter the senders id");
//                                        int.TryParse(Console.ReadLine(), out senderId);
//                                        temp.senderId = senderId;
//                                        Console.WriteLine("enter the target id");
//                                        int.TryParse(Console.ReadLine(), out targetId);
//                                        temp.targetId = targetId;
//                                        Console.WriteLine("enetr its urgency: press 1 for regular press 2 for fast and press 3 for emergency");
//                                        temp.priority = (DO.Proirities)int.Parse(Console.ReadLine());
//                                        Console.WriteLine("enter the weight of the package");//not sure if i should do it with enum or have to do tkinut kelet
//                                        temp.weight = (DO.WeightCatigories)int.Parse(Console.ReadLine());
//                                        Console.WriteLine("enter the drone id ");
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        temp.droneId = droneId;
//                                        Console.WriteLine("enter the time the packge was requested");
//                                        temp.requested = DateTime.Parse(Console.ReadLine());
//                                        Console.WriteLine("enter the time it was schedrules ");
//                                        temp.scheduled = DateTime.Parse(Console.ReadLine());
//                                        Console.WriteLine("enter the time it was Picked up  ");
//                                        temp.pickedUp = DateTime.Parse(Console.ReadLine());
//                                        Console.WriteLine("enter the time it was date time ");
//                                        temp.delivered = DateTime.Parse(Console.ReadLine());
//                                        try { Console.WriteLine(dal.addParcel(temp));  }
//                                        catch (DO.AddException add) { Console.WriteLine(add.Message); }
//                                    }
//                                    break;
//                            }
//                        }
//                        break;
//                    case MainChoice.update:
//                        {
//                            Console.WriteLine($"what would you like to update?");
//                            Console.WriteLine($"enter 1 to match the parcel to a drone");
//                            Console.WriteLine($"enter 2 to update parcel when it was picked up");
//                            Console.WriteLine($"enter 3 to deliver parcel to the Customer ");
//                            Console.WriteLine($"enter 4 to send a drone to be charged ");
//                            Console.WriteLine($"enter 5 to release a drone from a charging station");
//                            bool isB;
//                            string str = Console.ReadLine();
//                            isB = int.TryParse(str, out int error1);
//                            int num1;
//                            update choice1;
//                            if (isB)
//                                num1 = int.Parse(str);
//                            else
//                                num1 = -1;
//                            choice1 = (update)num1;
//                            switch (choice1)
//                            {
//                                case update.attribute:
//                                    {
//                                        Console.WriteLine("Enter the drone ID:\n");
//                                        int droneId;
//                                        int parcelId;
//                                        string input = Console.ReadLine();
//                                        int.TryParse(input, out droneId);
//                                        Console.WriteLine("Enter the parcel ID to attribute: ");
//                                        input = Console.ReadLine();
//                                        int.TryParse(input, out parcelId);
//                                        try { dal.attribute(droneId, parcelId); }
//                                        catch (findException find) { Console.WriteLine(find.Message); }
//                                    }
//                                    break;
//                                case update.PickUpPackageByDrone:
//                                    {
//                                        Console.WriteLine("Enter the drones ID:\n");
//                                        int droneId;
//                                        int parcelId;
//                                        string input = Console.ReadLine();
//                                        int.TryParse(input, out droneId);
//                                        Console.WriteLine("Enter the parcel ID: ");
//                                        input = Console.ReadLine();
//                                        int.TryParse(input, out parcelId);
//                                        try { dal.PickUpPackageByDrone(droneId, parcelId); }
//                                        catch (findException find) { Console.WriteLine(find.Message); }
//                                    }
//                                    break;
//                                case update.DeliveryPackageCustomer:
//                                    {
//                                        int Cid, pId, proirity;
//                                        Console.WriteLine("please enter your Customer ID number");
//                                        int.TryParse(Console.ReadLine(), out Cid);
//                                        Console.WriteLine("please enter the ID number of your parcel");
//                                        int.TryParse(Console.ReadLine(), out pId);
//                                        Console.WriteLine("eneter level of priority");
//                                        int.TryParse(Console.ReadLine(), out proirity);
//                                        try { dal.DeliveryPackageCustomer(Cid, pId, (DO.Proirities)proirity); }
//                                        catch (findException find) { Console.WriteLine(find.Message); }
//                                    }
//                                    break;
//                                case update.SendToCharge:
//                                    {
//                                        int droneId, idStation;
//                                        Console.WriteLine("enter your drone id:");
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        Console.WriteLine("enter the station id that you want to charge your drone at  from the list below:");
//                                        foreach (DO.Station st in dal.GetStationList()) { if (st.chargeSlots > 0) Console.WriteLine(st.id + "\n"); }
//                                        int.TryParse(Console.ReadLine(), out idStation);
//                                        try { dal.SendToCharge(droneId, idStation); }
//                                        catch (findException find) { Console.WriteLine(find.Message); }
//                                    }
//                                    break;
//                                case update.releasingDrone:
//                                    {
//                                        int dId;
//                                        Console.WriteLine("please enter the drone ID number");
//                                        int.TryParse(Console.ReadLine(), out dId);
//                                        try { dal.releasingDrone(dal.GetChargedDrone(dId)); }
//                                        catch (findException find) { Console.WriteLine(find.Message); }
//                                    }
//                                    break;
//                                default:
//                                    break;
//                            }
//                            break;
//                        }

//                    case MainChoice.display:
//                        {
//                            Console.WriteLine($"what would you like to add?");
//                            Console.WriteLine($"enter 1 to display station");
//                            Console.WriteLine($"enter 2 to display drones");
//                            Console.WriteLine($"enter 3 to display Customers");
//                            Console.WriteLine($"enter 4 to display parcel");
//                            bool isB;
//                            string str = Console.ReadLine();
//                            isB = int.TryParse(str, out int error1);
//                            int num1;
//                            CategoryChoice choice1;
//                            if (isB)
//                                num1 = int.Parse(str);
//                            else
//                                num1 = -1;
//                            choice1 = (CategoryChoice)num1;
//                            switch (choice1)
//                            {
//                                case CategoryChoice.STATION:
//                                    {
//                                        int stationID;
//                                        Console.WriteLine("enter the station's id");
//                                        int.TryParse(Console.ReadLine(), out stationID);
//                                        DO.Station? currentStation = null;
//                                        try { currentStation = dal.GetStation(stationID); }
//                                        catch (DO.findException find) { Console.WriteLine(find.Message); }
//                                        Console.WriteLine(currentStation.ToString());
//                                    }
//                                    break;
//                                case CategoryChoice.DRONE:
//                                    {
//                                        int droneID;
//                                        Console.WriteLine("enter the drone's id");
//                                        int.TryParse(Console.ReadLine(), out droneID);
//                                        DO.Drone? currentDrone = null;
//                                        try { currentDrone = dal.GetDrone(droneID); }
//                                        catch (DO.findException find) { Console.WriteLine(find.Message); }
//                                        Console.WriteLine(currentDrone.ToString());
//                                    }
//                                    break;
//                                case CategoryChoice.Customer:

//                                    {
//                                        int CustomerID;
//                                        Console.WriteLine("enter the Customer's id");
//                                        int.TryParse(Console.ReadLine(), out CustomerID);
//                                        DO.Customer? currentCustomer = null;
//                                        currentCustomer = dal.GetCustomer(CustomerID);
//                                        try { currentCustomer = dal.GetCustomer(CustomerID); }
//                                        catch (DO.findException find) { Console.WriteLine(find.Message); }
//                                        Console.WriteLine(currentCustomer.ToString());
//                                    }
//                                    break;
//                                case CategoryChoice.PARCEL:
//                                    {
//                                        int parcelID;
//                                        Console.WriteLine("enter the parcel's id");
//                                        int.TryParse(Console.ReadLine(), out parcelID);
//                                        DO.Parcel? currentParcel = null;
//                                        try { currentParcel = dal.GetParcel(parcelID); }
//                                        catch (DO.findException find) { Console.WriteLine(find.Message); }
//                                        Console.WriteLine(currentParcel.ToString());
//                                    }
//                                    break;
//                                default:
//                                    break;
//                            }
//                        }
//                        break;
//                    case MainChoice.list:
//                        {
//                            Console.WriteLine($"what would you print to add?");
//                            Console.WriteLine($"enter 1 to list  station");
//                            Console.WriteLine($"enter 2 to list drones");
//                            Console.WriteLine($"enter 3 to list Customers");
//                            Console.WriteLine($"enter 4 to list parcel");
//                            Console.WriteLine($"enter 5 to o print the list of the parcels that arn't matched with a drone");
//                            Console.WriteLine("");
//                            bool isB;
//                            string str = Console.ReadLine();
//                            isB = int.TryParse(str, out int error1);
//                            int num1;
//                            printLists choice1;
//                            if (isB)
//                                num1 = int.Parse(str);
//                            else
//                                num1 = -1;
//                            choice1 = (printLists)num1;
//                            switch (choice1)
//                            {
//                                case printLists.STATION:
//                                    {
//                                        foreach (DO.Station st in dal.GetStationList()) { Console.WriteLine(st.ToString() + "\n"); }
//                                    }
//                                    break;
//                                case printLists.DRONE:
//                                    {
//                                        foreach (DO.Drone d in dal.GetDroneList()) { Console.WriteLine(d.ToString() + "\n"); }
//                                    }
//                                    break;
//                                case printLists.Customer:
//                                    {
//                                        foreach (DO.Customer c in dal.GetCustomerList()) { Console.WriteLine(c.ToString() + "\n"); }
//                                    }
//                                    break;
//                                case printLists.PARCEL:
//                                    {
//                                        foreach (DO.Parcel p in dal.GetParcelList()) { Console.WriteLine(p.ToString() + "\n"); }
//                                    }
//                                    break;
//                                case printLists.DISMATCHED_PACKAGES:
//                                    {
//                                        foreach (DO.Parcel p in dal.GetParcelList()) { if (p.droneId == 0) Console.WriteLine(p.id.ToString() + "\n"); }
//                                    }
//                                    break;
//                                case printLists.AVAILABLE_SLOTS:
//                                    {
//                                        foreach (DO.Station st in dal.GetStationList()) { if (st.chargeSlots > 0) Console.WriteLine(st.ToString() + "\n"); }
//                                    }
//                                    break;
//                                default:
//                                    break;
//                            }

//                        }
//                        break;
//                    case MainChoice.exit:
//                        break;
//                    default:
//                        break;

//                }
//            } while (choice != (MainChoice.exit));
//        }
//    }
//}