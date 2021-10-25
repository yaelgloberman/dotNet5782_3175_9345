using System;

namespace ConsoleUI
{
    public enum MainChoice { add = 1, update, models, list, exit };
    class Program
    {
        static void Main(string[] args)
        {
            //IDAL.DO.BaseStation baseStasion = new IDAL.DO.BaseStation();
            int userA;
            Console.WriteLine("Menue: ");
            Console.WriteLine("press 1 to add a drone");
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
                            IDAL.DO.Station temp = new IDAL.DO.Station();
                            Console.WriteLine("enter a station id");
                            userA = int.Parse(Console.ReadLine());
                            temp.id = userA;
                            Console.WriteLine("enter name");
                            int name1 =int.Parse(Console.ReadLine());
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
            }
        }
    }
}