using TheBoops.Database.Tables;
using TheBoops.ViewModel;

namespace TheBoops.Views;

public partial class MissionsPage : ContentPage
{
	MissionsPageViewModel viewModel;
    RoomsDb Room { get; set; }
    public MissionsPage(RoomsDb _room)
	{
		viewModel = new(_room);
        Room = _room;
        InitializeComponent();
		BindingContext = viewModel;

	}
    protected override void OnAppearing()
    {
        viewModel.LoadPageData(Room);
        base.OnAppearing();
    }
}