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
            catch
            {
                Console.WriteLine("File not found to retrieve data");
                Console.WriteLine("If its your first login, Dont worry it will be created once you exit the session");
            }
            return new ATMData();
        }


    }
}
