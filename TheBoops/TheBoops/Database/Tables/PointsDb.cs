using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBoops.Database.Tables
{

    [DynamoDBTable("Points")]
    public class PointsDb
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("UserID")]
        public string? UserID { get; set; }
        [DynamoDBProperty("CompletionDate")]
        public string? CompletionDate { get; set; }

        [DynamoDBProperty("MissionID")]
        public string? MissionID { get; set; }
        

    }
}
