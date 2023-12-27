using Amazon.DynamoDBv2.DocumentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
        private IEnumerable<MissionsDb> Missions_ { get; set; }
        public IEnumerable<MissionsDb> Missions
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
        private MissionsDb Mission_ { get; set; }
        public MissionsDb Mission
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
            RoomDB = i_room;
            _dbHaHandler = GlobalControl.GetDbHandler();
            Missions = (IEnumerable<MissionsDb>)await _dbHaHandler.GetTableData(Constants.MissionsAWSTable, new() { new SearchQuery() { Field = "RoomID", Operator = ScanOperator.Equal, FieldValue = RoomDB.RoomID } });
        }
        private async Task<bool> MissionSelected(MissionsDb mission)
        {
            bool returnvalue;
            returnvalue = await _dbHaHandler.AddRecordToDb(Constants.PointsAWSTable, mission.MissionID, mission.MissionScore.ToString());
            await NavigationHandler.DisplayAlert("Mission complete!");
            await NavigationHandler.PopToRoot();
            return returnvalue;
        }

        #endregion
    }
}
