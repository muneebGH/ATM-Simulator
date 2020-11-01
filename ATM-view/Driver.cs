using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ATM_BO;
using ATM_BLL;

namespace ATM_view
{
    public class Driver
    {


        private ATMBrain brain;
        private ApplicationContext context;
        private Menu menu;
        Action<String> cl = Console.WriteLine;


        public Driver()
        {
            brain = new ATMBrain();
            context = ApplicationContext.getContext();
            menu = new Menu();
           
           
        }


        public void runApplication()
        {

            try
            {
                int userType = menu.presentUserChooseScreen();


                Power choosenPower;
                if (userType == 1)
                {
                    choosenPower = Power.Root;
                }
                else
                {
                    choosenPower = Power.Noob;
                }

                Boolean passedLoginPhase = false;
                while (!passedLoginPhase)
                {
                    Dictionary<String, String> credentials = menu.presentLoginMenu(choosenPower);
                    Boolean success = brain.logInUser(credentials["username"], credentials["password"], choosenPower);
                    if (!success)
                    {
                        Console.WriteLine("--" + context.Error + "----");
                        context.clearErrors();
                    }
                    else
                    {
                        passedLoginPhase = true;
                    }
                }

                if (choosenPower == Power.Noob)
                {
                    handleCustomerMenu();
                }
                else
                {
                    handleAdminMenu();
                }
            }catch(Exception e)
            {
                Console.WriteLine($" Error {e.Message}");
                Console.WriteLine("--saving state--");
                brain.performExitOperations();
                cl("Exiting...");
            }
            


        }

        private void handleCustomerMenu()
        {
            Boolean repeat = true;
            while (repeat)
            {
                int choose = menu.presentCustomerMenu();
                switch (choose)
                {
                    case 1:
                        while (!handleWithdrawCashMenu()) ;
                        break;
                    case 2:
                        Boolean again = true;
                        while(again)
                        {
                            Dictionary<String, int> values = menu.presentMoneyTransferScreen();
                            Boolean success = handleMoneyTransfer(values);
                            if(!success)
                            {
                                Console.WriteLine(context.Error);
                                context.clearErrors();
                            }
                            else
                            {
                                again = false;
                            }
                        }
                        break;
                    case 3:
                        while (!handleDepositCash()) ;
                        break;
                    case 4:
                        Console.WriteLine("--Current balance--");
                        Console.WriteLine($"Account id:{context.CurrentCustomer.id}");
                        Console.WriteLine($"Date: {DateTime.Today}");
                        Console.WriteLine($"Balance : {context.CurrentCustomer.balance}");
                        break;
                    default:
                        brain.performExitOperations();
                        return;
                }

            }


        }

        private Boolean handleWithdrawCashMenu()
        {
            int choice = menu.withdrawCashMenu();
            int amount;
            if (choice == 1)
            {
                amount = menu.presentFastCashMenu();
            }
            else
            {
                amount = menu.presentNormalCashMenu();
            }
            Boolean success = brain.withdrawCash(amount,false);
            if (context.HasError)
            {
                Console.WriteLine(context.Error);
                context.clearErrors();
                return false;
            }
            else
            {
                while(true)
                {
                    cl("Do you wish to Print a reciept (Y/N)");
                    String input = Console.ReadLine().ToLower();
                    if(input=="y")
                    {
                        Console.WriteLine("--reciept--");
                        Console.WriteLine($"Account id : {context.CurrentCustomer.id}");
                        Console.WriteLine($"Date: {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].date}");
                        Console.WriteLine($"Amount Withdrawn : {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].amountAddedOrSubtracted}");
                        Console.WriteLine($"Balance : {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].balance}");
                        break;
                    }
                    else if(input=="n")
                    {
                        break;
                    }
                    else
                    {
                        cl("Bad input: please try again");
                    }
                }
                

                
            }
            return success;

        }



