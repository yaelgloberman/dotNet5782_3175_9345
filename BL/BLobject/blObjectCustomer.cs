using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
using IBL;
using System.Runtime.Serialization;
namespace BL
{
    public partial class BL : IBl
    {
        public List<ParcelCustomer> CustomerReceiveParcel(int customerID)
        {
            List<IBL.BO.ParcelCustomer> recievedParcels = new List<ParcelCustomer>();
            foreach (var parcel in GetParcels())
            {
                if (parcel.receive.id == customerID)
                {
                    IBL.BO.ParcelCustomer customersPrevioseParcel = new ParcelCustomer { CustomerInParcel = parcel.receive, id = parcel.id, parcelStatus = ParcelStatus.Delivered, priority = parcel.priority, weight = parcel.weightCategorie };
                    recievedParcels.Add(customersPrevioseParcel);
                }
            }
            return recievedParcels;

        }
        public void updateCustomer(int customerID, string Name = " ", int phoneNum = 0)
        {
            try
            {
                IDAL.DO.Customer customerDl = new IDAL.DO.Customer();
                customerDl = dal.GetCustomer(customerID);
                customerDl.name = Name;
                customerDl.phoneNumber = phoneNum;
                dal.updateCustomer(customerID, customerDl);
            }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
        }
        public IBL.BO.Customer GetCustomer(int id)
        {
            try
            {
                List<IBL.BO.Parcel> tempParcels = GetParcels();
                IBL.BO.Customer CustomerBo = new IBL.BO.Customer();
                IDAL.DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.Name = CustomerDo.name;
                CustomerBo.phoneNumber = CustomerDo.phoneNumber;
                CustomerBo.location = new Location() { latitude = CustomerDo.latitude, longitude = CustomerDo.longitude };
                CustomerBo.SentParcels = getCustomerShippedParcels(id).ToList();
                CustomerBo.ReceiveParcel = CustomerReceiveParcel(id).ToList();
                return CustomerBo;
            }
            catch (IDAL.DO.findException Fex)
            {
                throw new validException(Fex.Message);
            }
        }
        #region Get Customer
        public IBL.BO.CustomerInList GetCustomerToList(int id)
        {
            try
            {
                List<IBL.BO.Parcel> tempParcels = GetParcels();
                IBL.BO.CustomerInList CustomerBo = new();
                IDAL.DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.Name = CustomerDo.name;
                CustomerBo.PhoneNumber = CustomerDo.phoneNumber;
                CustomerBo.Parcles_Delivered_Recieved = GetParcels().Count(x => x.delivered != DateTime.MinValue && x.pickedUp == DateTime.MinValue);
                CustomerBo.Parcels_unrecieved = GetParcels().Count(x => x.pickedUp == DateTime.MinValue);   //נראלי שזה שלא קבלו אומר שהם לא אספו
                CustomerBo.Recieved_Parcels = CustomerReceiveParcel(id).Count;////לא בטוחה צריך לבדוק את זה
                CustomerBo.ParcelsInDeliver = GetParcels().Count(x => x.delivered != DateTime.MinValue);
                return CustomerBo;
            }
            catch (IDAL.DO.findException Fex)
            {
                throw new validException($"Customer id {id}", Fex);
            }
        }
        #endregion
        public List<IBL.BO.Customer> GetCustomers()
        {
            List<IBL.BO.Customer> customers = new List<IBL.BO.Customer>();
            foreach (var c in dal.CustomerList())
            { customers.Add(GetCustomer(c.id)); }
            return customers;
        }
        public List<IBL.BO.CustomerInList> GetCustomersToList()
        {
            List<IBL.BO.CustomerInList> customers = new();
            foreach (var c in dal.CustomerList())
            { customers.Add(GetCustomerToList(c.id)); }
            return customers;
        }
        #region Add Customer
        public void addCustomer(IBL.BO.Customer CustomerToAdd)
        {

            if (!(CustomerToAdd.id >= 10000000 && CustomerToAdd.id <= 1000000000))
                throw new validException("the id number of the drone is invalid\n");
            if (!(CustomerToAdd.phoneNumber >= 500000000 && CustomerToAdd.phoneNumber <= 0589999999))
                throw new validException("the phone number of the Customer is invalid\n");
            if (CustomerToAdd.location.latitude < (double)31 || CustomerToAdd.location.latitude > 33.3)
                throw new validException("the given latitude do not exist in this country/\n");
            if (CustomerToAdd.location.longitude < 34.3 || CustomerToAdd.location.longitude > 35.5)
                throw new validException("the given longitude do not exist in this country/\n");
            if (dal.GetCustomers().ToList().Exists(item => item.id == CustomerToAdd.id))
                throw new AlreadyExistException("Customer already exist");
            IDAL.DO.Customer CustomerDo = new IDAL.DO.Customer();
            CustomerDo.id = CustomerToAdd.id;
            CustomerDo.name = CustomerToAdd.Name;
            CustomerDo.phoneNumber = CustomerToAdd.phoneNumber;
            CustomerDo.longitude = CustomerToAdd.location.longitude;
            CustomerDo.latitude = CustomerToAdd.location.latitude;
            try
            {
                dal.addCustomer(CustomerDo);
            }
            catch (AddException exp)
            {

                throw new AlreadyExistException("the customer already exist", exp);
            }
            // Nה צריך לבדוק עם SentParcels ReceiveParcel
        }
        #endregion


    }
}
