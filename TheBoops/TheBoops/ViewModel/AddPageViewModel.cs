using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TheBoops.Database.DbHandlers;
using TheBoops.Database.Tables;
using TheBoops.Global;
using TheBoops.Services;

namespace TheBoops.ViewModel
{
    public class AddPageViewModel: INotifyPropertyChanged
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
        string PageName { get; set; }
        #endregion
        #region Commands
        public ICommand SaveRecordButton { get; }
        #endregion
        #region Page Variables
        public string Title { get; set; }
        public Entry UserNameEntry { get; set; }
        public Entry RoomNameEntry { get; set; }
        public Entry MissionNameEntry { get; set; }
        public Entry MissionPointsEntry { get; set; }
        RoomsDb Room { get; set; }

        #endregion
        #region Constructor
        public AddPageViewModel(string pageName, RoomsDb i_Room = null)
        {
            PageName = pageName ?? string.Empty;
            Room = i_Room ?? new RoomsDb();
            SaveRecordButton = new Command(async () => await SaveRecord());
        }

        #endregion
        #region Function Calls
        private async Task SaveRecord()
        {
            bool RecordSaved = false;
            _dbHaHandler = GlobalControl.GetDbHandler();
            switch (PageName)
            {
                case Constants.UsersPage:
                    RecordSaved = await _dbHaHandler.AddRecordToDb(Constants.UsersAWSTable, UserNameEntry.Text);
                    break;
                case Constants.RoomsPage:
                    RecordSaved = await _dbHaHandler.AddRecordToDb(Constants.RoomsAWSTable, RoomNameEntry.Text);
                    break;
                case Constants.MissionsPage:
                    RecordSaved = await _dbHaHandler.AddRecordToDb(Constants.MissionsAWSTable, MissionNameEntry.Text, MissionPointsEntry.Text, Room.RoomID);
                    break;
                default:
                    break;
            }
            if (RecordSaved)
                await NavigationHandler.PopPage();
        }
         public void SetUserValues(Entry i_UserNameEntry)
        {
           UserNameEntry = i_UserNameEntry;
        }
        public void SetRoomValues(Entry i_RoomNameEntry)
        {
            RoomNameEntry = i_RoomNameEntry;
        }
        public void SetMissionValues(Entry i_MissionNameEntry, Entry i_MissionPointsEntry)
        {
            MissionNameEntry = i_MissionNameEntry;
            MissionPointsEntry = i_MissionPointsEntry;
        }

        #endregion
    }
}
