using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace ATM_BO
{
    public class CustomerManager
    {
        private List<Customer> customerList;
        private CustomerManager()
        {

        }
        public CustomerManager(List<Customer> list)
        {
            this.customerList = list;
        }

        public Boolean addCustomer(Customer c)
        {

            try
            {
                customerList.Add(c);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public List<Customer> getCustomers()
        {
            return customerList;
        }

        public List<Customer> findCustomers(int? id, String username, String name, Customer.AccountType type, int? balance, Customer.Status status)
        {
            List<Customer> ls = new List<Customer>();
            Console.WriteLine(username);
            if (id != null)
            {
                Console.WriteLine("in userid find");
                Customer c = customerList.Find(cstmr => cstmr.id == id);
                if (c != null)
                {
                    ls.Add(c);
                    return ls;
                }
                else
                {
                    return ls;
                }
            }

            if (username != "")
            {

                Console.WriteLine("in user name find");
                Customer c = customerList.Find(cstmr => cstmr.userName.Equals(username));
                if (c != null)
                {
                    ls.Add(c);

                    return ls;
                }
                else
                {
                    return ls;
                }
            }


            if (type != Customer.AccountType.None && balance != -1 && status != Customer.Status.None)
            {
                if (name != "")
                {
                    return customerList.FindAll(c => c.name == name && c.type == type && c.balance == balance && c.status == status);
                }
                return customerList.FindAll(c => c.type == type && c.balance == balance && c.status == status);
            }
            else if (type != Customer.AccountType.None && balance != -1)
            {
                if (name != "")
                {
                    return customerList.FindAll(c => c.name == name && c.type == type && c.balance == balance);
                }
                return customerList.FindAll(c => c.type == type && c.balance == balance);
            }
            else if (type != Customer.AccountType.None && status != Customer.Status.None)
            {
                if (name != "")
                {
                    return customerList.FindAll(c => c.name == name && c.type == type && c.status == status);
                }
                return customerList.FindAll(c => c.type == type && c.status == status);
            }
            else if (balance != -1 && status != Customer.Status.None)
            {
                if (name != "")
                {
                    return customerList.FindAll(c => c.name == name && c.balance == balance && c.status == status);
                }
                return customerList.FindAll(c => c.balance == balance && c.status == status);
            }
            else if (balance != -1)
            {
                if (name != "")
                {
                    return customerList.FindAll(c => c.name == name && c.balance == balance);
                }
                return customerList.FindAll(c => c.balance == balance);
            }
            else if (type != Customer.AccountType.None)
            {
                if (name != "")
                {
                    return customerList.FindAll(c => c.name == name && c.type == type);
                }
                return customerList.FindAll(c => c.type == type);
            }
            else if (status != Customer.Status.None)
            {
                if (name != "")
                {
                    return customerList.FindAll(c => c.name == name && c.status == status);
                }
                return customerList.FindAll(c => c.status == status);
            }
            else if (name != "")
            {
                return customerList.FindAll(c => c.name == name);
            }

            return ls;
        }


        public Customer findCustomerByID(int id)
        {
            Customer cstmr=customerList.Find(c => c.id == id);
            return cstmr;
        }

        public void updateCustomerInfo(Customer c)
        {
            Customer toUpdate = findCustomerByID(c.id);

            if(c.userName!="")
            {
                toUpdate.userName = c.userName;
            }

            if (c.name != "")
            {
                toUpdate.name = c.name;
            }


            if (c.pinCode != "")
            {
                toUpdate.pinCode = c.pinCode;
            }

            if(c.balance!=-1)
            {
                toUpdate.balance = c.balance;
            }

            if (c.type != Customer.AccountType.None)
            {
                toUpdate.type = c.type;
            }

            if(c.status!=Customer.Status.None)
            {
                toUpdate.status = c.status;
            }

        }

        public List<Customer> filterByBalanceRange(int min,int max)
        {
            return customerList.FindAll(c => c.balance >= min && c.balance <= max);
        }

        public List<Customer.Reciept> filterByDateRange(DateTime start,DateTime end)
        {
            List<Customer.Reciept> reciepts = new List<Customer.Reciept>();
            foreach(Customer c in customerList)
            {
                foreach(Customer.Reciept r in c.reciepts)
                {
                    DateTime recieptTime = DateTime.Parse(r.date);
                    if(recieptTime.CompareTo(start)>=0 && recieptTime.CompareTo(end)<=0)
                    {
                        reciepts.Add(r);
                    }
                }
            }

            return reciepts;
        }
    }
}
