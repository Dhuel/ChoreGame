using Amazon.DynamoDBv2.DocumentModel;
using Android.Service.Autofill;
using System;
using System.Collections;
using System.Collections.Generic;
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
        #endregion
        #region Constructors
        public UsersPageViewModel()
        {
            Title = Constants.UsersPage;
            AddButtonClicked = new Command(async () => await  NavigationHandler.NavToAddPage(Title)); 
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
                int total = 0;
                List<SearchQuery>_ = new()
                {
                    new SearchQuery() { Field = "UserID", Operator = ScanOperator.Equal, FieldValue = user.UserID }
                };
                foreach (PointsDb _point in (List<PointsDb>)await _dbHaHandler.GetTableData(Constants.PointsAWSTable,new(){new SearchQuery() { Field = "UserID", Operator = ScanOperator.Equal, FieldValue = user.UserID}}))
                {
                    total += _point.PointValue;
                }
                List<UsersDisplay> _User = Users.ToList();
                _User.Add(new UsersDisplay(user, total));
                Users = _User;
            }
            
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
