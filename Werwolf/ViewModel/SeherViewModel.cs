using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class SeherViewModel : ObservableObject
    {
        private GameManager _gameManager;
        private bool _showBeforeRoleOpening;
        [ObservableProperty] public ObservableCollection<RolePresentation> playerList;
        [ObservableProperty] public string powerLabel;
        [ObservableProperty] public bool isCoupleReveal;
        [ObservableProperty] public bool isBiteReveal;

        public SeherViewModel(GameManager gm)
        {
            _gameManager = gm;
            PlayerList = new ObservableCollection<RolePresentation>();
            PowerLabel = "Du hast dir die Rolle von folgendem Spieler aufgedeckt:";
            _showBeforeRoleOpening = _gameManager.CurrentPlayer.Connections.Any(x => x.ConnectionType == ConnectionType.Couple || x.ConnectionType == ConnectionType.Bite);
            IsCoupleReveal = _gameManager.CurrentPlayer.Connections.Any(x => x.ConnectionType == ConnectionType.Couple);
            IsBiteReveal = _gameManager.CurrentPlayer.Connections.Any(x => x.ConnectionType == ConnectionType.Bite);

            foreach (string player in _gameManager.CurrentPlayer.SelectedPlayersForAction)
            {
                Role selectedPlayer = _gameManager.Player.FirstOrDefault(x => x.PlayerName == player)!;

                if (selectedPlayer != null)
                {
                    PlayerList.Add(RolePresentation.Clone(selectedPlayer));
                    RolePresentation.RevealAvatar(player);
                }
            }

            _gameManager.CurrentPlayer.SelectedPlayersForAction.Clear();
        }

        [RelayCommand]
        public void FinishUp()
        {
            RolePresentation.HideAllAvatars();

            if (_gameManager.Player.Count - 1 == _gameManager.CurrentPlayerCount)
            {
                _gameManager.EndNight();
                return;
            }
            _gameManager.NextRole();
        }
    }
}
