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
    public enum objectChoice { add=1,update,retrieve,lists,exit}
    public enum updateDrone { updateDroneName=1,sendToCharge,releasingDrone}
    public enum updateCustomer { updateCustomerName=1, deliveryParcelToCustomer }
    public enum updateParcel { attributeParcelToDrone=1, pickedUpParcelByDrone }
    class Program
    {
        #region check string calid
        public static void ValidateString(string string1)
        {
            List<string> invalidChars = new List<string>() { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-" };
            // Check for length
            if (string1.Length > 100)
            {
                throw new validException("String too Long");
            }
            else if (!(!string1.Equals(string1.ToLower())))
            {
                //Check for min 1 uppercase
                throw new validException("Requres at least one uppercase");
            }
            else
            {
                //Iterate your list of invalids and check if input has one
                foreach (string s in invalidChars)
                {
                    if (string1.Contains(s))
                    {
                        throw new validException("String contains invalid character: " + s);
                    }
                }
            }
        }
        #endregion
        public static IBL.IBl bl;
        public static void Main(string[] args)
        {
            bl = new BL.BL();
            bool b;
            string s;
            MainChoice choice;
            int num;
            Console.WriteLine("Menue: ");
            Console.WriteLine("press 1 to station menue");
            Console.WriteLine("press 2 to drone menue");
            Console.WriteLine("press 3 to customer menue");
            Console.WriteLine("press 4 to parcel menue");
            Console.WriteLine("enter a number between 1-4");
            s = Console.ReadLine();
            b = int.TryParse(s, out int error);
            if (b)
                num = int.Parse(s);
            else
                num = -1;
            choice = (MainChoice)num;
            while(num !=5)
            {
                switch (choice)
                {
                    case MainChoice.station:
                        {
                            Console.WriteLine("Menue: ");
                            Console.WriteLine("press 1 to add");
                            Console.WriteLine("press 2 to update");
                            Console.WriteLine("press 3 to retrieve");
                            Console.WriteLine("press 4 to list");
                            Console.WriteLine("press 5 to exit");
                            Console.WriteLine("enter a number between 1-5");
                            objectChoice choice1;
                            s = Console.ReadLine();
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
                                        try { ValidateString(name); }
                                        catch (validException exp){Console.WriteLine(exp.Message); };
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
                                        catch (validException valid) { Console.WriteLine(valid.Message); }
                                        break;
                                    }
                                case objectChoice.update:
                                    { 
                                        Console.WriteLine("enter station id");
                                        Console.WriteLine("enter station name");
                                        Console.WriteLine("enter total amount of charging slots");
                                        int stationID, AmountOfChargingSlots;
                                        string stationName;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out stationID);
                                        stationName = Console.ReadLine();
                                        try{ ValidateString(stationName); }
                                        catch (validException exp){Console.WriteLine(exp.Message); };
                                        input = Console.ReadLine();
                                        int.TryParse(input, out AmountOfChargingSlots);
                                        try { bl.updateStation(stationID, AmountOfChargingSlots, stationName); }
                                        catch (validException exp) {Console.WriteLine(exp.Message);}
                                    }
                                    break;
                                case objectChoice.retrieve:
                                    {
                                        int stationId;
                                        IBL.BO.BaseStation baseStation = new BaseStation();
                                        Console.WriteLine("please enter the drone ID:");
                                        stationId = int.Parse(Console.ReadLine());
                                        try { baseStation = bl.GetStation(stationId); Console.WriteLine(baseStation.ToString()); }
                                        catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                    }
                                    break;
                                case objectChoice.lists:
                                    {
                                        foreach (var st in bl.GetBaseStationToLists()) { Console.WriteLine(st.ToString()+"\n"); }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    case MainChoice.drone:
                        {
                            Console.WriteLine("Menue: ");
                            Console.WriteLine("press 1 to add");
                            Console.WriteLine("press 2 to update");
                            Console.WriteLine("press 3 to retrieve");
                            Console.WriteLine("press 4 to list");
                            Console.WriteLine("press 5 to exit");
                            Console.WriteLine("enter a number between 1-5");
                            objectChoice choice1;
                            s = Console.ReadLine();
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
                                        Weight weight;
                                        Console.WriteLine("enter a drone id");
                                        int.TryParse(Console.ReadLine(), out droneId);
                                        Console.WriteLine("enter Model");
                                        droneModel = Console.ReadLine();
                                        try { ValidateString(droneModel); }
                                        catch (validException exp) { Console.WriteLine(exp.Message); };
                                        Console.WriteLine("enter  a number from 1-3 describing its max weight, 3 is the heaviest");//go over the phraising
                                        weight = (IBL.BO.Weight)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter a station id");
                                        int.TryParse(Console.ReadLine(), out stationId);
                                        try { bl.addDrone(droneId, stationId, droneModel, weight); }
                                        catch (validException exp) { Console.WriteLine(exp.Message); }
                                        catch (AlreadyExistException exp) { Console.WriteLine(exp.Message); }
                                        catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
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
                                        //try { ValidateString(str); }
                                        //catch (validException exp) { Console.WriteLine(exp.Message); };
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
                                                    int droneId;
                                                    Console.WriteLine("enter drone id\n");
                                                    int.TryParse(Console.ReadLine(), out droneId);
                                                    Console.WriteLine("enter new drone model\n");
                                                    string droneModel = Console.ReadLine();
                                                    try { bl.updateDroneName(droneId, droneModel); }
                                                    catch (dosntExisetException exp) { Console.WriteLine(exp.Message); };
                                                }
                                                break;
                                            case updateDrone.sendToCharge:
                                                {

                                                    int droneId;
                                                    Console.WriteLine("enter drone id\n");
                                                    int.TryParse(Console.ReadLine(), out droneId);
                                                    try
                                                    {
                                                        DroneToList d = bl.GetDrone(droneId);
                                                        bl.SendToCharge(droneId);
                                                    }
                                                    catch (deleteException exp) { Console.WriteLine(exp.Message); }
                                                    catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                                    catch (unavailableException exp) { Console.WriteLine(exp.Message); }
                                                }
                                                break;
                                            case updateDrone.releasingDrone:
                                                {
                                                    int droneId;
                                                    TimeSpan chargingTime;
                                                    Console.WriteLine("enter drone id\n");
                                                    string input = Console.ReadLine();
                                                    int.TryParse(input, out droneId);
                                                    TimeSpan.TryParse(input, out chargingTime);
                                                    try { bl.releasingDrone(droneId, chargingTime); }
                                                    catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                                }
                                                break;
                                            default:
                                                break;

                                        }
                                    }
                                    break;
                                case objectChoice.retrieve:
                                    {
                                        int DroneID;
                                        IBL.BO.Drone currentDrone = new ();
                                        Console.WriteLine("please enter the drone ID:");
                                        DroneID = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            currentDrone = bl.returnsDrone(DroneID);
                                            Console.WriteLine(currentDrone.ToString());
                                        }
                                        catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
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
                            Console.WriteLine("Menue: ");
                            Console.WriteLine("press 1 to add");
                            Console.WriteLine("press 2 to update");
                            Console.WriteLine("press 3 to retrieve");
                            Console.WriteLine("press 4 to list");
                            Console.WriteLine("press 5 to exit");
                            Console.WriteLine("enter a number between 1-5");
                            objectChoice choice1;
                            s = Console.ReadLine(); bool isB;
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
                                        name=Console.ReadLine();
                                        try { ValidateString(name); }
                                        catch (validException exp) {Console.WriteLine(exp.Message); };
                                        temp.Name = name;
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
                                        catch (validException exp) { Console.WriteLine(exp.Message); }
                                        
                                    }
                                    break;
                                case objectChoice.update:
                                    {
                                        updateCustomer choiceCustomer;
                                        Console.WriteLine($"enter 1 to update customer ");
                                        Console.WriteLine("enter 2 to delivery parcel to customer ");
                                        s = Console.ReadLine();
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
                                                    Console.WriteLine("enter customer id");
                                                    Console.WriteLine("enter new name");
                                                    Console.WriteLine("enter new phone number");
                                                    string input = Console.ReadLine();
                                                    int.TryParse(input, out customerId);
                                                    customerName = Console.ReadLine();
                                                    input = Console.ReadLine();
                                                    int.TryParse(input, out phoneNumber);
                                                    try{ bl.updateCustomer(customerId, customerName, phoneNumber);}
                                                    catch (validException exp){Console.WriteLine(exp.Message);}
                                                    catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                                }
                                                break;
                                            case updateCustomer.deliveryParcelToCustomer:
                                                {
                                                    int droneId;
                                                    Console.WriteLine("enter drone id\n");
                                                    string input = Console.ReadLine();
                                                    int.TryParse(input, out droneId);
                                                    try { bl.deliveryParcelToCustomer(droneId); }
                                                    catch (dosntExisetException exp){ Console.WriteLine(exp.Message);}
                                                    catch (ExecutionTheDroneIsntAvilablle exp) { Console.WriteLine(exp.Message); }
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
                                        IBL.BO.Customer currentCustomer = new();
                                        Console.WriteLine("please enter the customers ID:");
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out customerID);
                                        try
                                        {
                                            currentCustomer = bl.GetCustomer(customerID);
                                            Console.WriteLine(currentCustomer.ToString());
                                        }
                                        catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                    }
                                    break;
                                case objectChoice.lists:
                                    {
                                        try { foreach (var c in bl.GetCustomersToList()) { Console.WriteLine(c.ToString() + "\n"); } }
                                        catch (dosntExisetException ex) { Console.WriteLine(ex.Message); }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case MainChoice.parcel:
                        {
                            Console.WriteLine("Menue: ");
                            Console.WriteLine("press 1 to add");
                            Console.WriteLine("press 2 to update");
                            Console.WriteLine("press 3 to retrieve");
                            Console.WriteLine("press 4 to list");
                            Console.WriteLine("press 5 to exit");
                            Console.WriteLine("enter a number between 1-5");
                            objectChoice choice1;
                            s = Console.ReadLine(); b = int.TryParse(s, out error);
                            if (b)
                                num = int.Parse(s);
                            else
                                num = -1;
                            choice1 = (objectChoice)num;
                            switch (choice1)
                            {
                                case objectChoice.add:
                                    {
                                        int senderId, receiveId;
                                        IBL.BO.Parcel temp = new IBL.BO.Parcel();
                                        IBL.BO.CustomerInParcel cipS = new CustomerInParcel();
                                        IBL.BO.CustomerInParcel cipR = new CustomerInParcel();
                                        IBL.BO.DroneInParcel dipS = new DroneInParcel();
                                        Console.WriteLine("enter the senders id");
                                        int.TryParse(Console.ReadLine(), out senderId);
                                        if (senderId >= 10000000 && senderId <= 1000000000) 
                                            throw new validException("the id sender number of the pardel is invalid\n");
                                        cipS.id = senderId;
                                        cipS.name = bl.GetCustomer(senderId).Name;
                                        Console.WriteLine("enter the reciever id");
                                        int.TryParse(Console.ReadLine(), out receiveId);
                                        if (receiveId >= 10000000 && receiveId <= 1000000000)
                                        throw new validException("the id sender number of the pardel is invalid\n");
                                        cipR.id = receiveId;
                                        cipR.name = bl.GetCustomer(receiveId).Name;
                                        temp.sender = cipS;
                                        temp.receive = cipR;
                                        Console.WriteLine("enetr its urgency: press 1 for regular press 2 for fast and press 3 for emergency");
                                        temp.priority = (IBL.BO.Priority)int.Parse(Console.ReadLine());
                                        Console.WriteLine("enter the weight of the package");//not sure if i should do it with enum or have to do tkinut kelet
                                        temp.weightCategorie = (IBL.BO.Weight)int.Parse(Console.ReadLine());
                                        try {Console.WriteLine("the id of the parcel is:"+bl.addParcel(temp));  }
                                        catch (AlreadyExistException exp) { Console.WriteLine(exp.Message); }
                                        catch (validException exp) { Console.WriteLine(exp.Message); }
                                    }
                                    break;
                                case objectChoice.update:
                                    {
                                        Console.WriteLine("enter 1 to match parcel to drone");
                                        Console.WriteLine("enter 2 to picke up parcel by drone");
                                        s = Console.ReadLine();
                                        b = int.TryParse(s, out error);
                                        if (b)
                                            num = int.Parse(s);
                                        else
                                            num = -1;
                                        updateParcel choiceParcel;
                                        choiceParcel = (updateParcel)num;
                                        switch (choiceParcel)
                                        {
                                            case updateParcel.attributeParcelToDrone:
                                                {
                                                    int droneId;
                                                    Console.WriteLine("enter drone id");
                                                    string input = Console.ReadLine();
                                                    int.TryParse(input, out droneId);
                                                    try { bl.matchingDroneToParcel(droneId); }
                                                    catch (unavailableException exp){Console.WriteLine(exp.Message); }
                                                    catch (dosntExisetException exp) { Console.WriteLine( exp.Message  ); }
                                                }
                                                break;
                                            case updateParcel.pickedUpParcelByDrone:
                                                {
                                                    int droneId;
                                                    Console.WriteLine("enter drone id");
                                                    string input = Console.ReadLine();
                                                    int.TryParse(input, out droneId);
                                                    try { bl.pickedUpParcelByDrone(droneId); }
                                                    catch (unavailableException exp) {Console.WriteLine(exp.Message);}
                                                    catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                                    catch (validException exp) { Console.WriteLine(exp.Message); }
                                                    
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
                                        string input = Console.ReadLine(); 
                                        int.TryParse(input, out parcelID);
                                        try
                                        {
                                            currentParcel = bl.GetParcel(parcelID);
                                            Console.WriteLine(currentParcel.ToString());
                                        }
                                        catch (dosntExisetException exp) { Console.WriteLine(exp.Message); }
                                    }
                                    break;
                                case objectChoice.lists:
                                    {
                                        foreach (var d in bl.GetParcelToLists()) { Console.WriteLine(d.ToString() + "\n"); }
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
                Console.WriteLine("Menue: ");
                Console.WriteLine("press 1 to station menue");
                Console.WriteLine("press 2 to drone menue");
                Console.WriteLine("press 3 to customer menue");
                Console.WriteLine("press 4 to parcel menue");
                Console.WriteLine("enter a number between 1-4");
                s = Console.ReadLine();
                b = int.TryParse(s, out error);
                if (b)
                    num = int.Parse(s);
                else
                    num = -1;
                choice = (MainChoice)num;
            }
        }
    }
}
