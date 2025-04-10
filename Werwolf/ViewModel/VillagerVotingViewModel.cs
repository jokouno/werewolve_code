
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class VillagerVotingViewModel : ObservableObject
    {
        private GameManager _gameManager;
        [ObservableProperty] public string playerName;
        [ObservableProperty] public string roleAvatar;
        [ObservableProperty] public bool isAllowedToVote;
        [ObservableProperty] public bool isNotAllowedToVote;
        [ObservableProperty] public bool isFinishUpButtonEnabled;
        [ObservableProperty] public ObservableCollection<RolePresentation> playerList;
        private List<RolePresentation> _playersVoted;

        public VillagerVotingViewModel(GameManager gm)
        {
            _gameManager = gm;
            _playersVoted = new List<RolePresentation>();
            RolePresentation currentPlayer = RolePresentation.Clone(_gameManager.CurrentPlayer);
            PlayerName = $"{currentPlayer.PlayerName} stimmt ab";
            RoleAvatar = currentPlayer.DefaultAvatar;
            IsAllowedToVote = currentPlayer.IsAllowedToVote;
            IsNotAllowedToVote = !IsAllowedToVote;
            PlayerList = new ObservableCollection<RolePresentation>(RolePresentation.Clone(_gameManager.Player).Where(player => player.PlayerName != currentPlayer.PlayerName)
                .ToList());

            IsFinishUpButtonEnabled = isNotAllowedToVote;
        }

        [RelayCommand]
        public void FinishUp()
        {
            if (IsAllowedToVote)
            {
                _gameManager.VotForVillagerElection(_playersVoted);
            }
            _playersVoted.Clear();

            if (_gameManager.Player.Count - 1 == _gameManager.CurrentPlayerCount)
            {
                _gameManager.EndPlayerVote();
                return;
            }

            _gameManager.NextPlayerVote();
        }

        public void SetSelectedPlayer(string name)
        {
            RolePresentation selectedPlayer = PlayerList.FirstOrDefault(player => player.PlayerName == name)!;

            if (selectedPlayer == null)
            {
                return;
            }

            IsFinishUpButtonEnabled = true;
            _playersVoted.Add(selectedPlayer);
        }
    }
}