        private Boolean handleDepositCash()
        {
            int cash = menu.presentDepositCashMenu();
            Boolean success= brain.depositCash(cash,null);
            if(!success)
            {
                if(context.HasError)
                {
                    cl(context.Error);
                    context.clearErrors();
                    return false;
                }
            }

            cl("Do you wish to print reciept (Y/N)");
            while (true)
            {
                string input = Console.ReadLine().ToLower();
                if (input == "y")
                {
                    Console.WriteLine("--reciept--");
                    Console.WriteLine($"Account id : {context.CurrentCustomer.id}");
                    Console.WriteLine($"Date: {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].date}");
                    Console.WriteLine($"Amount Deposited : {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].amountAddedOrSubtracted}");
                    Console.WriteLine($"Balance : {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].balance}");
                    break;
                }
                else if (input == "n")
                {
                    break;
                }
                else
                {
                    cl("bad input try again...");
                }
            }

            return true;

        }


        private Boolean handleMoneyTransfer(Dictionary<String,int> values)
        {
            Customer c = brain.getInfoOfCustomerByID(values["ID"]);
            if(c==null)
            {
                context.Error = "No account with this ID found";
                return false;
                     
            }
            Console.WriteLine($"You want to transfer {values["money"]} to account holder {c.name}. If you are sure about this please reneter the Account ID");
            int accountID = -1;
            Boolean repeat = true;
            while(repeat)
            {
                try
                {
                    accountID = int.Parse(Console.ReadLine());
                    if(accountID!=values["ID"])
                    {
                        cl("Different id entered: Looks like you dont want to trasfer. ok!");
                        return true;
                    }
                    else
                    {
                        repeat = false;
                    }
                }
                catch (Exception e)
                {
                    
                    Console.WriteLine("Please enter a valid value");
                }
            }

            Boolean success = brain.withdrawCash(values["money"],true);
            if(!success)
            {
            
                return false;
            }

            success=brain.depositCash(values["money"], values["ID"]);
            if(!success)
            {
                return false;
            }

            cl("Do you wish to print reciept: (Y/N):");
            while(true)
            {
                string input = Console.ReadLine().ToLower();
                if(input=="y")
                {
                    Console.WriteLine("--reciept--");
                    Console.WriteLine($"Account id : {context.CurrentCustomer.id}");
                    Console.WriteLine($"Date: {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].date}");
                    Console.WriteLine($"Amount Transferred : {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].amountAddedOrSubtracted}");
                    Console.WriteLine($"Balance : {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].balance}");
                    break;
                }
                else if(input=="n")
                {
                    break;
                }
                else
                {
                    cl("bad input try again...");
                }
            }

            return true;

            
        }

        //admin

