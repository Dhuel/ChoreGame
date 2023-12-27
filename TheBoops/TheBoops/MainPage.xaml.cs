using TheBoops.Services;
using TheBoops.ViewModel;

namespace TheBoops
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        protected async override void OnAppearing()
        {
            await NavigationHandler.NavToUsers();
            base.OnAppearing();
        }
    } 
}
