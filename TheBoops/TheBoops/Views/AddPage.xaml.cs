using TheBoops.Database.Tables;
using TheBoops.ViewModel;

namespace TheBoops.Views;

public partial class AddPage : ContentPage
{

    AddPageViewModel viewmodel;

    public AddPage(string pageTitle, RoomsDb i_Room)
	{
        viewmodel = new AddPageViewModel(pageTitle, i_Room);
		InitializeComponent();
        BindingContext = viewmodel;


        switch (pageTitle)
        {
            case Global.Constants.UsersPage:
                AddUserPageLayout();
                break;
            case Global.Constants.RoomsPage:
                AddRoomPageLayout();
                break;
            case Global.Constants.MissionsPage:
                AddMissionPageLayout();
                break;
            default:
                break;
        }
    }

    private void AddUserPageLayout()
    {
        Title = "Add New User";
        AddUser.IsVisible = true;
        viewmodel.SetUserValues(UserNameEntry);
    }
    private void AddRoomPageLayout()
    {
        Title = "Add New Room";
        AddRoom.IsVisible = true;
        viewmodel.SetRoomValues(RoomNameEntry);
    }
    private void AddMissionPageLayout()
    {
        Title = "Add New Mission";
        AddMission.IsVisible = true;
        viewmodel.SetMissionValues(MissionNameEntry, MissionPointsEntry);
    }


}