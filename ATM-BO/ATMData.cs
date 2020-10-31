using System;
using System.Collections.Generic;

namespace ATM_BO
{
    public class ATMData
    {
        public List<Customer> customerList = new List<Customer>();
        public int presentID=0;

        public ATMData()
        {
        }

        public ATMData(List<Customer> list, int id)
        {
            this.customerList = list;
            this.presentID = id;
        }
         
    }
}
