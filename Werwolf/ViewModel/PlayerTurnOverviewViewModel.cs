
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Xml;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    [QueryProperty(nameof(CurrentPlayerName), "CurrentPlayerName")]
    public partial class PlayerTurnOverviewViewModel : ObservableObject
    {
        private GameManager _gameManager;

        [ObservableProperty] public bool isRevealRoleButtonEnabled;
        [ObservableProperty] public string currentPlayerName;
        [ObservableProperty] public bool isContinueAvailable;

        public PlayerTurnOverviewViewModel(GameManager gameManager)
        {
            _gameManager = gameManager;
            IsRevealRoleButtonEnabled = false;
        }

        [RelayCommand]
        public void OnProfileImageTapped()
        {
            IsRevealRoleButtonEnabled = true;
        }

        [RelayCommand]
        public void OpenRole()
        {
            if (IsRevealRoleButtonEnabled)
            {
                _gameManager.OpenRole();
                IsRevealRoleButtonEnabled = false;
            }
        }
    }
}
