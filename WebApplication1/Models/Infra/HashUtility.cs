using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Models.Infra
{
    public static class HashUtility
    {
        public static string ToSHA256(string plainText, string salt)
        {
            using (var mySHA256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(salt + plainText);

                var hash = mySHA256.ComputeHash(passwordBytes);

                var sb = new StringBuilder();
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string GetSalt()
        {
           string randomSalt= GetLetter()+GetNumber()+GetLetter()+GetNumber()+GetLetter();
            return randomSalt;
        }
        private static string GetNumber()
        {
            var random = new Random();
            var number = random.Next(1, 1001).ToString("D4");
            return number;
        }
        private static string GetLetter()
        {
            var random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_=-<>?";
            string randomLetter="";
            for(int i = 0; i < 6; i++)
            {
                char randomchar= characters[random.Next(characters.Length)];
                randomLetter += randomchar;
            }
            return randomLetter;
        }
    }
}
