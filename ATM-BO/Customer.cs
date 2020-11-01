using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ATM_BO
{
    public class Customer:User
    {
        public AccountType type;
        public Status status;
        public int balance;
        public List<Reciept> reciepts;

        public enum AccountType
        {
            Savings, Current,None
        };

        public enum Status
        {
            Active, Disabled,None
        };

        public class Reciept
        {
            public int id;
            public String date;
            public int amountAddedOrSubtracted;
            public int balance;
            public RecieptType recieptType;

            public Reciept(int id, String date, int amount, int balance,RecieptType rt)
            {
                this.id = id;
                this.date = date;
                this.amountAddedOrSubtracted = amount;
                this.balance = balance;
                this.recieptType = rt;
            }

            public enum RecieptType
            {
                DEPOSITED,WITHDRAWN,TRANSFERED,NONE
            }

        }
        public Customer(int id, String name, String pinCode, AccountType type, Status status, int balance, String userName,Power power):base(id,name,pinCode,userName,power)
        {
            this.type = type;
            this.status = status;
            this.balance = balance;
            this.reciepts = new List<Reciept>();
        }

        public void addReciept(String d, int amount, int b, Reciept.RecieptType t)
        {
            if (d == null)
            {
                d = DateTime.Now.ToString("dd/MM/yyyy");
            }
            if (b == default(int))
            {
                b = balance;
            }

            reciepts.Add(new Reciept(id, d, amount, b,t));
        }

    }
}
