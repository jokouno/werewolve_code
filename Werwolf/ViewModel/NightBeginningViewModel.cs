using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class NightBeginningViewModel : ObservableObject
    {
        private GameManager _gameManager;
        [ObservableProperty] public string nightCount;

        public NightBeginningViewModel(GameManager gm)
        {
            _gameManager = gm;
            NightCount = string.Empty;
        }

        [RelayCommand]
        public void FinishUp()
        {
            _gameManager.StartNightMenu();
        }

        public void OnPageAppearing()
        {
            NightCount = _gameManager.NightCounter;
        }
    }
}
