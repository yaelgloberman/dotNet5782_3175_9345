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
            public bool checkCustomer(int id)
            {
                return DataSource.Customers.Any(c => c.id == id);
            }
            public void addCustomer(Customer c)
            {
                if (DataSource.Customers.Exists(item => item.id == c.id))
                    throw new AddException("Customer already exist");
                DataSource.Customers.Add(c);
            }
            
            public Customer GetCustomer(int id)//function that gets id and finding the Customer in the Customers list and returns Customer
            {
                Customer? tmp = null;
                foreach (Customer c in DataSource.Customers)
                {
                    if (c.id == id)
                    {
                        tmp = c;
                        break;
                    }
                }
                if (tmp == null)
                {
                    throw new findException("Customer does not exist");
                }
                return (Customer)tmp;
            }
            public void deleteCustomer(Customer c)
            {
                if (!DataSource.Customers.Exists(item => item.id == c.id))
                    throw new findException("Customer");
                DataSource.Customers.Remove(c);

            }
        }
    }
    
