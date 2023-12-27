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
        //public async static Task<bool> RunTestSet(IDbHandler _DbHandler)
        //{
        //    bool result = true;
        //    //string _hashValue;

        //    //UsersDb test0 = (UsersDb)await _DbHandler.GetDataFromTable("Users", Constants.isAdmin, "Dhuel");
        //    //GlobalControl.setHash(test0.Password);
        //    //_hashValue = GlobalControl.getHash();
        //    //result = result && test0 != null;

        //    ////Insert a random user
        //    //UsersDb user = new UsersDb();
        //    //user.IsAdmin = "Admin";
        //    //user.UserName = GlobalControl.GetHash(8);
        //    //user.Password = GlobalControl.GetHash(16);
        //    //var test1 = await _DbHandler.SetDataInTable("Users", Global.Constants.isAdmin, user);
        //    //result = result && test1;

        //    //RoomsDb room = new RoomsDb
        //    //{
        //    //    RoomID = "0",
        //    //    RoomName = "Bedroom"
        //    //};
        //    //var test2 = await _DbHandler.SetDataInTable("Rooms", Constants.isAdmin, room);
        //    //result = result && test2;

        //    //Test to get all data in table
        //    //List<UsersDb> test3 = (List<UsersDb>)await _DbHandler.GetDataFromTable("Users", Constants.isAdmin);

        //    return result;
        //}
        
    }
}
