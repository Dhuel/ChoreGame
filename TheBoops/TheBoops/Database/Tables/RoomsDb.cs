using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBoops.Database.Tables
{
    [DynamoDBTable("Rooms")]
    public class RoomsDb
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("RoomID")]
        public string? RoomID { get; set; }
        [DynamoDBProperty("RoomName")]
        public string? RoomName { get; set; }
    }
}
