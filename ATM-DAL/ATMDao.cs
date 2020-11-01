using System;
using Newtonsoft.Json;
using ATM_BO;
namespace ATM_DAL
{
    public class ATMDao
    {
        private static String filename = "data.txt";
        public ATMDao()
        {
        }

        public static Boolean saveCustomers(ATMData data)
        {
            try
            {
                string output = JsonConvert.SerializeObject(data);
                System.IO.File.WriteAllText(filename, output);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public static ATMData loadData()
        {
            try
            {
                String output = System.IO.File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<ATMData>(output);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new ATMData();
        }


    }
}
