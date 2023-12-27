using TheBoops.ViewModel;

namespace TheBoops.Views;

public partial class UsersPage : ContentPage
{
    UsersPageViewModel viewmodel = new();

    public UsersPage()
	{
        InitializeComponent();
		BindingContext = viewmodel;
	}
    protected override void OnAppearing()
    {
        viewmodel.LoadPageData();
        base.OnAppearing();
    }
}