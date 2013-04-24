using System;
using System.Text;

namespace MannusBackup
{
    /// <summary>
    /// Beheer en creer passwords
    /// </summary>
    internal class PasswordGenerator
    {
        private static string Password;
        private Random _random;

        public PasswordGenerator()
        {
            _random = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Genereer nieuw password
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(10));
                builder.Append(RandomNumber(100, 999));
                builder.Append(RandomString(15));
                builder.Append(RandomNumber(1000, 9999));
                Password = builder.ToString();
            }
            return Password;
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}