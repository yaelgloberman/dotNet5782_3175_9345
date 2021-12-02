using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
using BL;

namespace ConsoleUI_BL
{
    public enum MainChoice { station=1,drone,customer,parcel}
    public enum objectChoice { add=1,update,retrieve,lists}
    public enum updateDrone { updateDroneName=1,sendToCharge,releasingDrone}
    public enum updateCustomer { updateCustomerName=1, deliveryParcelToCustomer }
    public enum updateParcel { attributeParcelToDrone=1, pickedUpParcelByDrone }
    class Program
    {
        static IBL.IBl bl= new BL.BL();
        static IDAL.DO.IDal dal = new DalObject.DalObject();
        static void main(string[] args)
        {

            bool b;
            string s;
            MainChoice choice;
            int num;
            Console.WriteLine("Menue: ");
            Console.WriteLine("press 1 to add an object");
            Console.WriteLine("press 2 to update");
            Console.WriteLine("press 3 for display");
            Console.WriteLine("press 4 to see lists ");
            Console.WriteLine("press 5 to exit");
            Console.WriteLine("enter a number between 1-5");
            s = Console.ReadLine();
            b = int.TryParse(s, out int error);
            if (b)
                num = int.Parse(s);
            else
                num = -1;
            choice = (MainChoice)num;
            switch (choice)
            {
                case MainChoice.station:
                    {
                        objectChoice choice1;
                        b = int.TryParse(s, out error);
                        if (b)
                            num = int.Parse(s);
                        else
                            num = -1;
                        choice1 = (objectChoice)num;
                        switch (choice1)
                        {
                            case objectChoice.add:
                                {
                                    IBL.BO.BaseStation temp = new IBL.BO.BaseStation();
                                    int id, chargeSlots;
                                    string name;
                                    double longitude, latitude;
                                    IBL.BO.Location stationLoc = new Location();
                                    Console.WriteLine("enter a station id");
                                    int.TryParse(Console.ReadLine(), out id);
                                    temp.id = id;
                                    Console.WriteLine("enter station name");
                                    name = Console.ReadLine();
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
                                    catch (AlreadyExistException add) { Console.WriteLine(add.Message); }
                                    break;
                                }
                                break;
                            case objectChoice.update:
                                {  

                                    Console.WriteLine("enter station id\n");
                                    Console.WriteLine("enter station name");
                                    Console.WriteLine("enter total amount of charging slots");
                                    int stationID, AmountOfChargingSlots;
                                    string stationName;
                                    string input = Console.ReadLine();
                                    int.TryParse(input, out stationID);
                                    stationName = Console.ReadLine();
                                    input = Console.ReadLine();
                                    int.TryParse(input, out AmountOfChargingSlots);
                                    try { bl.updateStation(stationID, AmountOfChargingSlots, stationName); }
                                    catch (validException exp)
                                    {
                                        Console.WriteLine(exp.Message);
                                    }                   
                                }
                                break;
                            case objectChoice.retrieve:
                                {
                                    int stationID;
                                    IBL.BO.BaseStation baseStation = new IBL.BO.BaseStation();
                                    Console.WriteLine("please enter the drone ID:");
                                    stationID = int.Parse(Console.ReadLine());
                                    try { baseStation = bl.GetStation(stationID); Console.WriteLine(baseStation.ToString()); }
                                    catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                }

                                break;
                            case objectChoice.lists://have to do the try and catch here of there are no objects in the lists
                                {
                                    foreach (var st in bl.GetStations()) { Console.WriteLine(st.ToString() + "\n"); }

                                }
                                    break;
                                        default:
                                    break;
                        }                                  
                    }

                    break;
                case MainChoice.drone:
                    {
                        objectChoice choice1;
                        b = int.TryParse(s, out error);
                        if (b)
                            num = int.Parse(s);
                        else
                            num = -1;
                        choice1 = (objectChoice)num;
                    switch (choice1)
                    {
                            case objectChoice.add:
                                {
                                    int droneId, stationId;
                                    string droneModel;
                                    double bateryStatus;
                                    IBL.BO.DroneToList temp = new IBL.BO.DroneToList();
                                    Console.WriteLine("enter a drone id");
                                    int.TryParse(Console.ReadLine(), out droneId);
                                    temp.id = droneId;
                                    Console.WriteLine("enter Model");
                                    droneModel = Console.ReadLine();
                                    temp.droneModel = droneModel;
                                    Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
                                    temp.weight = (IBL.BO.Weight)int.Parse(Console.ReadLine());
                                    Console.WriteLine("enter a station id");
                                    int.TryParse(Console.ReadLine(), out stationId);
                                    try { bl.addDrone(temp,stationId); }
                                    catch (AlreadyExistException exp) { Console.WriteLine(exp.Message); }
                                }
                                break;
                            case objectChoice.update:
                                {

                                    Console.WriteLine($"what would you like to update?");
                                    Console.WriteLine($"enter 1 to update drone name");
                                    Console.WriteLine($"enter 2 to send drone to charge");
                                    Console.WriteLine($"enter 3 to release drone from charge slots");

                                    bool isB;
                                    string str = Console.ReadLine();
                                    isB = int.TryParse(str, out int error1);
                                    int num1;
                                    updateDrone choiceDrone;
                                    if (isB)
                                        num1 = int.Parse(str);
                                    else
                                        num1 = -1;
                                    choiceDrone = (updateDrone)num1;
                                    switch (choiceDrone)
                                    {
                                        case updateDrone.updateDroneName:
                                            {
                                                Console.WriteLine("enter drine id\n");
                                                Console.WriteLine("enter new drone model\n");
                                                int droneId;
                                                string droneModel;
                                                string input = Console.ReadLine();
                                                int.TryParse(input, out droneId);
                                                input = Console.ReadLine();
                                                droneModel = Console.ReadLine();
                                                try
                                                {
                                                    bl.updateDroneName(droneId, droneModel);
                                                }
                                                catch (validException exp)
                                                {
                                                    Console.WriteLine(exp.Message);
                                                }
                                            }
                                            break;
                                        case updateDrone.sendToCharge:
                                            {
                                             
                                                int droneId;
                                                int stationId;
                                                Console.WriteLine("enter drone id\n");
                                                string input = Console.ReadLine();
                                                int.TryParse(input, out droneId);
                                                DroneToList d = bl.GetDrone(droneId);
                                                Console.WriteLine("enter station id\n");
                                                input = Console.ReadLine();
                                                int.TryParse(input, out stationId);
                                                try { bl.SendToCharge(droneId, stationId); }
                                                catch (dosntExisetException exp)
                                                {
                                                    Console.WriteLine(exp.Message);
                                                }
                                            }
                                            break;
                                        case updateDrone.releasingDrone:
                                            {
                                                int droneId;
                                                TimeSpan chargingTime;
                                                Console.WriteLine("enter drone id\n");
                                                string input = Console.ReadLine();
                                                int.TryParse(input, out droneId);
                                                input = Console.ReadLine();
                                                TimeSpan.TryParse(input, out chargingTime);
                                                try { bl.releasingDrone(droneId, chargingTime); }
                                                catch (dosntExisetException exp)
                                                {
                                                    Console.WriteLine(exp.Message);
                                                }
                                            }
                                            break;
                                        default:
                                            break;

                                    }
                                }
                                break;


                            //Console.WriteLine("please enter the drones id:");
                            //int id = int.Parse(Console.ReadLine());
                            //Console.WriteLine("please enter the new name of the drone model");
                            //int dModel = int.Parse(Console.ReadLine());
                            //bl.updateDroneName(id, dModel);
                            case objectChoice.retrieve:
                                {
                                    int DroneID;
                                    IBL.BO.DroneToList currentDrone = new IBL.BO.DroneToList();
                                    Console.WriteLine("please enter the drone ID:");
                                    DroneID = int.Parse(Console.ReadLine());
                                    try
                                    { 
                                        currentDrone = bl.GetDrone(DroneID);
                                        Console.WriteLine(currentDrone.ToString()); 
                                    }
                                    catch (IDAL.DO.findException find) { Console.WriteLine(find.Message); }
                                }
                                break;
                            case objectChoice.lists:
                                {
                                    foreach (var d in bl.GetDrones()) { Console.WriteLine(d.ToString() + "\n"); }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case MainChoice.customer:
                    {
                        objectChoice choice1;
                        bool isB;
                        isB = int.TryParse(s, out error);
                        if (b)
                            num = int.Parse(s);
                        else
                            num = -1;
                        choice1 = (objectChoice)num;
                        switch (choice1)
                        {
                            case objectChoice.add:
                                {
                                    IBL.BO.Customer temp = new IBL.BO.Customer();
                                    int id, phoneNumber;
                                    double longitude, latitude;
                                    string name;
                                    IBL.BO.Location customerLoc = new Location();
                                    Console.WriteLine("enter the Customers id");
                                    int.TryParse(Console.ReadLine(), out id);
                                    temp.id = id;
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
                                    try { bl.addCustomer(temp); }
                                    catch (AlreadyExistException exp) { Console.WriteLine(exp.Message); }
                                }
                                break;
                            case objectChoice.update:
                                {
                                    updateCustomer choiceCustomer;
                                    Console.WriteLine($"enter 1 to update customer ");
                                    Console.WriteLine("enter 2 to delivery parcel to customer ");
                                    s=Console.ReadLine();
                                    b = int.TryParse(s, out error);
                                    if (b)
                                        num = int.Parse(s);
                                    else
                                        num = -1;
                                    choiceCustomer = (updateCustomer)num;
                                    switch (choiceCustomer)
                                    {
                                        case updateCustomer.updateCustomerName:
                                            {
                                                int customerId, phoneNumber;
                                                string customerName;
                                                Console.WriteLine("enter customer id \n");
                                                Console.WriteLine("enter new name\n");
                                                Console.WriteLine("enter new phone number\n");
                                                string input = Console.ReadLine();
                                                int.TryParse(input, out customerId);
                                                customerName = Console.ReadLine();
                                                input = Console.ReadLine();
                                                int.TryParse(input, out phoneNumber);
                                                try
                                                {
                                                    bl.updateCustomer(customerId, customerName, phoneNumber);
                                                }
                                                catch(validException exp)
                                                {
                                                    Console.WriteLine(exp.Message);
                                                }
                                            }
                                            break;
                                        case updateCustomer.deliveryParcelToCustomer:
                                            {
                                                int droneId;
                                                Console.WriteLine("enter drone id\n");
                                                string input = Console.ReadLine();
                                                int.TryParse(input, out droneId);
                                                try { bl.deliveryParcelToCustomer(droneId); }
                                                catch (dosntExisetException exp)
                                                {
                                                    Console.WriteLine(exp.Message);
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;  
                            case objectChoice.retrieve:
                                {
                                    int customerID;
                                    IBL.BO.Customer currentCustomer = new IBL.BO.Customer();
                                    Console.WriteLine("please enter the customers ID:");
                                    customerID = int.Parse(Console.ReadLine());
                                    try 
                                    {
                                        currentCustomer = bl.GetCustomer(customerID);
                                        Console.WriteLine(currentCustomer.ToString()); 
                                    }
                                    catch (IDAL.DO.findException find) { Console.WriteLine(find.Message); }
                                }
                                break;
                            case objectChoice.lists:
                                {
                                    foreach (var c in bl.GetCustomers()) { Console.WriteLine(c.ToString() + "\n"); }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case MainChoice.parcel:
                    {
                        objectChoice choice1;
                        b = int.TryParse(s, out error);
                        if (b)
                            num = int.Parse(s);
                        else
                            num = -1;
                        choice1 = (objectChoice)num;
                        switch (choice1)
                        {
                            case objectChoice.add:
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
                                    DroneInParcel_Status.battery = bl.GetDrone(droneId).batteryStatus;
                                    DroneInParcel_Status.location = bl.GetDrone(droneId).location;
                                    temp.droneInParcel = DroneInParcel_Status;
                                    Console.WriteLine("enter the time the packge was requested");
                                    temp.requested = DateTime.Parse(Console.ReadLine());
                                    Console.WriteLine("enter the time it was schedrules ");
                                    temp.scheduled = DateTime.Parse(Console.ReadLine());
                                    Console.WriteLine("enter the time it was Picked up  ");
                                    temp.pickedUp = DateTime.Parse(Console.ReadLine());
                                    Console.WriteLine("enter the time it was date time ");
                                    temp.delivered = DateTime.Parse(Console.ReadLine());
                                    try { bl.addParcel(temp); }
                                    catch (AlreadyExistException exp) 
                                    { Console.WriteLine(exp.Message); }
                                }
                                break;
                            case objectChoice.update:
                                {
                                    Console.WriteLine("enter 6 to match parcel to drone");
                                    Console.WriteLine("enter 7 to picke up parcel by drone");
                                    s = Console.ReadLine();
                                    b = int.TryParse(s, out error);
                                    if (b)
                                        num = int.Parse(s);
                                    else
                                        num = -1;
                                    updateParcel choiceParcel;
                                      choiceParcel = (updateParcel) num; 
                                    switch (choiceParcel)
                                    {
                                        case updateParcel.attributeParcelToDrone:
                                            {
                                                int droneId;
                                                Console.WriteLine("enter drone id\n");
                                                string input = Console.ReadLine();
                                                int.TryParse(input, out droneId);
                                                try { bl.matchingDroneToParcel(droneId); }
                                                catch (unavailableException exp)
                                                {
                                                    Console.WriteLine(exp.Message);
                                                }
                                            }
                                            break;
                                        case updateParcel.pickedUpParcelByDrone:
                                            {
                                                int droneId;
                                                Console.WriteLine("enter drone id\n");
                                                string input = Console.ReadLine();
                                                int.TryParse(input, out droneId);
                                                try { bl.pickedUpParcelByDrone(droneId); }
                                                catch (unavailableException exp)   ////צריך לכתוב את הפונקציה הזראת
                                                {
                                                    Console.WriteLine(exp.Message);
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                }
            
                                break;
                            case objectChoice.retrieve:
                                {
                                    int parcelID;
                                    IBL.BO.Parcel currentParcel = new IBL.BO.Parcel();
                                    Console.WriteLine("please enter the Parcels ID:");
                                    parcelID = int.Parse(Console.ReadLine());
                                    try 
                                    {
                                        currentParcel = bl.GetParcel(parcelID);
                                        Console.WriteLine(currentParcel.ToString());
                                    }
                                    catch (IDAL.DO.findException find) { Console.WriteLine(find.Message); }
                                }
                                break;
                            case objectChoice.lists:
                                {
                                    foreach (var d in bl.GetParcels()) { Console.WriteLine(d.ToString() + "\n"); }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}



   