using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DalApi;
using DO;
namespace Dal
{
    sealed partial class DalObject : IDal
    {
        #region check Parcel
        public bool checkParcel(int id)
        {
            return DataSource.parcels.Any(p => p.id == id);
        }
        #endregion
        #region add parcel
        public int addParcel(Parcel p)
        {
            if (DataSource.parcels.Exists(item => item.id == p.id))
                throw new AddException("drone already exist");
            if(p.id==0)
            {
                p.id = DataSource.Config.parcelSerial++;

            }
            DataSource.parcels.Add(p);
            return p.id;
        }
        #endregion
        #region matching parcel to drone
        public void attribute(int dID, int pID)//the function attribute parcel to drone
        {
            Drone tmpD = GetDrone(dID);
            Parcel tmpP = GetParcel(pID);
            DataSource.parcels.RemoveAll(m => m.id == tmpP.id);  
            tmpP.droneId = tmpD.id;        
            tmpP.scheduled = DateTime.Now; 
            DataSource.parcels.Add(tmpP); 
        }
        #endregion
        #region Pick Up Package By Drone
        public void PickUpPackageByDrone(int dID, int pID)// the function picking up the parcel by the drone
        {
            GetDrone(dID);
            GetParcel(pID);
            for (int i = 0; i < DataSource.parcels.Count; i++) 
            {
                if (DataSource.parcels[i].id == pID)
                {
                    Parcel tmpP = DataSource.parcels[i];  
                    tmpP.pickedUp = DateTime.Now; 
                    DataSource.parcels[i] = tmpP;
                }
            }
            for (int i = 0; i < DataSource.drones.Count; i++) 
            {
                if (DataSource.drones[i].id == dID) 
                {
                    Drone tmpD = DataSource.drones[i];
                    DataSource.drones[i] = tmpD;
                }
            }
        }
        #endregion
        #region Delivery Package Customer
        public void DeliveryPackageCustomer(int cID, int pId, DO.Proirities proirity)
        {
            Parcel tmpP = GetParcel(pId);
            Customer tmpC = GetCustomer(cID);
            DataSource.parcels.RemoveAll(m => m.id == tmpP.id);
            tmpP.priority = proirity;
            tmpP.targetId = tmpC.id;
            tmpP.delivered = DateTime.Now;
            DataSource.parcels.Add(tmpP);
        }
        #endregion
        #region IEnumerable get parcels
        public IEnumerable<Parcel> GetParcels()
        {
            return DataSource.parcels;
        }
        #endregion
        #region get parcel
        public Parcel GetParcel(int id)
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
                throw new findException("parcel does not exist");
            }
            return (Parcel)tmp;
        }
        #endregion
        #region delete parcel
        public void deleteParcel(Parcel p)
        {
            if (!DataSource.parcels.Exists(item => item.id == p.id))
                throw new findException("parcel");
            DataSource.parcels.Remove(p);
        }
        #endregion
        #region undelivered parsels
        public List<Parcel> UndiliveredParcels()
        {
            List<Parcel> unDeliveredP = new List<Parcel>();
            DateTime dateTime_Help = new DateTime(0, 0, 0);
            foreach (Parcel p in DataSource.parcels)
            {
                if (p.delivered == dateTime_Help && p.droneId > 0)
                    unDeliveredP.Add(p);
            }
            return unDeliveredP;
        }
        #endregion
        #region Get Parcels
        public IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null)
        => predicate == null ? DataSource.parcels : DataSource.parcels.Where(predicate);
        #endregion
    }
}
   

