
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel;

public partial class ChangedRoleViewModel : ObservableObject
{
    private readonly GameManager _gameManager;

    [ObservableProperty] public string infoLabel;
    [ObservableProperty] public string roleAvatar;

    public ChangedRoleViewModel(GameManager gm)
    {
        _gameManager = gm;
        roleAvatar = _gameManager.CurrentPlayer.Avatar;

        List<Connection> infos = _gameManager.CurrentPlayer.Connections.Where(x => x.ConnectionType == ConnectionType.ChangedRole).ToList();

        if (infos.Any())
        {
            foreach (Connection info in infos)
            {
                if (_gameManager.CurrentPlayer.RoleName == info.To.RoleName)
                {
                    infoLabel = info.Message;
                }
            }
        }
    }

    [RelayCommand]
    public void FinishUp()
    {
        _gameManager.CurrentPlayer.Connections.RemoveAll(x => x.ConnectionType == ConnectionType.ChangedRole);
        _gameManager.OpenRole();
    }
}