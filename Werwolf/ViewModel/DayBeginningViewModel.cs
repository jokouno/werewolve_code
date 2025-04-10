
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class DayBeginningViewModel : ObservableObject
    {
        private GameManager _gameManager;

        [ObservableProperty] public string dayCount;

        public DayBeginningViewModel(GameManager gm)
        {
            _gameManager = gm;
            dayCount = _gameManager.DayCounter;
        }

        [RelayCommand]
        public void RevealDeadPlayers()
        {
            _gameManager.RevealDeadPlayers();
        }
    }
}
