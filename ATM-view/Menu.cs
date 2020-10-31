using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ATM_BO;
using ATM_BLL;

namespace ATM_view
{
    public class Menu
    {
       
        public Menu()
        {
        }

        public int presentUserChooseScreen()
        {
            Console.WriteLine("1: Login as Admin");
            Console.WriteLine("2: Login as Customer");
            return int.Parse(Console.ReadLine());
        }

        public Dictionary<String, String> presentLoginMenu(Power p)
        {
            if (p == Power.Root)
            {
                Console.WriteLine("Admin Login");
            }
            else if (p == Power.Noob)
            {
                Console.WriteLine("Customer Login");
            }
            Console.WriteLine("Enter login");
            String username = Console.ReadLine();
            Console.WriteLine("Enter password");
            String pass = Console.ReadLine();
            Dictionary<String, String> d = new Dictionary<String, String>();
            d.Add("username", username);
            d.Add("password", pass);
            return d;
        }


        public int presentCustomerMenu()
        {
            Console.WriteLine("Customer menu");
            Console.WriteLine("1: Withdraw cash");
            Console.WriteLine("2: CashTransfer");
            Console.WriteLine("3: Deposit cash");
            Console.WriteLine("4: Display Balance");
            Console.WriteLine("5: Exit");
            return int.Parse(Console.ReadLine());
        }

        public int withdrawCashMenu()
        {
            Console.WriteLine("1: fast cash");
            Console.WriteLine("2: Normal cash");
            return int.Parse(Console.ReadLine());
        }

        public int presentFastCashMenu()
        {
            Console.WriteLine("1: 500");
            Console.WriteLine("2: 1000");
            Console.WriteLine("3: 2000");
            Console.WriteLine("4: 5000");
            Console.WriteLine("5: 10000");
            Console.WriteLine("6: 15000");
            Console.WriteLine("7: 20000");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    return 500;
                case 2:
                    return 100;
                case 3:
                    return 2000;
                case 4:
                    return 5000;
                case 5:
                    return 10000;
                case 6:
                    return 15000;
                case 7:
                    return 20000;
                default:
                    return 500;
            }
        }

        public int presentNormalCashMenu()
        {
            Console.WriteLine("Enter the ammount to withdraw");
            return int.Parse(Console.ReadLine());
        }


        public void displaySearchedCustomers(List<Customer> ls)
        {
            Console.WriteLine("===== Search Results====");

            foreach (Customer c in ls)
            {
                Console.WriteLine($"Account id{c.id}");
                Console.WriteLine($"username{c.userName}");
                Console.WriteLine($"name {c.name}");
                Console.WriteLine($"Account Type {c.type.ToString()}");
                Console.WriteLine($"Account balance {c.balance}");
                Console.WriteLine($"Account status {c.status.ToString()}");
            }
        }


        public int presentAdminMenu()
        {
            Console.WriteLine("1: Create new account");
            Console.WriteLine("2: Delete existing  account");
            Console.WriteLine("3: Update Account information");
            Console.WriteLine("4: Search existing account");
            Console.WriteLine("5: View reports");
            Console.WriteLine("6: Exit");
            return int.Parse(Console.ReadLine());
        }

        public int presentDepositCashMenu()
        {
            Console.WriteLine("Enter ammount to deposit");
            return int.Parse(Console.ReadLine());
        }

        public Customer getNewCustomerInfo(int id)
        {
            Console.WriteLine("Login: ");
            String username = Console.ReadLine();
            Console.WriteLine("PinCode:");
            String pinCode = Console.ReadLine();
            Console.WriteLine("Holders name");
            String name = Console.ReadLine();
            Console.WriteLine("Type: Savings / Current ");
            String type = Console.ReadLine().ToLower();
            Console.WriteLine("Starting balance");
            int balance = -1;
            try
            { 
               balance = int.Parse(Console.ReadLine());
            }catch(Exception e)
            {

                balance = -1;
            }
             
            Console.WriteLine("Status: Active/Disabled");
            string status = Console.ReadLine().ToLower();
            Customer.AccountType accType;
            if (type == "savings")
            {
                accType = Customer.AccountType.Savings;
            }
            else if (type == "current")
            {
                accType = Customer.AccountType.Current;
            }
            else
            {
                accType = Customer.AccountType.None;
            }

            Customer.Status accStatus;

            if (status == "active")
            {
                accStatus = Customer.Status.Active;
            }
            else if (status == "disabled")
            {
                accStatus = Customer.Status.Disabled;
            }
            else
            {
                accStatus = Customer.Status.None;
            }

            return new Customer(id, name, pinCode, accType, accStatus,balance, username,Power.Noob);

        }


