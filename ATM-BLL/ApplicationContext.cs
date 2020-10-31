using System;
using System.Collections.Generic;
using ATM_BO;
using ATM_DAL;
namespace ATM_BLL
{

    public class ApplicationContext
    {
        private Boolean _someoneLoggedIn;
        private Customer _loggedInCustomer;
        private Boolean _hasError;
        private String _error="";
        private int _cashWithdrawnInSession = 0;
        private List<Customer> _customers;
        private ATMData _data;
        private Boolean _rootMode = false;


        public Boolean RootMode
        {
            get
            {
                return _rootMode;
            }

            set
            {
                _rootMode = value;
            }
        }
        public ATMData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                AllCustomers = value.customerList;
            }
        }

        public Boolean SomeoneLoggedIn
        {
            get
            {
                return _someoneLoggedIn;
            }
            set
            {
                _someoneLoggedIn = value;
                if (_someoneLoggedIn)
                {
                    CustomerLoggedIn(_loggedInCustomer);
                }
                else
                {
                    CustomerLoggedOut(_loggedInCustomer);
                }
                RootMode = false;

            }
        }

        public int CashWithdrawnInSession
        {
            get
            {
                return _cashWithdrawnInSession;
            }
            set
            {
                if (_cashWithdrawnInSession + value > 20000)
                {
                    Error = "cash for today increased than limit";
                }
                _cashWithdrawnInSession = value;
            }
        }
        public Customer CurrentCustomer
        {
            get
            {
                return _loggedInCustomer;
            }

            set
            {
                _loggedInCustomer = value;
                if(_loggedInCustomer!=null)
                {
                    _someoneLoggedIn = true;
                }
                else
                {
                    _someoneLoggedIn = false;
                }
                CustomerObjChanged(_loggedInCustomer);
            }
        }

        public Boolean HasError
        {
            get
            {
                return _hasError;
            }
            set
            {
                _hasError = value;
                if (_hasError)
                {
                    SomeErrorRecieved(Error);
                }
                else
                {
                    Error = "";
                }
            }
        }

        public String Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                if (value != "")
                {
                    HasError = true;
                }
                SomeErrorRecieved(_error);
            }
        }

        public List<Customer> AllCustomers
        {
            get
            {
                return _customers;
            }
            set
            {
                _customers = value;
            }
        }

        private ApplicationContext()
        {
            _someoneLoggedIn = false;
            _error = "";
            _hasError = false;
       
        }

        public void clearErrors()
        {
            Error = "";
            HasError = false;
        }
        private static ApplicationContext context = new ApplicationContext();

        public static ApplicationContext getContext()
        {
            return context;
        }

        public event Action<Customer> CustomerLoggedIn;
        public event Action<Customer> CustomerLoggedOut;
        public event Action<Customer> CustomerObjChanged;
        public event Action<String> SomeErrorRecieved;


    }


}
