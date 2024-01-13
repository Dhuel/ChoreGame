using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Linq;
using TheBoops.Database.DbHandlers;
using TheBoops.Database.Tables;
using TheBoops.Global;
using TheBoops.Services;

namespace TheBoops.ViewModel
{
    public class MissionsPageViewModel : INotifyPropertyChanged
    {
        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Global Variables
        IDbHandler _dbHaHandler;
        #endregion
        #region Commands
        public ICommand AddButtonClicked { get; }
        #endregion
        #region Page Variables
        public string Title { get; set; }
        private IEnumerable<MissionDisplay> Missions_ { get; set; }
        public IEnumerable<MissionDisplay> Missions
        {
            get
            {
                return Missions_;
            }
            set
            {
                Missions_ = value;
                NotifyPropertyChanged();
            }
        }
        private MissionDisplay Mission_ { get; set; }
        public MissionDisplay Mission
        {
            get { return Mission_; }
            set
            {
                Mission_ = value;
                _ = MissionSelected(Mission_);
                NotifyPropertyChanged();
            }
        }
        public string AddText
        {
            get
            {
                return string.Concat("Add ", Title.AsSpan(0, Title.Length - 1));
            }
        }
        private RoomsDb RoomDB {  get; set; }  

        private DateTime CompletionDate_ { get; set; }
        public DateTime CompletionDate
        {
            get { return CompletionDate_; }
            set
            {
                CompletionDate_ = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
        #region Constructor
        public MissionsPageViewModel(RoomsDb i_room)
        {
            RoomDB = i_room;
            Title = Constants.MissionsPage;
            AddButtonClicked = new Command(async () => await NavigationHandler.NavToAddPage(Title, RoomDB));
        }
        #endregion
        #region Function Calls
        public async void LoadPageData(RoomsDb i_room)
        {
            CompletionDate = DateTime.Now;
            RoomDB = i_room;
            _dbHaHandler = GlobalControl.GetDbHandler();
            Missions = new List<MissionDisplay>();

            IEnumerable<MissionsDb> Missions_ = ((IEnumerable<MissionsDb>)await _dbHaHandler.GetTableData(Constants.MissionsAWSTable, new() { new SearchQuery("RoomID", ScanOperator.Equal, RoomDB.RoomID) })).OrderBy(c => c.MissionName); ;
            foreach (MissionsDb mission in Missions_)
            {
                List<MissionDisplay> _Missions = Missions.ToList();
                _Missions.Add(new MissionDisplay(mission));
                Missions = _Missions;
            }
        }
        private async Task<bool> MissionSelected(MissionsDb mission)
        {
            bool returnvalue;
            PointsDb points = new()
            {
                MissionID = mission.MissionID,
                PointValue = mission.MissionScore,
                TableName = Constants.PointsAWSTable,
                UserID = GlobalControl.GetHash(),
                PointsID = GlobalControl.GetHash(12),
                CompletionDate = CompletionDate.ToString("MM/dd/yyyy")
            };
            returnvalue = await _dbHaHandler.AddRecordToDb(Constants.PointsAWSTable, points);


            LogDb log = new()
            {
                LogID = GlobalControl.GetHash(12),
                MissionScore = mission.MissionScore,
                TableName = Constants.LogsAWSTable,
                UserName = GlobalControl.GetHash(),
                CompletionTime = CompletionDate.ToString("MM/dd/yyyy"),
                TaskCompleted = mission.MissionID
            };
            await _dbHaHandler.SaveTableData("Logs", log);
            await NavigationHandler.DisplayAlert("Mission complete!");
            await NavigationHandler.PopToRoot();
            return returnvalue;
        }

        #endregion
    }
}
