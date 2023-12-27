using Amazon.DynamoDBv2.DataModel;


namespace TheBoops.Database.Tables
{
    [DynamoDBTable("Rooms")]
    public class RoomsDb
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("TableName")]
        public string TableName { get; set; }
        [DynamoDBProperty("RoomID")]
        public string RoomID { get; set; }
        [DynamoDBProperty("RoomName")]
        public string RoomName { get; set; }
    }
}
