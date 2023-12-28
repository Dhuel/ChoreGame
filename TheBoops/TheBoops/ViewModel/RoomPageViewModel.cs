using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TheBoops.Database.DbHandlers;
using TheBoops.Database.Tables;
using TheBoops.Global;
using TheBoops.Services;

namespace TheBoops.ViewModel
{
     
    public class RoomPageViewModel : INotifyPropertyChanged
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
        private IEnumerable<RoomsDb> Rooms_ { get; set; }
        public IEnumerable<RoomsDb> Rooms
        {
            get
            {
                return Rooms_;
            }
            set
            {
                Rooms_ = value;
                NotifyPropertyChanged();
            }
        }
        private RoomsDb Room_ { get; set; }
        public RoomsDb Room
            {
                get { return Room_; }
                set
                {
                    Room_ = value;
                    RoomSelected(Room_);
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
        #endregion
        #region Constructor
        public RoomPageViewModel()
        {
            Title = "Rooms";
            AddButtonClicked = new Command(async () => await RoomPageViewModel.AddPage(Title));
        }
        #endregion
        #region Function Calls
        public async void LoadPageDataAsync()
        {
            _dbHaHandler = GlobalControl.GetDbHandler();
            Rooms = ((IEnumerable<RoomsDb>)await _dbHaHandler.GetTableData(Constants.RoomsAWSTable)).OrderBy(c=>c.RoomName);
        }
        private static async void RoomSelected(RoomsDb room)
        {
            await NavigationHandler.NavToMissions(room);
        }
        private static async Task AddPage(string title)
        {
            await NavigationHandler.NavToAddPage(title);
        }
        #endregion
    }
}
