
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class DiscussionViewModel : ObservableObject
    {
        private GameManager _gameManager;

        public DiscussionViewModel(GameManager gm)
        {
            _gameManager = gm;
        }

        [RelayCommand]
        public void FinishUp()
        {
            _gameManager.StartDiscussion();
        }
    }
}
