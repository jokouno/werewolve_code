using CommunityToolkit.Mvvm.ComponentModel;

namespace Werwolf.Data.Actions
{
    public partial class PlayerAction : ObservableObject
    {
        [ObservableProperty] public string actionName;
        [ObservableProperty] public string actionPicture;
        [ObservableProperty] public ActionType actionType;
        [ObservableProperty] public List<RolePresentation> selectedPlayers;
    }
}
