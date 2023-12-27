using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Diagnostics;
using TheBoops.Database.Tables;
using TheBoops.Global;

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

        public async Task<object> GetTableData(string tableName, List<SearchQuery> searchQueries = null)
        {
            object returnData = null;
            try
            {
                switch (tableName)
                {
                    case Constants.UsersAWSTable:
                        if (searchQueries == null)
                            returnData = await _context.QueryAsync<UsersDb>($"{tableName}").GetRemainingAsync();
                        else
                        {
                            List<ScanCondition> conditions = new();
                            foreach (SearchQuery query in searchQueries)
                            {
                                conditions.Add(new ScanCondition(query.Field, query.Operator, query.FieldValue));
                            }
                            returnData = await _context.ScanAsync<UsersDb>(conditions).GetRemainingAsync();
                        }
                        break;
                    case Constants.RoomsAWSTable:
                        if (searchQueries == null)
                            returnData = await _context.QueryAsync<RoomsDb>($"{tableName}").GetRemainingAsync();
                        else
                        {
                            List<ScanCondition> conditions = new();
                            foreach (SearchQuery query in searchQueries)
                            {
                                conditions.Add(new ScanCondition(query.Field, query.Operator, query.FieldValue));
                            }
                            returnData = await _context.ScanAsync<RoomsDb>(conditions).GetRemainingAsync();
                        }
                        break;
                    case Constants.MissionsAWSTable:
                        if (searchQueries == null)
                            returnData = await _context.QueryAsync<MissionsDb>($"{tableName}").GetRemainingAsync();
                        else
                        {
                            List<ScanCondition> conditions = new();
                            foreach (SearchQuery query in searchQueries)
                            {
                                conditions.Add(new ScanCondition(query.Field, query.Operator, query.FieldValue));
                            }
                            returnData = await _context.ScanAsync<MissionsDb>(conditions).GetRemainingAsync();
                        }
                        break;
                    case Constants.PointsAWSTable:
                        if (searchQueries == null)
                            returnData = await _context.QueryAsync<PointsDb>($"{tableName}").GetRemainingAsync();
                        else
                        {
                            List<ScanCondition> conditions = new();
                            foreach (SearchQuery query in searchQueries)
                            {
                                conditions.Add(new ScanCondition(query.Field, query.Operator, query.FieldValue));
                            }
                            returnData = await _context.ScanAsync<PointsDb>(conditions).GetRemainingAsync();
                        }
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
        public async Task<bool>SaveTableData(string tableName, object DbObject = null)
        {
            //TODO - update this to take AWS values
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
                    case Constants.UsersPage:
                        UsersDb user = (UsersDb)DbObject;
                        await _context.SaveAsync(user);
                        returnData = true;
                        break;
                    case Constants.RoomsPage:
                        RoomsDb room = (RoomsDb)DbObject;
                        await _context.SaveAsync(room);
                        returnData = true;
                        break;
                    case Constants.MissionsPage:
                        MissionsDb misison = (MissionsDb)DbObject;
                        await _context.SaveAsync(misison);
                        returnData = true;
                        break;
                    case Constants.PointsPage:
                        PointsDb points = (PointsDb)DbObject;
                        await _context.SaveAsync(points);
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
        public async Task<bool> AddRecordToDb(string RecordType, string Name, string Value = "", string ID = "")
        {
            bool returnvalue = false;

            if (string.IsNullOrEmpty(Name))
                return false;

            switch (RecordType)
            {
                case Constants.UsersAWSTable:
                    UsersDb _user;
                    IEnumerable<UsersDb> _users = (IEnumerable<UsersDb>)await GetTableData(Constants.UsersAWSTable, new() { new SearchQuery() { Field = "UserName", Operator = ScanOperator.Equal, FieldValue = Name}});
                    if (_users.Count() == 0)
                        _user = CreateUsersDB(Name);
                    else
                        _user = _users.FirstOrDefault();
                    returnvalue = await SaveTableData(Constants.UsersPage, _user);
                    break;
                case Constants.RoomsAWSTable:
                    RoomsDb _room;
                    IEnumerable<RoomsDb>_rooms = (IEnumerable<RoomsDb>)await GetTableData(Constants.RoomsAWSTable, new() { new SearchQuery() { Field = "RoomName", Operator = ScanOperator.Equal, FieldValue = Name } });
                    if (_rooms.Count() == 0)
                        _room = CreateRoomDB(Name);
                    else
                        _room = _rooms.FirstOrDefault();
                    returnvalue = await SaveTableData(Constants.RoomsPage, _room);
                    break;
                case Constants.MissionsAWSTable:
                    MissionsDb _mission;
                    IEnumerable<MissionsDb> _missions = (IEnumerable<MissionsDb>)await GetTableData(Constants.MissionsAWSTable, new() { new SearchQuery() { Field = "MissionName", Operator = ScanOperator.Equal, FieldValue = Name }, new SearchQuery() { Field = "RoomID", Operator = ScanOperator.Equal, FieldValue = ID } });
                    if (_missions.Count() == 0)
                        _mission = CreateMissionDB(Name, Convert.ToInt32(Value), ID);
                    else
                        _mission = _missions.FirstOrDefault();
                    returnvalue = await SaveTableData(Constants.MissionsPage, _mission);
                    break;
                case Constants.PointsAWSTable:
                    PointsDb points = new()
                    {
                        MissionID = Name,
                        TableName = Constants.PointsAWSTable,
                        CompletionDate = DateTime.Now.ToString("MM/dd/yyyy"),
                        UserID = GlobalControl.GetHash(),
                        PointsID = GlobalControl.GetHash(12),
                        PointValue = Convert.ToInt32(Value)
                    };
                    returnvalue = await SaveTableData(Constants.PointsPage, points);
                    break;
            }

            return returnvalue;
        }
        static UsersDb CreateUsersDB(string i_UserName)
        {
            UsersDb _returnUser = new()
            {
                TableName = Constants.UsersAWSTable,
                UserName = i_UserName,
                UserID = GlobalControl.GetHash(12)
            };

            return _returnUser;
        }
        static RoomsDb CreateRoomDB(string i_RoomName)
        {
            RoomsDb _returnRoom = new()
            {
                TableName = Constants.RoomsAWSTable,
                RoomName = i_RoomName,
                RoomID = GlobalControl.GetHash(12)
            };

            return _returnRoom;
        }
        MissionsDb CreateMissionDB(string i_MissionName, int i_MissionValue, string i_RoomID)
        {
            MissionsDb _returnMission = new()
            {
                TableName = Constants.MissionsAWSTable,
                MissionName = i_MissionName,
                MissionScore = i_MissionValue,
                MissionID = GlobalControl.GetHash(12),
                RoomID = i_RoomID
            };

            return _returnMission;
        }
    }
    public class SearchQuery
    {
        public string Field { get; set; }
        public ScanOperator Operator { get; set; }
        public string FieldValue { get; set; }
    }
}
