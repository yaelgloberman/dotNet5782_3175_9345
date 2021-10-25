using System;

namespace ConsoleUI
{
    public enum MainChoice { add = 1, update, models, list, exit };
    public enum addChoice {STATION=1, DRONE,CUSTOMER,PARCEL};
    class Program
    {
        static void Main(string[] args)
        {
            //IDAL.DO.BaseStation baseStasion = new IDAL.DO.BaseStation();
            int userA;
            Console.WriteLine("Menue: ");
            Console.WriteLine("press 1 to add an object");
            Console.WriteLine("press 2 to update");
            Console.WriteLine("press 3 to display");
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
                            addChoice choice1;
                            if (isB)
                                num1 = int.Parse(str);
                            else
                                num1 = -1;
                            choice1 = (addChoice)num1;
                            switch (choice1)
                            {
                                case addChoice.STATION:
                                    {
                                        IDAL.DO.Station temp = new IDAL.DO.Station();
                                        Console.WriteLine("enter a station id");
                                        userA = int.Parse(Console.ReadLine());
                                        temp.id = userA;
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
                                case addChoice.DRONE:
                                    {
                                        IDAL.DO.Drone temp = new IDAL.DO.Drone();
                                        Console.WriteLine("enter a drone  id");
                                        int tmpId= int.Parse(Console.ReadLine());
                                        temp.Id = tmpId;
                                        Console.WriteLine("enter Model");
                                        string tmpModel = Console.ReadLine();
                                        Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
                                        IDAL.DO.WeightCatigories weight = (IDAL.DO.WeightCatigories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the batery status");
                                        double tmpBateryStatus = double.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the current drone status");//have to go over to see if i did it right
                                        IDAL.DO.DroneStatuses tmpDroneStatuses = (IDAL.DO.DroneStatuses)int.Parse(Console.ReadLine());
                                        //temp.set(userA)
                                        DalObject.DalObject.addStation(userA, name1, longitude1, latitude1, chargeSlots1);
                                                public struct Drone
        {
           

        }
                                    break;
                                case addChoice.CUSTOMER:
                                    break;
                                case addChoice.PARCEL:
                                    break;
                                default:
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
                    case MainChoice.models:
                        break;
                    case MainChoice.list:
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