        public void handleAdminMenu()
        {
            Boolean repeat = true;
            while (repeat)
            {
                int choose = menu.presentAdminMenu();
                switch (choose)
                {
                    case 1:
                        Customer c = menu.getNewCustomerInfo();
                        Boolean success = brain.addUser(c);
                        if (!success)
                        {
                            cl(context.Error);
                            context.clearErrors();
                            context.RootMode = false;
                            context.CurrentCustomer = null;
                            context.SomeoneLoggedIn = false;
                            cl("Logged out");
                            brain.performExitOperations();
                            cl("--Exit--");
                            return;
                        }
                        break;
                    case 2:
                        success = handleDelExistingAccount();
                        if (!success)
                        {
                            Console.WriteLine(context.Error);

                            context.clearErrors();
                        }
                        
                        break;
                    case 3:
                        while(true)
                        {
                            int id = menu.getCustomerID();
                            c = brain.getInfoOfCustomerByID(id);
                            if(context.HasError)
                            {
                                cl(context.Error);
                                context.clearErrors();

                            }
                            else
                            {
                                c = menu.getNewCustomerInfoToUpdate(id);
                                brain.updateCustomer(c);
                                break;
                            }
                            
                        }
                        break;
                    case 4:
                        menu.displaySearchedCustomers(getSearchForCustomerInfo());
                        break;
                    case 5:
                        while(true)
                        {
                            int n = menu.presentViewReportsByMenu();
                            if (n == 1)
                            {
                                Dictionary<String, int> values = menu.getPriceRangeForSearch();
                                if (!handleAmountRangeReport(values))
                                {
                                    Console.WriteLine("Try again");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Dictionary<String, String> values = menu.getDatesrangeForSearch();
                                handleDateRangeReport(values);

                            }
                        }
                        break;

                    default:
                        brain.performExitOperations();
                        return;
                }

            }
        }

        public void handleDateRangeReport(Dictionary<String,String> values)
        {
            List<Customer.Reciept> reciepts = brain.getCustomerRecieptsInRange(values["startDate"],values["endDate"]);

        }

        public List<Customer> getSearchForCustomerInfo()
        {
            Console.WriteLine("Account id:");
            int? id;
            try
            {
                id = int.Parse(Console.ReadLine());
            } catch(Exception e)
            {
                id = null;
               
            }
            
            Console.WriteLine("User name");
            string username = Console.ReadLine();
            Console.WriteLine("account holders name");
            string name = Console.ReadLine();
            string type = "";
            do
            {
                Console.WriteLine("Type: Savings / Current ");
                type = Console.ReadLine().ToLower();
            }
            while (type != "" && (type != "savings" && type != "current"));

            Customer.AccountType t;
            if(type=="savings")
            {
                t = Customer.AccountType.Savings;
            }
            else if (type == "current")
            {
                t = Customer.AccountType.Current;
            }
            else
            {
                t = Customer.AccountType.None;
            }

            Console.WriteLine("Balance:");
            int? balance;
            try
            {
                balance = int.Parse(Console.ReadLine());
            }catch(Exception e)
            {
                
                balance = null;
            }
            string statusStr = "";
            do
            {
                Console.WriteLine("Status: Active/Disabled");
                statusStr = Console.ReadLine().ToLower();
            }
            while (statusStr != "" && (statusStr != "active" && statusStr != "disabled"));

            Customer.Status status;
            if (statusStr == "active")
            {
                status = Customer.Status.Active;
            }else if(statusStr=="disabled")
            {
                status = Customer.Status.Disabled;
            }
            else
            {
                status = Customer.Status.None;
            }
            return brain.findBy(id, username, name, t, balance, status);
        }


        public Boolean handleDelExistingAccount()
        {
            Console.WriteLine("Enter the account id of account you want to delete");
            int id = -1;
            while(id==-1)
            {
                try
                {
                    id = int.Parse(Console.ReadLine());
                    if(id>=0)
                    {
                        break;
                    }
                    cl("Wrong range: try again");
                    id = -1;
                }
                catch
                {
                    cl("bad input: Try again");
                }
            }
            
            Customer cstmr = context.AllCustomers.Find(c => c.id == id);
            if (cstmr != null)
            {
                
                string input = "";
                while(input=="" || (input!="y" && input!="n"))
                {
                    Console.WriteLine($"You sure you wana remove {cstmr.name}");
                    input = Console.ReadLine().ToLower();
                }

                if(input=="y")
                {
                    brain.deleteCustomer(id);
                    cl("Successfully deleted");
                }
                else
                {
                    cl("Customer not deleted");
                }
                
                return true;
            }
            else
            {
                cl("No customer with this id found");
            }

            return false;
        }

        public Boolean handleAmountRangeReport(Dictionary<String,int> values)
        {
            try
            {
                List<Customer> cstmrs=brain.GetCustomersInBalanceRange(values);
                if(cstmrs==null || cstmrs.Count==0)
                {
                    Console.WriteLine("No record found");

                }
                else
                {
                    menu.displaySearchedCustomers(cstmrs);
                }
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }



    }


}
