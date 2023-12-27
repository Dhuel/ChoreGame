using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBoops.Database.Tables
{
    [DynamoDBTable("Missions")]
    public class MissionsDb
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("TableName")]
        public string TableName { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey]
        [DynamoDBProperty("MissionID")]
        public string MissionID { get; set; }
        [DynamoDBProperty("RoomID")]
        public string RoomID { get; set; }
        [DynamoDBProperty("MissionName")]
        public string MissionName { get; set; }
        [DynamoDBProperty("MissionScore")]
        public int MissionScore { get; set; }
    }
}
