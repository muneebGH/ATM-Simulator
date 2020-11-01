using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ATM_BO;
using ATM_BLL;

namespace ATM_view
{
    public class Menu
    {

        Action<String> cl = Console.WriteLine;
        public Menu()
        {
        }

        public int presentUserChooseScreen()
        {
            int ret;
            while(true)
            {
                Console.WriteLine("1: Login as Admin");
                Console.WriteLine("2: Login as Customer");
                try
                {
                    ret=int.Parse(Console.ReadLine());
                    if(ret==1 || ret==2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Bad range: again");
                    }
                    
                }
                catch(Exception _) { Console.WriteLine("Wrong input: Again"); }
            }
            return ret;
        }

        public Dictionary<String, String> presentLoginMenu(Power p)
        {
            Dictionary<String, String> d;
            while (true)
            {
                if (p == Power.Root)
                {
                    Console.WriteLine("-----Admin Login-----");
                }
                else if (p == Power.Noob)
                {
                    Console.WriteLine("----Customer Login----");
                }
                Console.WriteLine("Enter login");
                String username = Console.ReadLine();
                Console.WriteLine("Enter password");
                String pass = Console.ReadLine();
                if(username!="" && pass!="")
                {
                    d = new Dictionary<String, String>();
                    d.Add("username", username);
                    d.Add("password", pass);
                    break;
                }
                else
                {
                    Console.WriteLine("bad inputs: again");
                }
                
            }
            
            return d;
        }


        public int presentCustomerMenu()
        {
            int ret;
            while(true)
            {
                Console.WriteLine("Customer menu");
                Console.WriteLine("1: Withdraw cash");
                Console.WriteLine("2: CashTransfer");
                Console.WriteLine("3: Deposit cash");
                Console.WriteLine("4: Display Balance");
                Console.WriteLine("5: Exit");
                try
                {
                    ret = int.Parse(Console.ReadLine());
                    if(ret>=1 && ret<=5)
                    {
                        break;
                        
                    }
                    else
                    {
                        cl("Wrong input range try again");
                    }
                }catch(Exception _)
                {
                    cl("Bad input: try again...");
                }
                

            }

            return ret;
           
        }

        public int withdrawCashMenu()
        {
            int ret;
            while(true)
            {
                Console.WriteLine("1: fast cash");
                Console.WriteLine("2: Normal cash");
                try
                {
                    ret = int.Parse(Console.ReadLine());
                    if(ret==1 || ret==2)
                    {
                        break;
                    }
                    else
                    {
                        cl("bad input range: try again...");
                    }
                }
                catch(Exception _)
                {
                    cl("bad input: try again...");
                }
                
            }

            return ret;
            
        }

        public int presentFastCashMenu()
        {
            while(true)
            {
                Console.WriteLine("1: 500");
                Console.WriteLine("2: 1000");
                Console.WriteLine("3: 2000");
                Console.WriteLine("4: 5000");
                Console.WriteLine("5: 10000");
                Console.WriteLine("6: 15000");
                Console.WriteLine("7: 20000");
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    if(choice>=1 && choice<=7)
                    {
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
                    else
                    {
                        cl("wrong input range: try again...");
                    }
                    
                }
                catch(Exception _)
                {
                    cl("bad input: try again...");
                }
            }
            
        }

        public int presentNormalCashMenu()
        {
            int ret;
            while(true)
            {
                Console.WriteLine("Enter the ammount to withdraw");
                try
                {
                    ret= int.Parse(Console.ReadLine());
                    if(ret>0)
                    {
                        break;
                    }
                    else
                    {
                        cl("This input isnt allowed...");
                    }
                }catch(Exception _)
                {
                    cl("bad input: try again...");
                }
            }
            return ret;
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

            int ret;
            while (true)
            {

                Console.WriteLine("1: Create new account");
                Console.WriteLine("2: Delete existing  account");
                Console.WriteLine("3: Update Account information");
                Console.WriteLine("4: Search existing account");
                Console.WriteLine("5: View reports");
                Console.WriteLine("6: Exit");
                try
                {
                    ret = int.Parse(Console.ReadLine());
                    if (ret >= 1 && ret <= 6)
                    {
                        break;

                    }
                    else
                    {
                        cl("Wrong input range try again");
                    }
                }
                catch (Exception _)
                {
                    cl("Bad input: try again...");
                }


            }

            return ret;
        }