        public int getCustomerID()
        {
            bool valid = false;
            int id = 0;
            while (!valid)
            {
                Console.WriteLine("Enter ID of customer");
                try
                {
                    id = int.Parse(Console.ReadLine());
                    valid = true;
                }
                catch(Exception e)
                {
                    valid = false;
                }
               
                
            }
            return id;

            
        }

        public void displayCustomerInfo(Customer c)
        {
            Console.WriteLine($"Account id: {c.id}");
            Console.WriteLine($"Login: {c.userName}");
            Console.WriteLine($"Account Hodlers name: {c.name}");
            Console.WriteLine($"Account id: {c.id}");
            Console.WriteLine($"Account Type: {c.type.ToString()}");
            Console.WriteLine($"Account status: {c.status.ToString()}");
            Console.WriteLine($"Account balance: {c.balance}");
        }


        public Dictionary<String,int> presentMoneyTransferScreen()
        {
            Dictionary<String, int> values = new Dictionary<string, int>();
            Console.WriteLine("Enter amount in multiples of 500");
            int amount = 0;
            Boolean repeat = true;
            while(repeat)
            {
                try
                {
                    amount = int.Parse(Console.ReadLine());
                    if((amount%500)!=0 || amount<=0)
                    {
                        repeat = true;
                    }
                    else
                    {
                        repeat = false;
                    }
    
                }
                catch (Exception e)
                {

                    repeat = true;
                }
            }

            Console.WriteLine("Enter the account number you want to transfer to: ");
            int accountNumber = -1;
            repeat = true;
            while (repeat)
            {
                try
                {
                    accountNumber = int.Parse(Console.ReadLine());
                    repeat = false;
          
                }
                catch (Exception e)
                {

                    repeat = true;
                }
            }

            values.Add("money", amount);
            values.Add("ID", accountNumber);
            return values;

        }

        public int presentViewReportsByMenu()
        {
            Console.WriteLine("1: Accounts By balance");
            Console.WriteLine("2: Accounts By Date");
            int choice = -1;
            Boolean repeat = true;
            while(repeat)
            {
                try
                {
                    choice = int.Parse(Console.ReadLine());
                    if (choice == 1 || choice == 2)
                    {
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine("Wrong input try again");
                        repeat = true;
                    }
                }catch(Exception e)
                {
                    Console.WriteLine("Bad input");
                    repeat = true;
                }
            }

            return choice;
           
        }

        public Dictionary<String,int> getPriceRangeForSearch()
        {
            int min = -1;
            int max = -1;
            Console.WriteLine("Enter the minimum amount");
            while(true)
            {
                try
                {
                    min = int.Parse(Console.ReadLine());
                    if(min>0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No not in good range");
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Bad input try again");

                }
            }
            Console.WriteLine("Enter the maximum amount");
            while (true)
            {
                try
                {
                    max = int.Parse(Console.ReadLine());
                    if (min > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No not in good range");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Bad input try again");

                }
            }

            Dictionary<String, int> values = new Dictionary<string, int>();
            values.Add("min", min);
            values.Add("max", max);
            return values;

        }

        public Dictionary<String,String> getDatesrangeForSearch()
        {
            String startDate;
            String endDate;

            Console.WriteLine("Enter starting date");
            startDate = Console.ReadLine();
            Console.WriteLine("Enter end date:");
            endDate = Console.ReadLine();
            Dictionary<String, String> values = new Dictionary<string,string>();
            values.Add("startDate", startDate);
            values.Add("endDate", endDate);
            return values;

        }

        public Boolean showReciepts(List<Customer.Reciept> reciepts)
        {
            foreach(Customer.Reciept r in reciepts)
            {
                Console.WriteLine(r);
            }
            return true;
        }
    }
}
