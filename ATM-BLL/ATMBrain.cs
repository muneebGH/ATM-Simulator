using System;
using ATM_DAL;
using ATM_BO;
using System.Collections.Generic;
namespace ATM_BLL
{
    public class ATMBrain
    {
        ApplicationContext context;
        CustomerManager cManager;
        public ATMBrain()
        {
            init();
            
        }

        public void init()
        {
            context = ApplicationContext.getContext();
            ATMData data = ATMDao.loadData();
            context.Data = data;
            cManager = new CustomerManager(data.customerList);
            setListners();
        }
        public Boolean logInUser(String username, String pass, Power p)
        {
            if(p==Power.Root)
            {
                if(username=="muneeb" && pass=="admin")
                {
                    context.RootMode = true;
                    return true;
                }
                else
                {
                    context.Error = "Passowrd incorrect for admin";
                    return false;

                }
            }
            Customer customer = context.AllCustomers.Find(c => c.userName.Equals(username) && c.pinCode.Equals(pass) && c.power == p);
            if (customer == null)
            {
                context.Error = "User not found";
                return false;
            }
            else
            {
                context.HasError = false;
                context.CurrentCustomer = customer;
                return true;
            }


        }


        public Boolean withdrawCash(int amount,Boolean transfer)
        {
            Console.WriteLine($"withdrawn cash amount ${amount}");
            if (context.CurrentCustomer.status == Customer.Status.Disabled)
            {
                context.Error = "Account disabled";
                return false;
            }

            int alreadyWithdrawn = cManager.getTodaysTransactionOfCustomer(context.CurrentCustomer.id);

            if(alreadyWithdrawn+amount>=20000)
            {
                context.Error = "Limit reached";
                return false;
            }

            int balance = context.CurrentCustomer.balance;
            if(balance-amount<0)
            {
                context.Error = "Not Enough Cash";
                return false;
            }
            

            context.CurrentCustomer.balance -= amount;
            if (transfer)
            {
                context.CurrentCustomer.addReciept(DateTime.Now.ToString("dd/MM/yyyy"), amount, context.CurrentCustomer.balance, Customer.Reciept.RecieptType.TRANSFERED);
            }
            else
            {
                context.CurrentCustomer.addReciept(DateTime.Now.ToString("dd/MM/yyyy"), amount, context.CurrentCustomer.balance, Customer.Reciept.RecieptType.WITHDRAWN);
            }
            return true;
        }

        public Boolean depositCash(int amount, int? id)
        {

            
            int unWrappedID = id ?? -1;
            if(unWrappedID==-1)
            {
                context.CurrentCustomer.balance += amount;
                context.CurrentCustomer.addReciept(DateTime.Now.ToString("dd/MM/yyyy"), amount, context.CurrentCustomer.balance,Customer.Reciept.RecieptType.DEPOSITED);
                return true;
            }

            Customer c = getInfoOfCustomerByID(unWrappedID);
            c.balance += amount;
            c.addReciept(DateTime.Now.ToString("dd/MM/yyyy"), amount, c.balance, Customer.Reciept.RecieptType.DEPOSITED);

            return true;
            
        }




        public void performExitOperations()
        {
            ATMDao.saveCustomers(context.Data);
        }



        public List<Customer> findBy(int? id,String username, String name,Customer.AccountType type,int? balance,Customer.Status status)
        {
            return cManager.findCustomers(id, username, name, type, balance, status);

        }

   

        public Boolean deleteCustomer(int id)
        {
            Customer cstmr = context.AllCustomers.Find(c => c.id == id);
            if(cstmr==null)
            {
                context.Error = "There is no customer to delete";
                return false;

            }

            context.AllCustomers.RemoveAll(c=> c.id==id);
            return true;
        }

        public Boolean addUser(Customer c)
        {
            if(!authorizeRootOperation())
            {
                return false;
            }
            c.id = context.Data.presentID++;
            cManager.addCustomer(c);
            return true;
            
        }

        private Boolean authorizeRootOperation()
        {
            if (context.RootMode)
            {
                return true;
            }

            context.Error = "Not authorized for this";
            return false;

        }


        public Customer getInfoOfCustomerByID(int id)
        {
            Customer c = cManager.findCustomerByID(id);
            if(c==null)
            {
                context.Error = "No customer with this id ";
                return null;
            }
            else
            {
                return c;
            }
        }

        public void updateCustomer(Customer c)
        {
            cManager.updateCustomerInfo(c);
        }

        public static void contextErrorListner(String error)
        {
        }
        public static void contextCustomerLoggedInListner(Customer c)
        {
        }
        public static void contextCustomerLoggedOutListner(Customer c)
        {

        }
        public static void contextCustomerObjChangedListner(Customer c)
        {
        }

        public List<Customer> GetCustomersInBalanceRange(Dictionary<String,int> values)
        {
            return cManager.filterByBalanceRange(values["min"], values["max"]);
        }

        public List<Customer.Reciept> getCustomerRecieptsInRange(String start,String end)
        {
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = DateTime.Parse(end);
            return cManager.filterByDateRange(startDate, endDate);
        }
        private void setListners()
        {
            context.SomeErrorRecieved += ATMBrain.contextErrorListner;
            context.CustomerLoggedIn += ATMBrain.contextCustomerLoggedInListner;
            context.CustomerLoggedOut += ATMBrain.contextCustomerLoggedOutListner;
            context.CustomerObjChanged += ATMBrain.contextCustomerObjChangedListner;
        }

    }
}
