
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class DayDeadPlayerViewModel : ObservableObject
    {
        private GameManager _gameManager;

        [ObservableProperty] public ObservableCollection<RolePresentation> deadPlayers;
        [ObservableProperty] public string titleLabel;
        [ObservableProperty] public DeadPlayerPageType deadPlayerPageType;

        public DayDeadPlayerViewModel(GameManager gm)
        {
            _gameManager = gm;
            titleLabel = string.Empty;
            deadPlayers = new ObservableCollection<RolePresentation>();
            DeadPlayerPageType = _gameManager.DeadPlayerPageType;

            foreach (RolePresentation rolePresentation in RolePresentation.Clone(gm.DeadPlayers))
            {
                deadPlayers.Add(rolePresentation);
                RolePresentation.RevealAvatar(rolePresentation.playerName);
            }

            SetLabel();
        }

        [RelayCommand]
        public void FinishUp()
        {
            _gameManager.CheckGameOver();
             if (_gameManager.IsGameOver)
            {
                return;
            }

            if (DeadPlayerPageType == DeadPlayerPageType.DeadByNight)
            {
                _gameManager.StartDiscussionInfo();
            }
            else if (DeadPlayerPageType == DeadPlayerPageType.DeadByVoting)
            {
                _gameManager.StartNight();
            }
        }

        private void SetLabel()
        {
            if (DeadPlayerPageType == DeadPlayerPageType.DeadByNight)
            {
                switch (DeadPlayers.Count)
                {
                    case 0:
                        TitleLabel = "Diese Nacht sind keine Spieler gestorben.";
                        break;
                    case 1:
                        TitleLabel = "Diese Nacht ist folgender Spieler gestorben:";
                        break;
                    default:
                        TitleLabel = "Diese Nacht sind folgende Spieler gestorben:";
                        break;
                }
            }

            if (DeadPlayerPageType == DeadPlayerPageType.DeadByVoting)
            {
                switch (DeadPlayers.Count)
                {
                    case 0:
                        TitleLabel = "Das Dorf konnte sich nicht einig werden. Am heutigen Tag wird niemand umgebracht.";
                        break;
                    case 1:
                        TitleLabel = "Folgender Spieler wurde durch das Dorf umgebracht:";
                        break;
                    default:
                        TitleLabel = "Folgende Spieler wurden durch das Dorf umgebracht:";
                        break;
                }
            }
        }
    }

    public enum DeadPlayerPageType
    {
        None = 0,
        DeadByNight = 1,
        DeadByVoting = 2
    }
}
