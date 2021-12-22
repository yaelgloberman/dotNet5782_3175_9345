using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
namespace Dal
{
    sealed partial class DalObject : IDal
    {
        public bool checkCustomer(int id)
        {
            return DataSource.Customers.Any(c => c.id == id);
        }
        public void addCustomer(Customer c)
        {

            if (DataSource.Customers.Exists(item => item.id == c.id))
            {
                if (c.isActive == false)
                {
                    DataSource.Customers.Remove(c);
                    c.isActive = true;
                    DataSource.Customers.Add(c);
                    return;
                }
                else
                    throw new AddException("Customer already exist");
            }
            DataSource.Customers.Add(c);

        }

        public Customer GetCustomer(int id)//function that gets id and finding the Customer in the Customers list and returns Customer
        {
            foreach(Customer item in DataSource.Customers)
            {
                if (item.id == id)
                    return item;
            }
            throw new findException("customer");
        }
        public IEnumerable<Customer> GetCustomers()
        {
           
            return DataSource.Customers;
        }
        public IEnumerable<Parcel> GetCustomerReceivedParcels(int customerId) //אני לא בטוחה שזה טוב אבל מה שניסיתי לעשות זה לבדוק ברשיה של כל החבילות אם התז אותו דבר כמו של הלקוח וגם המאפיין הבוליאני אם רבלתי שוו  אז החבילה שייכת לו
        {
            IEnumerable<Parcel> parcelTemp = new List<Parcel>();
            DataSource.parcels.ForEach(p => { if (p.targetId == customerId && p.isRecived) parcelTemp.ToList().Add(p); });
            return parcelTemp;
        }
        public IEnumerable<Parcel> getCustomerShippedParcels(int customerId) //אני לא בטוחה שזה טוב אבל מה שניסיתי לעשות זה לבדוק ברשיה של כל החבילות אם התז אותו דבר כמו של הלקוח וגם המאפיין הבוליאני אם רבלתי שוו  אז החבילה שייכת לו
        {
            IEnumerable<Parcel> parcelTemp = new List<Parcel>();
            DataSource.parcels.ForEach(p => { if (p.targetId == customerId && p.isShipped) parcelTemp.ToList().Add(p); });
            return parcelTemp;
        }
        
        public void deleteCustomer(Customer c)
        {
            if (!DataSource.Customers.Exists(item => item.id == c.id))
                throw new findException("Customer");
            DataSource.Customers.Remove(c);
            c.isActive = false;
            DataSource.Customers.Add(c);
        }
        public string GetCustomerName(int id)
        {
            string name=GetCustomer(id).name;
            return name;
        }
        public void updateCustomer(int customerId, Customer cust)
        {
            bool flag = false;

            DataSource.Customers.ForEach(c => { if (c.id == customerId) { c=cust; flag = true; } });
            if (!flag)
            {
                throw new findException("could not find customer");
            }

        }
        public IEnumerable<Customer> GetCustomer(Func<Customer, bool> predicate = null)
      => predicate == null ? DataSource.Customers : DataSource.Customers.Where(predicate);
    }
}
    
