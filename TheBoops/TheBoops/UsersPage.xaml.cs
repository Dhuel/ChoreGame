using TheBoops.ViewModel;

namespace TheBoops;

public partial class UsersPage : ContentPage
{
    UsersPageViewModel viewModel_ { get; set; }
    public UsersPage(UsersPageViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
        viewModel_ = viewModel;



    }
    protected override void OnAppearing()
    {
        viewModel_.LoadPageData();
        base.OnAppearing();
    }
}