        public int presentDepositCashMenu()
        {
            int ret;
            while(true)
            {
                Console.WriteLine("Enter ammount to deposit");
                try
                {
                    ret = int.Parse(Console.ReadLine());
                    if(ret>0)
                    {
                        break;
                    }
                    else
                    {
                        cl("This amount is allowed");
                    }
                }
                catch(Exception _)
                {
                    cl("Bad input try again");
                }
            }

            return ret;
            
        }

        public Customer getNewCustomerInfo()
        {
            String username="", pinCode="", name="", type="", status = "";
            while(username=="")
            {
                Console.WriteLine("Login: ");
                username= Console.ReadLine();
            }

            while(pinCode=="")
            {
                Console.WriteLine("PinCode:");
                pinCode = Console.ReadLine();
            }

            while(name=="")
            {
                Console.WriteLine("Holders name");
                name = Console.ReadLine();
            }

            while(type=="" || (type!="savings" && type!="current"))
            {
                Console.WriteLine("Type: Savings / Current ");
                type = Console.ReadLine().ToLower();
            }
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

            Console.WriteLine("Starting balance");
            int balance = -1;
            while(balance==-1)
            {
                try
                {
                    balance = int.Parse(Console.ReadLine());
                    if(balance>0)
                    {
                        break;
                    }
                    cl("Wrong input: again plz");
                }
                catch (Exception _)
                {
                    cl("Bad input: Again plz");
                    balance = -1;
                }
            }
            

            while(status=="" || (status!="active" && status!="disabled"))
            {
                Console.WriteLine("Status: Active/Disabled");
                status = Console.ReadLine().ToLower();
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

            return new Customer(-1, name, pinCode, accType, accStatus,balance, username,Power.Noob);

        }

        public Customer getNewCustomerInfoToUpdate(int id)
        {
            String username = "", pinCode = "", name = "", type = "", status = "";
            Console.WriteLine("Login: ");
            username = Console.ReadLine();

            Console.WriteLine("PinCode:");
            pinCode = Console.ReadLine();
        


            Console.WriteLine("Holders name");
            name = Console.ReadLine();

            do
            {
                Console.WriteLine("Type: Savings / Current ");
                type = Console.ReadLine().ToLower();
            }
            while (type != "" && (type != "savings" && type != "current"));
            
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

            Console.WriteLine("Starting balance");
            int balance = -1;
            while (balance == -1)
            {
                try
                {
                    balance = int.Parse(Console.ReadLine());
                    if (balance > 0 || balance==-1)
                    {
                        break;
                    }
                    cl("wrong input: Plz try again");
                }
                catch (Exception _)
                {
                    cl("Bad input: Again plz");
                    balance = -1;
                }
            }

            do
            {
                Console.WriteLine("Status: Active/Disabled");
                status = Console.ReadLine().ToLower();
            }
            while (status != "" && (status != "active" && status != "disabled"));
           

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

            return new Customer(id, name, pinCode, accType, accStatus, balance, username, Power.Noob);

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
                    if(id>=0)
                    {
                        valid = true;
                    }
                    else
                    {
                        cl("Wrong input range: Try again..");
                    }
                    

                }
                catch(Exception e)
                {
                    cl("Bad input: Try again");
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
            
            int amount = 0;
            Boolean repeat = true;
            while(repeat)
            {
                Console.WriteLine("Enter amount in multiples of 500");
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

                    cl("bad input: try again...");
                    repeat = true;
                }
            }

            
            int accountNumber = -1;
            repeat = true;
            while (repeat)
            {
                Console.WriteLine("Enter the account number you want to transfer to: ");
                try
                {
                    accountNumber = int.Parse(Console.ReadLine());
                    if(accountNumber>=0)
                    {
                        repeat = false;
                    }
                    else
                    {
                        cl("bad input: try again...");
                    }
                    
          
                }
                catch (Exception e)
                {
                    cl("bad input: try again...");
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
