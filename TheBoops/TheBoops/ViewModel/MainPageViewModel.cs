using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using TheBoops.Database.DbHandlers;
using TheBoops.Database.Tables;
using TheBoops.Global;

namespace TheBoops.ViewModel
{
    public class MainPageViewModel
    {
        private readonly IDbHandler _DbHandler;
        public ICommand ButtonCommand { get; }
        public MainPageViewModel(IDbHandler DbHandler)
        {
            ButtonCommand = new Command(async () => await LoadDB());
            _DbHandler = DbHandler;
            LoadGLobals();
        }

        private async Task<bool> LoadDB()
        {
            //Loading personal data into table
            bool returnvalue = false;
            try
            {
                List<string> Rooms = new List<string>() { "Bed/Bath","Living","Kitchen","Dining", "Laundry","Upstairs","Miscellaneous"};
                Dictionary<string, string> BedBathMissions = new()
                {
                    { "Clean Bathroom", "1" },
                    { "Clean master bedroom", "2" },
                    { "Take out master bathroom trash", "2" },
                    { "Make the bed", "1" },
                    { "Water plants", "2" },
                    { "Journal", "5" }
                };
                Dictionary<string, string> LivingMissions = new()
                {
                    { "Clean Living room", "2" }
                };
                Dictionary<string, string> KitchenMissions = new()
                {
                    { "Clean fridge", "3" },
                    { "Clean kitchen", "2" },
                    { "Fill ice tray", "2" },
                    { "Grocery shopping", "3" },
                    { "Make dinner", "4" },
                    { "Wash dishes", "4" },
                    { "Take out trash", "2" },
                    { "Water plants", "2" },
                    { "Refill soap", "3" },
                    { "Empty dishwasher", "3" },
                    { "Refill water", "2" },
                    { "Empty dish rack", "3" },
                    { "Errands", "3" },
                    { "Take out recycling", "2" },
                    { "Water sink", "2" }
                };
                Dictionary<string, string> DiningMissions = new()
                {
                    { "Feed the cat", "2" },
                    { "Clean dining room table", "2" },
                    { "Clean guest bathroom", "1" },
                    { "Clean downstairs Roomba", "1" },
                    { "Take trash outside", "2" }
                };
                Dictionary<string, string> LaundryMissions = new()
                {
                    { "Laundry", "3" },
                    { "Clean downstairs litter", "4" }
                };
                Dictionary<string, string> UpstairsMissions = new()
                {
                    { "Clean office", "2" },
                    { "Clean upstairs roomba", "2" },
                    { "Clean upstairs bathroom", "1" },
                    { "Clean upstairs litter", "4" },
                    { "Vacuum stairs", "3" }
                };
                Dictionary<string, string> MiscMissions = new()
                {
                    { "Organize finances", "1" },
                    { "Take out upstairs trash", "2" }
                };
                foreach (string RoomName in Rooms)
                {
                    //Add Rooms
                    _ = await _DbHandler.AddRecordToDb(Constants.RoomsAWSTable, RoomName);
                    var _Room = (IEnumerable<RoomsDb>)await _DbHandler.GetTableData(Constants.RoomsAWSTable, new() { new SearchQuery() { Field = "RoomName", Operator = ScanOperator.Equal, FieldValue = RoomName } });
                    if (_Room.Count() > 0)
                    {
                        //Add Missions To Room
                        if (RoomName == "Bed/Bath")
                        {
                            foreach (KeyValuePair<string, string> kvp in BedBathMissions)
                            {
                                _ = await _DbHandler.AddRecordToDb(Constants.MissionsAWSTable, kvp.Key, kvp.Value, _Room.First().RoomID);
                            }
                        }
                        else if (RoomName == "Living")
                        {
                            foreach (KeyValuePair<string, string> kvp in LivingMissions)
                            {
                                _ = await _DbHandler.AddRecordToDb(Constants.MissionsAWSTable, kvp.Key, kvp.Value, _Room.First().RoomID);
                            }
                        }
                        else if (RoomName == "Kitchen")
                        {
                            foreach (KeyValuePair<string, string> kvp in KitchenMissions)
                            {
                                _ = await _DbHandler.AddRecordToDb(Constants.MissionsAWSTable, kvp.Key, kvp.Value, _Room.First().RoomID);
                            }
                        }
                        else if (RoomName == "Dining")
                        {
                            foreach (KeyValuePair<string, string> kvp in DiningMissions)
                            {
                                _ = await _DbHandler.AddRecordToDb(Constants.MissionsAWSTable, kvp.Key, kvp.Value, _Room.First().RoomID);
                            }
                        }
                        else if (RoomName == "Laundry")
                        {
                            foreach (KeyValuePair<string, string> kvp in LaundryMissions)
                            {
                                _ = await _DbHandler.AddRecordToDb(Constants.MissionsAWSTable, kvp.Key, kvp.Value, _Room.First().RoomID);
                            }
                        }
                        else if (RoomName == "Upstairs")
                        {
                            foreach (KeyValuePair<string, string> kvp in UpstairsMissions)
                            {
                                _ = await _DbHandler.AddRecordToDb(Constants.MissionsAWSTable, kvp.Key, kvp.Value, _Room.First().RoomID);
                            }
                        }
                        else if (RoomName == "Miscellaneous")
                        {
                            foreach (KeyValuePair<string, string> kvp in MiscMissions)
                            {
                                _ = await _DbHandler.AddRecordToDb(Constants.MissionsAWSTable, kvp.Key, kvp.Value, _Room.First().RoomID);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //TODO - display Exception
            }
            return returnvalue;
        }

        public void LoadGLobals()
        {
            GlobalControl.SetDbHandler(_DbHandler);
        }
    }
}
