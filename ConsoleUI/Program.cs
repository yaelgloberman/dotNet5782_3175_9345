using System;

namespace ConsoleUI
{
    public enum MainChoice { add = 1, update, display, list, exit };
    public enum addChoice { STATION = 1, DRONE, CUSTOMER, PARCEL };
    public enum catigoryChoice { STATION = 1, DRONE, CUSTOMER, PARCEL };
    class Program
    {
        static void Main(string[] args)
        {
            //IDAL.DO.BaseStation baseStasion = new IDAL.DO.BaseStation();
            int userA;
            Console.WriteLine("Menue: ");
            Console.WriteLine("press 1 to add an object");
            Console.WriteLine("press 2 to update");
            Console.WriteLine("press 3 for display");
            Console.WriteLine("press 4 to see lists ");
            Console.WriteLine("press 5 to exit");
            MainChoice choice;
            bool b;
            string s;
            do
            {
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
                            Console.WriteLine("what would you like to add?");
                            Console.WriteLine("enter 1 to add station");
                            Console.WriteLine("enter 2 to add drone");
                            Console.WriteLine("enter 3 to add customer");
                            Console.WriteLine("enter 4 to add parcel");
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
                                        DalObject.DalObject.addStation(userA, name1, longitude1, latitude1, chargeSlots1);

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
                                        DalObject.DalObject.addDrone(temp);
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
                                        //temp.set(userA)
                                        DalObject.DalObject.addCustomer(temp);

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
                                        temp.Priority = (IDAL.DO.Proirities)int.Parse(Console.ReadLine());//might have to check validity of 05 
                                        Console.WriteLine("enter the weight of the package");//not sure if i should do it with enum or have to do tkinut kelet
                                        temp.Weight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        //do i have to do all the time entering or is that in the print function??\
                                        //Console.WriteLine("enter the time the packge was requested");
                                        //Console.WriteLine("enter the time it was schedrules ");
                                        //Console.WriteLine("enter the drone id ");
                                        //Console.WriteLine("enter the drone id ");
                                        DalObject.DalObject.addParcel(temp);
                                    }



                                    break;
                                default://what is the default????
                                    break;
                            }
                        }

                        break;
                    case MainChoice.update:
                        {
                            //how is this supposed to look as the output
                            IDAL.DO.Parcel p = new IDAL.DO.Parcel();
                            Console.WriteLine("enter the id of the parcel you want to update");
                            Console.WriteLine();
                            DalObject.DalObject.updateDrone(p);
                        }
                        break;
                    case MainChoice.display:
                        {
                            Console.WriteLine("what would you like to display?");
                            Console.WriteLine("enter 1 to display station");
                            Console.WriteLine("enter 2 to display drone");
                            Console.WriteLine("enter 3 to display customer");
                            Console.WriteLine("enter 4 to display parcel");
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
                                        currentStation = DalObject.DalObject.findStation(stationID);
                                        if (currentStation.id == 0)
                                            Console.WriteLine("this station dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentStation.ToString());//im not sure how to print this?
                                        }
                                    }
                                    break;
                                case catigoryChoice.DRONE:
                                    {

                                        Console.WriteLine("enter the drone's id");
                                        int droneID = int.Parse(Console.ReadLine());
                                        IDAL.DO.Drone currentDrone = new IDAL.DO.Drone();
                                        currentDrone = DalObject.DalObject.findDrone(droneID);
                                        if (currentDrone.Id == 0)
                                            Console.WriteLine("this drone dosent exist");
                                        else
                                        {
                                            Console.WriteLine(currentDrone.ToString());
                                        }
                                    }
                                    break;
                                case catigoryChoice.CUSTOMER://literal copy paste from the others - waiting  to make sure that it debugs
                                    break;
                                case catigoryChoice.PARCEL:
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case MainChoice.list:
                        {
                            Console.WriteLine("what would you like to display?");
                            Console.WriteLine("enter 1 to display station");
                            Console.WriteLine("enter 2 to display drone");
                            Console.WriteLine("enter 3 to display customer");
                            Console.WriteLine("enter 4 to display parcel");
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

                                        
                                    }
                                    break;
                                case catigoryChoice.DRONE:
                                    break;
                                case catigoryChoice.CUSTOMER:
                                    break;
                                case catigoryChoice.PARCEL:
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
            } while (true);//wrong boolian phrase just trying to minimuze errors - HAVE TO COME BACK TO FIX!! otherwise there will be an infinate loop
        }
    }
}