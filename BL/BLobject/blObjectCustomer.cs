using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using BO;
using DalApi;
using BlApi;
using System.Runtime.Serialization;
namespace BL
{
    public partial class BL : IBl
    {
        /// <summary>
        ///  recieving a customers id and name that was a sender or a recievr of a parcel form the  the datasource-(really from the parcel thats in the datasource) by recieving the id of the customer  and throwing an exception if the id was in correct
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public List<ParcelCustomer> CustomerReceiveParcel(int customerID)
        {
            List<BO.ParcelCustomer> recievedParcels = new List<ParcelCustomer>();
            foreach (var parcel in GetParcels())
            {
                if (parcel.receive.id == customerID)
                {
                    BO.ParcelCustomer customersPrevioseParcel = new ParcelCustomer { CustomerInParcel = parcel.receive, id = parcel.id, parcelStatus = ParcelStatus.Delivered, priority = parcel.priority, weight = parcel.weightCategorie };
                    recievedParcels.Add(customersPrevioseParcel);
                }
            }
            return recievedParcels;

        }
        /// <summary>
        /// an update function that updates the customers name or phonenumber
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="Name"></param>
        /// <param name="phoneNum"></param>
        /// <exception cref="dosntExisetException"></exception>
        public void updateCustomer(int customerID, string Name = " ", int phoneNum = 0)
        {
            try
            {
                DO.Customer customerDl = new DO.Customer();
                customerDl = dal.GetCustomer(customerID);
                customerDl.name = Name;
                customerDl.phoneNumber = phoneNum;
                dal.updateCustomer(customerID, customerDl);
            }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
        }
        /// <summary>
        ///  recieving a customer (regular) form the  the datasource by recieving the id of the customer  and throwing an exception if the id was in correct
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public BO.Customer GetCustomer(int id)
        {
            try
            {
                List<BO.Parcel> tempParcels = GetParcels();
                BO.Customer CustomerBo = new BO.Customer();
                DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.Name = CustomerDo.name;
                CustomerBo.phoneNumber = CustomerDo.phoneNumber;
                CustomerBo.location = new Location() { latitude = CustomerDo.latitude, longitude = CustomerDo.longitude };
                CustomerBo.SentParcels = getCustomerShippedParcels(id).ToList();
                CustomerBo.ReceiveParcel = CustomerReceiveParcel(id).ToList();
                return CustomerBo;
            }
            catch (DO.findException Fex)
            {
                throw new validException(Fex.Message);
            }
        }
        public IEnumerable<CustomerInList> GetCustomerToList()
        {
            List<CustomerInList> customerToLists = new();
            foreach (var item in dal.GetCustomerList())
            {
                CustomerInList station = new CustomerInList
                {
                    id = item.id,
                    Name = item.name,
                    PhoneNumber = item.phoneNumber,
                    Parcles_Delivered_Recieved = dal.GetParcelList().Count(x => x.senderId == item.id && x.requested != null && x.scheduled != null && x.pickedUp != null && x.delivered != null&& x.isRecived==true),
                    Parcels_unrecieved = dal.GetParcelList().Count(x => x.senderId == item.id && x.requested != null && x.scheduled != null && x.pickedUp != null && x.delivered == null),
                    Recieved_Parcels = GetCustomer(item.id).ReceiveParcel.Count(),
                    ParcelsInDeliver =dal.GetParcelList().Count(x => x.senderId == item.id && x.requested != null && x.scheduled != null && x.pickedUp != null && x.delivered != null && x.isRecived ==false),
                };
                customerToLists.Add(station);
            }
            return customerToLists.Take(customerToLists.Count).ToList();
        }


        #region Get Customer
        /// <summary>
        ///  recieving a customer (customer to list ) form the  the datasource by recieving the id of the customer  and throwing an exception if the id was in correct
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public BO.CustomerInList GetCustomerToList(int id)
        {
            try
            {
                List<BO.Parcel> tempParcels = GetParcels();
                BO.CustomerInList CustomerBo = new();
                DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.Name = CustomerDo.name;
                CustomerBo.PhoneNumber = CustomerDo.phoneNumber;
                CustomerBo.Parcles_Delivered_Recieved = GetParcels().Count(x => x.delivered != null && x.pickedUp == null);
                CustomerBo.Parcels_unrecieved = GetParcels().Count(x => x.pickedUp == null);   //נראלי שזה שלא קבלו אומר שהם לא אספו
                CustomerBo.Recieved_Parcels = CustomerReceiveParcel(id).Count;////לא בטוחה צריך לבדוק את זה
                CustomerBo.ParcelsInDeliver = GetParcels().Count(x => x.delivered != null);
                return CustomerBo;
            }
            catch (DO.findException Fex)
            {
                throw new validException($"Customer id {id}", Fex);
            }
        }
        #endregion
        /// <summary>
        ///  recieving a list pf all the  customers (customer regular) form the  the datasource by recieving the id of the customer  and throwing an exception if the id was in correct
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public List<BO.Customer> GetCustomers()
        {
            List<BO.Customer> customers = new List<BO.Customer>();
            foreach (var c in dal.GetCustomerList())
            { customers.Add(GetCustomer(c.id)); }
            return customers;
        }
        /// <summary>
        ///  recieving a list pf all the  customers (customer to list) form the  the datasource by recieving the id of the customer  and throwing an exception if the id was in correct
        /// </summary>
        /// <returns></returns>
        public List<BO.CustomerInList> GetCustomersToList()
        {
            List<BO.CustomerInList> customers = new();
            foreach (var c in dal.GetCustomerList())
            { customers.Add(GetCustomerToList(c.id)); }
            return customers;
        }
        #region Add Customer
        /// <summary>
        /// adding a customer to the data source and with customer regular features and throwing an exception if any of the users info was incorrect
        /// </summary>
        /// <param name="CustomerToAdd"></param>
        /// <exception cref="validException"></exception>
        /// <exception cref="AlreadyExistException"></exception>
        public void addCustomer(BO.Customer CustomerToAdd)
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
            DO.Customer CustomerDo = new DO.Customer();
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

