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
    public class DbHandler : IDbHandler
    {
        private readonly DynamoDBContext _context;
        public DbHandler(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                var user = await _context.LoadAsync<UsersDb>($"Admin", $"{userName}");
                return user is not null;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                return false;
            }

        }

        public async Task<UsersDb> Login(string userName, string password)
        {
            var config = new DynamoDBOperationConfig
            {
                QueryFilter = new List<ScanCondition>
                {
                    new("Password", ScanOperator.Equal, password)
                }
            };

            return await _context.LoadAsync<UsersDb>($"Admin", $"{userName}", config);
        }
    }
}
