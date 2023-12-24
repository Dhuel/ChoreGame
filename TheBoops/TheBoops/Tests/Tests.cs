using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBoops.Database.DbHandlers;
using TheBoops.Database.Tables;
using TheBoops.Global;

namespace TheBoops.Tests
{
    public static class Tests
    {
        /// <summary>
        /// Runs group of tests to test db Access
        /// </summary>
        /// <param name="_DbHandler"></param>
        public async static Task<bool> RunTestSet(IDbHandler _DbHandler)
        {
            bool result = true;
            string _hashValue;

           // Check if user exists
            UsersDb test0 = (UsersDb)await _DbHandler.GetDataFromTable("Users", Constants.isAdmin, "Dhuel");
            GlobalControl.setHash(test0.Password);
            _hashValue = GlobalControl.getHash();
            result = result && test0 != null;

            ////Insert a random user
            //UsersDb user = new UsersDb();
            //user.IsAdmin = "Admin";
            //user.UserName = GetRandomValue(8);
            //user.Password = GetRandomValue(16);
            //var test1 = await _DbHandler.SetDataInTable("Users", Global.Constants.isAdmin, user);
            //result = result && test1;


            var test2 = await _DbHandler.GetDataFromTable("Test", Constants.isAdmin, "Dhuel");
            result = result && test2 != null;
            return result;
        }
        public static string GetRandomValue(int length)
        {
            Random rand = new Random();
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
