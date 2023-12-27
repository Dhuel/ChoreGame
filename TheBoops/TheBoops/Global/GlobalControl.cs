using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBoops.Database.DbHandlers;

namespace TheBoops.Global
{
    public static class GlobalControl
    {
        private static string HashCode { get; set; }
        public static void SetHash(string _hashcode)
        {
            HashCode = _hashcode;
        }
        public static string GetHash()
        {
            return HashCode;
        }


        private static IDbHandler DbHandler{ get; set; }
        public static void SetDbHandler(IDbHandler _DbHandler)
        {
            DbHandler = _DbHandler;
        }
        public static IDbHandler GetDbHandler()
        {
            return DbHandler;
        }


        public static string GetHash(int length)
        {
            Random rand = new();
            int stringLength = length;
            string randomString = "";

            for (int i = 0; i < stringLength; i++)
            {
                int randValue = rand.Next(0, 26);
                char letter = Convert.ToChar(randValue + 65);
                randomString += letter;
            }

            return randomString;
        }

    }
}
