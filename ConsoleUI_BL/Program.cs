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
                    break;
                case MainChoice.drone:
                    break;
                case MainChoice.customer:
                    {
                        IBL.BO.Customer temp=new IBL.BO.Customer();
                        int id, phoneNumber;
                        double longitude, latitude;
                        string name;
                        //IDAL.DO.Customer temp = new IDAL.DO.Customer();
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
                        temp.Location.longitude = longitude;
                        Console.WriteLine("enter latitude");
                        double.TryParse(Console.ReadLine(), out latitude);
                        temp.location.latitude = latitude;
                        try { bl.addDrone(temp); }
                        catch (IBL.BO.AddException add) { Console.WriteLine(add.Message); }
                    }
                    break;
                case MainChoice.parcel:
                    break;
                default:
                    break;
            }
        }
    }
}
