using Amazon.DynamoDBv2.DataModel;


namespace TheBoops.Database.Tables
{
    [DynamoDBTable("Users")]
    public class UsersDb
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("isAdmin")]
        public string? IsAdmin { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey]
        [DynamoDBProperty("UserName")]
        public string? UserName { get; set; }
        [DynamoDBProperty("Password")]
        public string? Password { get; set; }
    }
}
