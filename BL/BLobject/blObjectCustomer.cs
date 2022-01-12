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
        #region list of CustomerReceiveParcel&CustomerSentParcel
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
        public List<ParcelCustomer> CustomerSentParcel(int customerID)
        {
            List<BO.ParcelCustomer> sentParcels = new List<ParcelCustomer>();
            foreach (var parcel in GetParcels())
            {
                if (parcel.sender.id == customerID)
                {
                    BO.ParcelCustomer customersPrevioseParcel = new ParcelCustomer { CustomerInParcel = parcel.receive, id = parcel.id, parcelStatus = ParcelStatus.Delivered, priority = parcel.priority, weight = parcel.weightCategorie };
                    sentParcels.Add(customersPrevioseParcel);
                }
            }
            return sentParcels;
        }
        #endregion
        #region update
        /// <summary>
        /// an update function that updates the customers name or phonenumber
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="Name"></param>
        /// <param name="phoneNum"></param>
        /// <exception cref="dosntExisetException"></exception>
        /// 
        public void updateCustomer(int customerID, string Name, string phoneNum)
        {
            try
            {
                DO.Customer customerDl = new();
                customerDl = dal.GetCustomer(customerID);
                customerDl.name = Name;
                customerDl.phoneNumber = phoneNum;
                dal.updateCustomer(customerID, customerDl);
            }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
        }
        #endregion
        #region Get functions
        public ParcelStatus GetParcelStatus(int id)
        {
            var parcelStatus = GetParcel(id);
            if (parcelStatus.delivered != null)
                return ParcelStatus.Delivered;
            if (parcelStatus.pickedUp != null)
                return ParcelStatus.PickedUp;
            if (parcelStatus.scheduled != null)
                return ParcelStatus.Created;
            return ParcelStatus.Assigned;
        }
        public Priority GetParcelPriorty(int id)
        {
            var priorty = GetParcel(id);
            if (Priority.emergency != 0)
                return Priority.emergency;
            if (Priority.fast != 0)
                return Priority.fast;
            if (Priority.regular != 0)
                return Priority.regular;
        }
        public ParcelCustomer GetParcelToCustomer(int id)
        {
            List<BO.Customer> customers = new List<BO.Customer>();
            customers.Where(x => x.id == id);
            {
                ParcelCustomer pc = new ParcelCustomer()
                {
                    id = id,
                    weight = GetParcel(id).weightCategorie,
                    parcelStatus = GetParcelToList(id).parcelStatus,
                    priority = GetParcelToList(id).priority,
                    CustomerInParcel = null,

                };
                return pc;
            }
        }
        public List<BO.Customer> GetCustomers()
        {
            List<BO.Customer> customers = new List<BO.Customer>();
            try
            {
                var p = GetParcels();
                foreach (var c in dal.GetCustomerList()) { customers.Add(GetCustomer(c.id)); };
                foreach (var c in dal.GetCustomerList())
                {
                    p.Where(p => p.delivered != null && p.receive.id == c.id);
                    GetCustomer(c.id).ReceiveParcel.Add(GetParcelToCustomer(c.id));
                };
            }
            catch (dosntExisetException exp) { throw new dosntExisetException(exp.Message); }
            return customers;

        }

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
                CustomerBo.Password = CustomerDo.Password;
                CustomerBo.isCustomer = CustomerDo.isCustomer;
                CustomerBo.PhoneNumber = CustomerDo.phoneNumber;
                CustomerBo.Parcles_Delivered_Recieved = CustomerSentParcel(id).Count;
                CustomerBo.Parcels_Delivered_unrecieved = CustomerSentParcel(id).Count;   //נראלי שזה שלא קבלו אומר שהם לא אספו
                CustomerBo.Recieved_Parcels = CustomerReceiveParcel(id).Count;////לא בטוחה צריך לבדוק את זה
                CustomerBo.ParcelsInDeliver = CustomerSentParcel(id).Count;
                return CustomerBo;
            }
            catch (DO.findException Fex)
            {
                throw new validException($"Customer id {id}", Fex);
            }
        }

        /// <summary>
        ///  recieving a customer (regular) form the  the datasource by recieving the id of the customer  and throwing an exception if the id was in correct
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        /// 

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
                CustomerBo.PassWord = CustomerDo.Password;
                CustomerBo.location = new Location() { latitude = CustomerDo.latitude, longitude = CustomerDo.longitude };
                IEnumerable<DO.Parcel> lstP = dal.GetParcels();
                CustomerBo.SentParcels = new List<ParcelCustomer>();
                CustomerBo.ReceiveParcel = new List<ParcelCustomer>();
                foreach (var item in lstP)
                {
                    //מוצא את כל החבילות שהלקוח מקבל
                    if (item.targetId == CustomerBo.id)
                    {
                        ParcelCustomer tmp = new();
                        tmp.id = item.id;
                        tmp.parcelStatus = GetParcelStatus(item.id);
                        tmp.priority = (BO.Priority)item.priority;
                        tmp.CustomerInParcel = new CustomerInParcel();
                        tmp.CustomerInParcel.id = item.senderId;
                        tmp.CustomerInParcel.name = dal.GetCustomer(item.senderId).name;
                        CustomerBo.ReceiveParcel.Add(tmp);
                    }
                    //מוצא את כל החבילות שהלקוח שולח
                    if (item.senderId == CustomerBo.id)
                    {
                        ParcelCustomer tmp = new();
                        tmp.id = item.id;
                        tmp.parcelStatus = GetParcelStatus(item.id);
                        tmp.CustomerInParcel = new CustomerInParcel();
                        tmp.CustomerInParcel.id = item.targetId;
                        tmp.CustomerInParcel.name = dal.GetCustomer(item.targetId).name;
                        tmp.priority = GetParcelPriorty(item.id);
                        CustomerBo.SentParcels.Add(tmp);
                    }
                }
                return CustomerBo;
            }
            catch (DO.findException Fex)
            {
                throw new validException(Fex.Message);
            }
        }
        public BO.CustomerInParcel GetCustomerParcel(int id)
        {
            try
            {
                List<BO.Parcel> tempParcels = GetParcels();
                BO.CustomerInParcel CustomerBo = new();
                DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.name = CustomerDo.name;
                return CustomerBo;
            }
            catch (DO.findException Fex)
            {
                throw new validException(Fex.Message);
            }
        }

        #endregion
        #region get customer
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
                    Parcles_Delivered_Recieved = GetCustomer(item.id).SentParcels.Where(s => s.parcelStatus == ParcelStatus.Delivered).Count(), //dal.GetParcelList().Count(x => x.senderId == item.id && x.requested != null && x.scheduled != null && x.pickedUp != null && x.delivered != null && x.isRecived == true),
                    Parcels_Delivered_unrecieved = GetCustomer(item.id).SentParcels.Where(s => s.parcelStatus == ParcelStatus.Delivered).Count(),//dal.GetParcelList().Count(x => x.senderId == item.id && x.requested != null && x.scheduled != null && x.pickedUp != null && x.delivered == null),
                    Recieved_Parcels = GetCustomer(item.id).ReceiveParcel.Where(s => s.parcelStatus == ParcelStatus.Delivered).Count(), //GetCustomer(item.id).ReceiveParcel.Count(),
                    ParcelsInDeliver = GetCustomer(item.id).SentParcels.Where(s => s.parcelStatus != ParcelStatus.Delivered).Count() //dal.GetParcelList().Count(x => x.senderId == item.id && x.requested != null && x.scheduled != null && x.pickedUp != null && x.delivered != null && x.isRecived == false),
                };
                customerToLists.Add(station);
            }
            return customerToLists.Take(customerToLists.Count).ToList();
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
        #endregion 
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
            //if (!(CustomerToAdd.phoneNumber >= 500000000 && CustomerToAdd.phoneNumber <= 0589999999))
            //    throw new validException("the phone number of the Customer is invalid\n");
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
            CustomerDo.Password = CustomerToAdd.PassWord;
            CustomerDo.isCustomer = CustomerToAdd.isCustomer;
            CustomerDo.isActive = true;
            try
            {
                dal.addCustomer(CustomerDo);
            }
            catch (AddException exp)
            {

                throw new AlreadyExistException("the customer already exist", exp);
            }
        }
        #endregion
    }
}



