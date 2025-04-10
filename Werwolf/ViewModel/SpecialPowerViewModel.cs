
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class SpecialPowerViewModel : ObservableObject
    {
        private GameManager _gameManager;
        private bool _showBeforeRoleOpening;
        private string _nextInfo;
        private bool _showAnotherInfo;
        [ObservableProperty] public ObservableCollection<RolePresentation> playerList;
        [ObservableProperty] public string powerLabel;
        [ObservableProperty] public bool isCoupleReveal;
        [ObservableProperty] public bool isBiteReveal;

        public SpecialPowerViewModel(GameManager gm)
        {
            _gameManager = gm;
            PlayerList = new ObservableCollection<RolePresentation>();
            GetPowerLabel();
            _showBeforeRoleOpening = _gameManager.CurrentPlayer.Connections.Any(x => x == Connection.Couple || x == Connection.Bite);
            IsCoupleReveal = _gameManager.CurrentPlayer.Connections.Any(x => x == Connection.Couple);
            IsBiteReveal = _gameManager.CurrentPlayer.Connections.Any(x => x == Connection.Bite);
            _showAnotherInfo = false;

            SetPlayerList();

            //_gameManager.CurrentPlayer.SelectedPlayersForAction.Clear();
        }

        [RelayCommand]
        public void FinishUp()
        {
            RolePresentation.HideAllAvatars();

            if (_showAnotherInfo)
            {
                _gameManager.ShowAnotherInfo(_nextInfo);
                return;
            }

            if (_showBeforeRoleOpening)
            {
                _gameManager.OpenRole();
            }
            else
            {
                if (_gameManager.Player.Count - 1 == _gameManager.CurrentPlayerCount)
                {
                    _gameManager.EndNight();
                    return;
                }
                _gameManager.NextRole();
            }
        }

        private void GetPowerLabel()
        {
            if (!string.IsNullOrEmpty(_gameManager.NextPlayerInfo))
            {
                PowerLabel = _gameManager.NextPlayerInfo;
                return;
            }

            switch (_gameManager.CurrentPlayer.RoleName)
            {
                case nameof(Seherin):
                    PowerLabel = "Du hast dir die Rolle von folgendem Spieler aufgedeckt:";
                    break;
                default:
                    PowerLabel = "Error";
                    break;
            }

            if (_gameManager.CurrentPlayer.Connections.Any(x => x == Connection.Bite))
            {
                PowerLabel = "Du bist vom Kätzchenwerwolf gebissen worden! Du verlierst deine alte Rolle und bist von nun an ein Werwolf!";
            }

            if (_gameManager.CurrentPlayer.Connections.Any(x => x == Connection.Couple))
            {
                PowerLabel = "Du bist durch Amor verliebt worden. Dein Liebespartner ist:";
            }
        }

        private void SetPlayerList()
        {
            if (!string.IsNullOrEmpty(_gameManager.NextPlayerInfo))
            {
                _nextInfo = string.Empty;
                _gameManager.NextPlayerInfo = string.Empty;
                _showAnotherInfo = false;
                return;
            }

            if (_gameManager.CurrentPlayer.Connections.Contains(Connection.Couple) &&
                _gameManager.CurrentPlayer.Connections.Contains(Connection.Bite))
            {
                foreach (string player in _gameManager.CurrentPlayer.SelectedPlayersForAction)
                {
                    Role selectedPlayer = _gameManager.Player.FirstOrDefault(x => x.PlayerName == player)!;

                    if (selectedPlayer != null)
                    {
                        PlayerList.Add(RolePresentation.Clone(selectedPlayer));
                        RolePresentation.RevealAvatar(player);
                    }
                }

                _showAnotherInfo = true;
                _nextInfo = "Du bist vom Kätzchenwerwolf gebissen worden! Du verlierst deine alte Rolle und bist von nun an ein Werwolf!";

                return;
            }

            if (_gameManager.CurrentPlayer.Connections.Contains(Connection.Couple) && string.IsNullOrEmpty(_gameManager.NextPlayerInfo))
            {
                foreach (string player in _gameManager.CurrentPlayer.SelectedPlayersForAction)
                {
                    Role selectedPlayer = _gameManager.Player.FirstOrDefault(x => x.PlayerName == player)!;

                    if (selectedPlayer != null)
                    {
                        PlayerList.Add(RolePresentation.Clone(selectedPlayer));
                        RolePresentation.RevealAvatar(player);
                    }
                }
            }
            else
            {
                PlayerList = new ObservableCollection<RolePresentation>();
            }
        }
    }
}
