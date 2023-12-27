using TheBoops.ViewModel;
namespace TheBoops.Views;

public partial class RoomsPage : ContentPage
{
	RoomPageViewModel viewModel = new();
    public RoomsPage()
	{
		InitializeComponent();	
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        viewModel.LoadPageDataAsync();
        base.OnAppearing();
    }
}