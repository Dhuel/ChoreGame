using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBoops.Database.Tables;
using TheBoops.Views;

namespace TheBoops.Services
{
    public static class NavigationHandler
    {
        public async static Task NavToRooms()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RoomsPage());
        }

        public async static Task NavToMissions(RoomsDb _room)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MissionsPage(_room));
        }
        public async static Task NavToAddPage(string PageName, RoomsDb room = null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddPage(PageName, room));
        }

        internal async static Task PopPage()
        {
            await Application.Current.MainPage.Navigation.PopAsync(); 
        }

        internal async static Task PopToRoot()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        internal async static Task DisplayAlert(string v)
        {
            await Application.Current.MainPage.DisplayAlert("Alert",v,"OK");
        }
    }
}
