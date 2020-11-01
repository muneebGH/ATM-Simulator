using System;
namespace ATM_BLL
{
    public class Util
    {
        private Util()
        {
        }

        public static string encryptDecryptPassword(string pass)
        {
            var charArray = pass.ToCharArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];

                if (c >= 'a' && c <= 'z')
                {
                    charArray[i] = (char)(96 + (123 - c));
                }

                if (c >= 'A' && c <= 'Z')
                {
                    charArray[i] = (char)(64 + (91 - c));
                }

                if (c >= '0' && c <= '9')
                {
                    charArray[i] = (char)(48 + (57 - c));
                }
            }

            return new String(charArray);
        }


    }

}
