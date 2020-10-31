using System;
namespace ATM_BO
{
    public class User
    {
        public int id;
        public String name;
        public String pinCode;
        public String userName;
        public Power power;

        public User(int id,String name,String pinCode,String userName,Power power)
        {
            this.id = id;
            this.name = name;
            this.pinCode = pinCode;
            this.userName = userName;
            this.power = power;
            Console.WriteLine(new Guid());
        }
    }

    public enum Power
    {
       Root,Noob
    }

}
