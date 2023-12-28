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
    public class UsersPageViewModel : INotifyPropertyChanged
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
        #region Global variables
        IDbHandler _dbHaHandler;
        #endregion
        #region Commands
        public ICommand AddButtonClicked { get; }
        #endregion
        #region PageVariables
        public string Title { get; set; }

        public string AddText
        {
            get
            {
                return string.Concat("Add ", Title.AsSpan(0, Title.Length - 1));
            }
        }
        private IEnumerable<UsersDisplay> Users_ { get;set; }
        public IEnumerable<UsersDisplay> Users
        {
            get
            {
                return Users_;
            }
            set
            {
                Users_ = value;
                NotifyPropertyChanged();
            }
        }
        private IEnumerable<PointsDb> Points_ { get; set; }
        public IEnumerable<PointsDb> Points
        {
            get
            {
                return Points_;
            }
            set
            {
                Points_ = value;
                NotifyPropertyChanged();
            }
        }
        private UsersDisplay User_ { get; set;}
        public UsersDisplay User {
            get { return User_; }
            set {
                User_ = value;
                UserSelected(User_);
                NotifyPropertyChanged();
                }
        }

        private IEnumerable<UsersDisplay> LastWeekUsers_ { get; set; }
        public IEnumerable<UsersDisplay> LastWeekUsers
        {
            get
            {
                return LastWeekUsers_;
            }
            set
            {
                LastWeekUsers_ = value;
                NotifyPropertyChanged();
            }
        }
        private UsersDisplay Winner_ { get; set; }
        public UsersDisplay Winner
        {
            get { return Winner_; }
            set
            {
                Winner_ = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region Constructors
        public UsersPageViewModel(IDbHandler DbHandler)
        {
            Title = Constants.UsersPage;
            AddButtonClicked = new Command(async () => await  NavigationHandler.NavToAddPage(Title));
            GlobalControl.SetDbHandler(DbHandler);
        }
        #endregion
        #region Function calls
        /// <summary>
        /// Loads user data
        /// </summary>
        public async void LoadPageData()
        {
            _dbHaHandler = GlobalControl.GetDbHandler();
            Users = new List<UsersDisplay>();


            IEnumerable<UsersDb> _Users = (IEnumerable<UsersDb>)await _dbHaHandler.GetTableData(Constants.UsersAWSTable);
            foreach (UsersDb user in _Users)
            {
                //Calculate current points
                int total = 0;
                foreach (PointsDb _point in (List<PointsDb>)await _dbHaHandler.GetTableData(
                    Constants.PointsAWSTable,new()
                    {
                        new SearchQuery("UserID",ScanOperator.Equal, user.UserID),
                        new SearchQuery("CompletionDate",ScanOperator.Between, LastSunday(),NextSunday())
                    })
                    )
                {
                    total += _point.PointValue;
                }
                List<UsersDisplay> _User = Users.ToList();
                _User.Add(new UsersDisplay(user, total));
                Users = _User;

                //calculate last weeks points
                int LastWeeksTotal = 0;
                foreach (PointsDb _point in (List<PointsDb>)await _dbHaHandler.GetTableData(
                    Constants.PointsAWSTable, new()
                    {
                        new SearchQuery("UserID",ScanOperator.Equal, user.UserID),
                        new SearchQuery("CompletionDate",ScanOperator.Between, LastSunday(-1),NextSunday(-1))
                    })
                    )
                {
                    LastWeeksTotal += _point.PointValue;
                }
                if (LastWeekUsers == null)
                    LastWeekUsers = new List<UsersDisplay>();
                List<UsersDisplay> _LastWeekUser = LastWeekUsers.ToList();
                _LastWeekUser.Add(new UsersDisplay(user, LastWeeksTotal));
                LastWeekUsers = _LastWeekUser;
            }
            Winner = LastWeekUsers.OrderByDescending(x => x.Points).First();
        }


        private string NextSunday(int week = 0)
        {
            DateTime checkedDate = DateTime.Now;
            if (week == -1)
                checkedDate = checkedDate.AddDays(-7);
            string returnDate = "";
            switch (checkedDate.DayOfWeek.ToString())
            {
                case "Sunday":
                    returnDate = checkedDate.AddDays(7).ToString("MM/dd/yyyy");
                    break;
                case "Monday":
                    returnDate = (checkedDate.AddDays(6)).ToString("MM/dd/yyyy");
                    break;
                case "Tuesday":
                    returnDate = (checkedDate.AddDays(5)).ToString("MM/dd/yyyy");
                    break;
                case "Wednesday":
                    returnDate = (checkedDate.AddDays(4)).ToString("MM/dd/yyyy");
                    break;
                case "Thursday":
                    returnDate = (checkedDate.AddDays(3)).ToString("MM/dd/yyyy");
                    break;
                case "Friday":
                    returnDate = (checkedDate.AddDays(2)).ToString("MM/dd/yyyy");
                    break;
                case "Saturday":
                    returnDate = (checkedDate.AddDays(1)).ToString("MM/dd/yyyy");
                    break;
            }
            return returnDate;
        }

        private string LastSunday( int week = 0)
        {
            DateTime checkedDate = DateTime.Now;
            if (week == -1)
                checkedDate = checkedDate.AddDays(-7);
            string returnDate = "";
            switch(checkedDate.DayOfWeek.ToString())
            {
                case "Sunday":
                    returnDate = checkedDate.ToString("MM/dd/yyyy");
                    break;
                case "Monday":
                    returnDate = (checkedDate.AddDays(-1)).ToString("MM/dd/yyyy");
                    break;
                case "Tuesday":
                    returnDate = (checkedDate.AddDays(-2)).ToString("MM/dd/yyyy");
                    break;
                case "Wednesday":
                    returnDate = (checkedDate.AddDays(-3)).ToString("MM/dd/yyyy");
                    break;
                case "Thursday":
                    returnDate = (checkedDate.AddDays(-4)).ToString("MM/dd/yyyy");
                    break;
                case "Friday":
                    returnDate = (checkedDate.AddDays(-5)).ToString("MM/dd/yyyy");
                    break;
                case "Saturday":
                    returnDate = (checkedDate.AddDays(-6)).ToString("MM/dd/yyyy");
                    break;
            }
            return returnDate;
        }

        /// <summary>
        /// Selects the clicked user
        /// </summary>
        /// <param name="user"></param>
        private async void UserSelected(UsersDisplay user)
        {
            if (user.UserID == "-")
            {
                user.UserID = GlobalControl.GetHash(12);
                await _dbHaHandler.SaveTableData("Users", user.AsDb());
            }

            GlobalControl.SetHash(user.UserID);
            await NavigationHandler.NavToRooms();
        }
        #endregion
    }
}
