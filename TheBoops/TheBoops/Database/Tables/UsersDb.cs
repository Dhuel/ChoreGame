using Amazon.DynamoDBv2.DataModel;

namespace TheBoops.Database.Tables
{
    [DynamoDBTable("Users")]
    public class UsersDb
    {

        [DynamoDBHashKey]
        [DynamoDBProperty("TableName")]
        public string TableName { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey]
        [DynamoDBProperty("UserID")]
        public string UserID { get; set; }
        [DynamoDBProperty("UserName")]
        public string UserName { get; set; }
    }

    public class UsersDisplay : UsersDb
    {
        public UsersDisplay(UsersDb db, int Points = 0)
        {
            this.UserID = db.UserID;
            this.UserName = db.UserName;
            this.TableName = db.TableName;
            this.UserNameDisplay = UserName+" (" + Points + ")";

        }
        public string UserNameDisplay { get; set; }

        internal UsersDb AsDb()
        {
            UsersDb returnval = new()
            {
                UserID = this.UserID,
                UserName = this.UserNameDisplay,
                TableName = this.TableName
            };
            return returnval;
        }
    }
}
