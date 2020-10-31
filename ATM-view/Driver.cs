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


        public Driver()
        {
            brain = new ATMBrain();
            context = ApplicationContext.getContext();
            menu = new Menu();
        }


        public void runApplication()
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
                        Console.WriteLine(context.CurrentCustomer.balance);
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
            Boolean success = brain.withdrawCash(amount);
            if (context.HasError)
            {
                Console.WriteLine(context.Error);
                context.clearErrors();
            }
            else
            {
                Console.WriteLine("--reciept--");
                Console.WriteLine($"Account id : {context.CurrentCustomer.id}");
                Console.WriteLine($"Date: {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].date}");
                Console.WriteLine($"Amount withdrew: {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].amountAddedOrSubtracted}");
                Console.WriteLine($"Balance : {context.CurrentCustomer.reciepts[context.CurrentCustomer.reciepts.Count - 1].balance}");
            }
            return success;

        }



        private Boolean handleDepositCash()
        {
            int cash = menu.presentDepositCashMenu();
            return brain.depositCash(cash,null);
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

            Boolean success = brain.withdrawCash(accountID);
            if(!success)
            {
                return false;
            }

            return brain.depositCash(values["money"], values["ID"]);

            
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
                        Customer c = menu.getNewCustomerInfo(-1);
                        Boolean success = brain.addUser(c);
                        if (!success)
                        {
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

                        int id;
                        do
                        {
                            id = menu.getCustomerID();
                            c = brain.getInfoOfCustomerByID(id);
                        } while (context.HasError);
                        c = menu.getNewCustomerInfo(id);
                        brain.updateCustomer(c);
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
                Console.WriteLine(e.Message);
            }
            
            Console.WriteLine("User name");
            string username = Console.ReadLine();
            Console.WriteLine("account holders name");
            string name = Console.ReadLine();
            Console.WriteLine("Type savings/current");
            String typeStr = Console.ReadLine().ToLower();
            Customer.AccountType type;
            if(typeStr=="savings")
            {
                type = Customer.AccountType.Savings;
            }
            else if (typeStr == "current")
            {
                type = Customer.AccountType.Current;
            }
            else
            {
                type = Customer.AccountType.None;
            }

            Console.WriteLine("Balance:");
            int? balance;
            try
            {
                balance = int.Parse(Console.ReadLine());
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                balance = null;
            }
            
            Console.WriteLine("Status");
            String statusStr = Console.ReadLine().ToLower();
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
            return brain.findBy(id, username, name, type, balance, status);
        }


        public Boolean handleDelExistingAccount()
        {
            Console.WriteLine("Enter the account id of account you want to delete");
            int id = int.Parse(Console.ReadLine());
            Customer cstmr = context.AllCustomers.Find(c => c.id == id);
            if (cstmr != null)
            {
                Console.WriteLine($"You sure you wana remove {cstmr.name}");
                if (Console.ReadLine().ToLower() == "y")
                {
                    brain.deleteCustomer(id);
                }
                return true;
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
