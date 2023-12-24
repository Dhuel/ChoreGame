using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheBoops.Database.DbHandlers;

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
            await _DbHandler.UserExists("Dhuel");
        }
    }
}
