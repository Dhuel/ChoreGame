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

        private IEnumerable<LogsDisplay> Logs_ { get; set; }
        public IEnumerable<LogsDisplay> Logs
        {
            get
            {
                return Logs_;
            }
            set
            {
                Logs_ = value;
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
            Logs = new List<LogsDisplay>();


            IEnumerable<UsersDb> _Users = (IEnumerable<UsersDb>)await _dbHaHandler.GetTableData(Constants.UsersAWSTable);
            foreach (UsersDb user in _Users)
            {
                //Calculate current points
                int total = 0;
                var query = (List<PointsDb>)await _dbHaHandler.GetTableData(
                    Constants.PointsAWSTable, new()
                    {
                        new SearchQuery("UserID",ScanOperator.Equal, user.UserID),
                        new SearchQuery("CompletionDate",ScanOperator.In, UsersPageViewModel.GetDaysInWeek())
                    });
                foreach (PointsDb _point in query)
                {
                    total += _point.PointValue;
                }
                List<UsersDisplay> _User = Users.ToList();
                _User.Add(new UsersDisplay(user, total));
                Users = _User;

                int LastWeeksTotal = 0;
                query = (List<PointsDb>)await _dbHaHandler.GetTableData(
                    Constants.PointsAWSTable, new()
                    {
                        new SearchQuery("UserID",ScanOperator.Equal, user.UserID),
                        new SearchQuery("CompletionDate",ScanOperator.In, UsersPageViewModel.GetDaysInWeek(true))
                    });
                foreach (PointsDb _point in query)
                {
                    LastWeeksTotal += _point.PointValue;
                }
                LastWeekUsers ??= new List<UsersDisplay>();
                List<UsersDisplay> _LastWeekUser = LastWeekUsers.ToList();
                _LastWeekUser.Add(new UsersDisplay(user, LastWeeksTotal));
                LastWeekUsers = _LastWeekUser;
            }
            Winner = LastWeekUsers.OrderByDescending(x => x.Points).First();

            IEnumerable<LogDb> _Logs = (IEnumerable<LogDb>)await _dbHaHandler.GetTableData(Constants.LogsAWSTable);

            foreach(LogDb _log in _Logs.Take(10))
            {
                List<LogsDisplay> Logs_ = Logs.ToList();
                var log = new LogsDisplay(_log, await LogsDisplay.GetUserName(_log.UserName), await LogsDisplay.GetTaskName(_log.TaskCompleted));
                Logs_.Add(log);
                Logs = Logs_;
            }
        }


        private static string[] GetDaysInWeek(bool previousWeek = false)
        {
            //return dates from next sunday? meh
            DateTime checkedDate = DateTime.Now;
            if (previousWeek)
                checkedDate = checkedDate.AddDays(-7);
            List<string> returnDate = new();
            string start_date = "";
            string end_date = "";
            switch (checkedDate.DayOfWeek.ToString())
            {
                case "Sunday":
                    end_date = checkedDate.AddDays(6).ToString("MM/dd/yyyy");
                    start_date = checkedDate.ToString("MM/dd/yyyy");
                    break;
                case "Monday":
                    end_date = checkedDate.AddDays(5).ToString("MM/dd/yyyy");
                    start_date = checkedDate.AddDays(-1).ToString("MM/dd/yyyy");
                    break;
                case "Tuesday":
                    end_date = checkedDate.AddDays(4).ToString("MM/dd/yyyy");
                    start_date = checkedDate.AddDays(-2).ToString("MM/dd/yyyy");
                    break;
                case "Wednesday":
                    end_date = checkedDate.AddDays(3).ToString("MM/dd/yyyy");
                    start_date = checkedDate.AddDays(-3).ToString("MM/dd/yyyy");
                    break;
                case "Thursday":
                    end_date = checkedDate.AddDays(2).ToString("MM/dd/yyyy");
                    start_date = checkedDate.AddDays(-4).ToString("MM/dd/yyyy");
                    break;
                case "Friday":
                    end_date = checkedDate.AddDays(1).ToString("MM/dd/yyyy");
                    start_date = checkedDate.AddDays(-5).ToString("MM/dd/yyyy");
                    break;
                case "Saturday":
                    end_date = checkedDate.ToString("MM/dd/yyyy");
                    start_date = checkedDate.AddDays(-6).ToString("MM/dd/yyyy");
                    break;
            }
            while (start_date != DateTime.Parse(end_date).AddDays(1).ToString("MM/dd/yyyy"))
            {
                returnDate.Add(start_date);
                start_date = DateTime.Parse(start_date).AddDays(1).ToString("MM/dd/yyyy");
            }
            return returnDate.ToArray();
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
