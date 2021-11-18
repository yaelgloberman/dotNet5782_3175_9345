using IDAL.DO;
using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace DalObject
    {
        public partial class DalObject
        {
            public bool checkParcel(int id)
            {
                return DataSource.parcels.Any(p => p.id == id);
            }
            public void addParcel(Parcel p)
            {
                if (!DataSource.Customers.Exists(item => item.id == p.id))
                    throw new AddException("drone already exist");
                DataSource.parcels.Add(p);
            }
            public void attribute(int dID, int pID)//the function attribute parcel to drone
            {
                Drone tmpD = GetDrone(dID);
                Parcel tmpP = GetParcel(pID);
                DataSource.parcels.RemoveAll(m => m.id == tmpP.id);   //removing all the data from the place in the list the equal to tmpP id
                tmpP.droneId = tmpD.id;        //attribute drones id to parcel 
                tmpP.scheduled = DateTime.Now; //changing the time to be right now
                DataSource.parcels.Add(tmpP); //adding to the parcel list tmpP
            }
            public void PickUpPackageByDrone(int dID, int pID)// the function picking up the parcel by the drone
            {
                GetDrone(dID);
                GetParcel(pID);
                for (int i = 0; i < DataSource.parcels.Count; i++)  //iterat that goes through all the parcel list
                {
                    if (DataSource.parcels[i].id == pID)// if the pId equal to the parcel list 
                    {
                        Parcel tmpP = DataSource.parcels[i];  //puting into tmpP the parcel that equal to pID
                        tmpP.pickedUp = DateTime.Now; //changing the time to be right now
                        DataSource.parcels[i] = tmpP;//puting into the parcel list the new tmpP
                    }
                }
                for (int i = 0; i < DataSource.drones.Count; i++) //iterat that goes throe all the drone list
                {
                    if (DataSource.drones[i].id == dID) //iterat that goes through all the parcel list
                    {
                        Drone tmpD = DataSource.drones[i];//puting into tmpD the drone that equal to dID
                        DataSource.drones[i] = tmpD; //puting into the drone list the new tmpD
                    }
                }
            }
            public void DeliveryPackageCustomer(int cID, int pId, IDAL.DO.Proirities proirity)//updating the drone when irt was called from the Customer
            {
                Parcel tmpP = GetParcel(pId);
                Customer tmpC = GetCustomer(cID);
                DataSource.parcels.RemoveAll(m => m.id == tmpP.id);//removing the parcel with the given id
                tmpP.priority = proirity;
                tmpP.targetId = tmpC.id;
                tmpP.delivered = DateTime.Now;
                DataSource.parcels.Add(tmpP);
            }
            public Parcel GetParcel(int id)//function that gets id and finding the parcel in the parcels list and returns parcel
            {
                Parcel? tmp = null;
                foreach (Parcel p in DataSource.parcels)
                {
                    if (p.id == id)
                    {
                        tmp = p;
                        break;
                    }
                }
                if (tmp == null)
                {
                    throw new UpdateException("parcel does not exist");
                }
                return (Parcel)tmp;
            }
            public void deleteParcel(Parcel p)
            {
                if (!DataSource.parcels.Exists(item => item.id == p.id))
                    throw new findException("parcel");
                DataSource.parcels.Remove(p);
            }
        }
    }
   

