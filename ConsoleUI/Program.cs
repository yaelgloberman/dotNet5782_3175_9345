using System;

namespace ConsoleUI
{
    public enum MainChoice { add = 1, update, display, list, exit };
    public enum catigoryChoice { STATION = 1, DRONE, CUSTOMER, PARCEL };
    public enum update { attribute = 1, PickUpPackageByDrone, DeliveryPackageCustomer, SendToCharge , printAvailableChrgingStations };
  
    class Program
    {
       static  DalObject.DalObject Data;
        static void Main(string[] args)
        {
            Data = new DalObject.DalObject();
            //IDAL.DO.BaseStation baseStasion = new IDAL.DO.BaseStation();
            int userA;

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
                            catigoryChoice choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (catigoryChoice)num1;
                            switch (choice1) //add
                            {
                                case catigoryChoice.STATION:
                                    {
                                        // IDAL.DO.Station temp = new IDAL.DO.Station();
                                        Console.WriteLine("enter a station id");
                                        userA = int.Parse(Console.ReadLine());
                                        //.id = userA;
                                        Console.WriteLine("enter name");
                                        int name1 = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter longitude");
                                        double longitude1 = double.Parse(Console.ReadLine());
                                        Console.WriteLine("enter latitude");
                                        double latitude1 = double.Parse(Console.ReadLine());
                                        Console.WriteLine("enter charge slots");
                                        int chargeSlots1 = int.Parse(Console.ReadLine());
                                        //temp.set(userA)
                                        Data.addStation(userA, name1, longitude1, latitude1, chargeSlots1);

                                    }
                                    break;
                                case catigoryChoice.DRONE:
                                    {
                                        IDAL.DO.Drone temp = new IDAL.DO.Drone();
                                        Console.WriteLine("enter a drone  id");
                                        temp.Id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter Model");
                                        temp.Model = Console.ReadLine();
                                        Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
                                        temp.MaxWeight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the batery status");
                                        temp.BateryStatus = double.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the current drone status");//have to go over to see if i did it right
                                        temp.Status = (IDAL.DO.DroneStatuses)int.Parse(Console.ReadLine());
                                        Data.addDrone(temp);
                                    }
                                    break;
                                case catigoryChoice.CUSTOMER:
                                    {
                                        IDAL.DO.customer temp = new IDAL.DO.customer();
                                        Console.WriteLine("enter the customers id");
                                        temp.Id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the customers name");
                                        temp.Name = Console.ReadLine();
                                        Console.WriteLine("enter the customers phonenumber");
                                        temp.PhoneNumber = int.Parse(Console.ReadLine());//might have to check validity of 05 
                                        Console.WriteLine("enter latitude");
                                        temp.Latitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the customers longitude");
                                        temp.Longitude = int.Parse(Console.ReadLine());
                                        Data.addCustomer(temp);

                                    }

                                    break;
                                    case catigoryChoice.PARCEL:
                                    {
                                        IDAL.DO.Parcel temp = new IDAL.DO.Parcel();
                                        Console.WriteLine("enter thye senders id");
                                        temp.Id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the target id");
                                        temp.TargetId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enetr its urgency: press 1 for regular press 2 for fast and press 3 for emergency");
                                        while (int.Parse(Console.ReadLine()) > 3)
                                        {
                                            Console.WriteLine("ERROR re-enter");
                                        }
                                        temp.Priority = (IDAL.DO.Proirities)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the weight of the package");//not sure if i should do it with enum or have to do tkinut kelet
                                        temp.Weight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the drone id ");
                                        temp.DroneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time the packge was requested");
                                        temp.Requested = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was schedrules ");
                                        temp.Scheduled = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was Picked up  ");
                                        temp.PickedUp = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the time it was date time ");
                                        temp.Datetime = DateTime.Parse(Console.ReadLine());
                                        Data.addParcel(temp);
                                    }

      
                            }
                        }
                        break;
                    case MainChoice.update:
                        {

                            Data.MenuPrint("update");
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
                                        Console.WriteLine("Enter the dronws ID:\n");
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
                                        Console.WriteLine("Enter the dronws ID:\n");
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
                                        DalObject.DalObject.DeliveryPackageCustomer();
                                    }
                                    break;
                                case update.SendToCharge:
                                    {
                                        Console.WriteLine("enter the drone id,station id");
                                        int idDrone = int.Parse(Console.ReadLine());
                                        int idStation = int.Parse(Console.ReadLine());
                                        Data.SendToCharge(idDrone, idStation);
                                    }
                                    break;
                                case update.printAvailableChrgingStations:
                                    {
                                        Data.printAvailableChrgingStations();
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }

                    case MainChoice.display:
                        {
                            Data.MenuPrint("870ד" +
                                "[==");
                            bool isB;
                            string str = Console.ReadLine();
                            //בדיקת תקינות קלט-have to go over that 
                            isB = int.TryParse(str, out int error1);
                            int num1;
                            catigoryChoice choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (catigoryChoice)num1;
                            switch (choice1)
                            {
                                case catigoryChoice.STATION:
                                    {

                                        Console.WriteLine("enter the station's id");
                                        int stationID = int.Parse(Console.ReadLine());
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
                                case catigoryChoice.DRONE:
                                    {

                                        Console.WriteLine("enter the drone's id");
                                        int droneID = int.Parse(Console.ReadLine());
                                        IDAL.DO.Drone currentDrone = new IDAL.DO.Drone();
                                        currentDrone = Data.findDrone(droneID);
                                        if (currentDrone.Id == 0)
                                            Console.WriteLine("this drone dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentDrone.ToString());
                                        }
                                    }
                                    break;
                                case catigoryChoice.CUSTOMER:

                                    {

                                        Console.WriteLine("enter the customer's id");
                                        int customerID = int.Parse(Console.ReadLine());
                                        IDAL.DO.customer currentCustomer = new IDAL.DO.customer();
                                        currentCustomer = Data.findCustomer(customerID);
                                        if (currentCustomer.Id == 0)
                                            Console.WriteLine("this customer dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentCustomer.ToString());
                                        }
                                    }
                                    break;
                                case catigoryChoice.PARCEL:
                                    {

                                        Console.WriteLine("enter the parcel's id");
                                        int parcelID = int.Parse(Console.ReadLine());
                                        IDAL.DO.Parcel currentParcel = new IDAL.DO.Parcel();
                                        currentParcel = Data.findParcel(parcelID);
                                        if (currentParcel.Id == 0)
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
                            Data.MenuPrint("print all of");
                            bool isB;
                            string str = Console.ReadLine();
                            //בדיקת תקינות קלט-have to go over that 
                            isB = int.TryParse(str, out int error1);
                            int num1;
                            catigoryChoice choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (catigoryChoice)num1;
                            switch (choice1)
                            {
                                case catigoryChoice.STATION:
                                    {
                                        Data.printStationList();
                                    }
                                    break;
                                case catigoryChoice.DRONE:
                                    {
                                        Data.printDroneList();
                                    }
                                    break;
                                case catigoryChoice.CUSTOMER:
                                    {
                                        Data.printCustomerList();
                                    }
                                    break;
                                case catigoryChoice.PARCEL:
                                    {
                                        Data.printParcelList();
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