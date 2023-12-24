using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBoops.Database.Tables;

namespace TheBoops.Database.DbHandlers
{
    /// <summary>
    /// Handles Database access
    /// </summary>
    public class DbHandler : IDbHandler
    {
        private readonly DynamoDBContext _context;
        public DbHandler(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }
        public async Task<object> GetDataFromTable(string tableName, string isAdmin = Global.Constants.isAdmin, string searchvalue = "")
        {
            object returnData = null;
            try
            {
                //Empty table sent
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentNullException(nameof(tableName));

                switch (tableName)
                {
                    case "Users":
                        returnData = await _context.LoadAsync<UsersDb>($"{isAdmin}", $"{searchvalue}");
                        break;
                    case "Test":
                        returnData = await _context.LoadAsync<UsersDb>( $"{searchvalue}");
                        break;
                    default:
                        break;
                };
                return returnData;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                return null;
            }
        }
        public async Task<bool>SetDataInTable(string tableName, string isAdmin = Global.Constants.isAdmin, object DbObject = null)
        {
            bool returnData = false;
            try
            {
                //Empty table sent
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentNullException(nameof(tableName));
                if(DbObject == null)
                    throw new ArgumentNullException(nameof(DbObject));

                switch (tableName)
                {
                    case "Users":
                        UsersDb user = (UsersDb)DbObject;
                        //Simplified from  - await _context.SaveAsync<UsersDb>(user);
                        await _context.SaveAsync(user);
                        returnData = true;
                        break;
                    default:
                        break;
                };
                return returnData;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

            return returnData;
        }
    }
}
