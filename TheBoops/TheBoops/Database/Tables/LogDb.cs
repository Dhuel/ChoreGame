using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBoops.Database.DbHandlers;
using TheBoops.Global;

namespace TheBoops.Database.Tables
{
    [DynamoDBTable("Logs")]
    public class LogDb
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("TableName")]
        public string TableName { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey]
        [DynamoDBProperty("LogID")]
        public string LogID { get; set; }
        [DynamoDBProperty("UserName")]
        public string UserName { get; set; }
        [DynamoDBProperty("CompletionTime")]
        public string CompletionTime { get; set; }
        [DynamoDBProperty("MissionScore")]
        public int MissionScore { get; set; }
        [DynamoDBProperty("TaskCompleted")]
        public string TaskCompleted { get; set; }
    }
    public class LogsDisplay : LogDb
    {
        public LogsDisplay(LogDb db)
        {
            LogID = db.LogID;
            TableName = db.TableName;
            CompletionTime = db.CompletionTime;
            MissionScore = db.MissionScore;
            
        }

        public LogsDisplay(LogDb db, string i_UserName, string i_TaskCompleted) : this(db)
        {
            LogID = db.LogID;
            TableName = db.TableName;
            CompletionTime = db.CompletionTime;
            MissionScore = db.MissionScore;
            UserName = i_UserName;
            TaskCompleted = i_TaskCompleted;
            LogDisplay = UserName + " completed (" + MissionScore + ")"+ TaskCompleted +" at " + CompletionTime;
        }

        internal static async Task<string> GetUserName(string UserID)
        {
            var _dbHaHandler = GlobalControl.GetDbHandler();
            IEnumerable<UsersDb> _users = (IEnumerable<UsersDb>)await _dbHaHandler.GetTableData(Constants.UsersAWSTable, new() { new SearchQuery("UserID", ScanOperator.Equal, UserID) });
            return _users.FirstOrDefault().UserName;
        }
        internal static async Task<string> GetTaskName(string MissionID)
        {
            var _dbHaHandler = GlobalControl.GetDbHandler();
            IEnumerable<MissionsDb> _mission = (IEnumerable<MissionsDb>)await _dbHaHandler.GetTableData(Constants.MissionsAWSTable, new() { new SearchQuery("MissionID", ScanOperator.Equal, MissionID) });
            return _mission.FirstOrDefault().MissionName; ;
        }
        public string LogDisplay { get; set; }
        public int Points { get; set; }

    }
}
