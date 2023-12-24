using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheBoops.Database.DbHandlers;
using TheBoops.Database.Tables;

namespace TheBoops.ViewModel
{
    public class MainPageViewModel
    {
        private readonly IDbHandler _DbHandler;
        public ICommand ButtonCommand { get; }
        public MainPageViewModel(IDbHandler DbHandler)
        {
            ButtonCommand = new Command(async () => await CheckDB());
            _DbHandler = DbHandler;
        }

        private async Task CheckDB()
        {
            if(await Tests.Tests.RunTestSet(_DbHandler))
            {
                Console.WriteLine("Tests passed");
            }
            else
            {
                Console.WriteLine("TEsts failed");
            }
        }
    }
}
