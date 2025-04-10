using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class GameOverViewModel : ObservableObject
    {
        private GameManager _gameManager;
        private RoleType _winner;

        [ObservableProperty] public string winnerLabel;
        [ObservableProperty] public ObservableCollection<RolePresentation> allPlayers;
        [ObservableProperty] public string backGroundPicture;
        [ObservableProperty] public string backGroundColor;
        [ObservableProperty] public Microsoft.Maui.Aspect backGroundAspect;

        public GameOverViewModel(GameManager gm)
        {
            _gameManager = gm;
            WinnerLabel = _gameManager.WinnerLabel;
            AllPlayers = new ObservableCollection<RolePresentation>(RolePresentation.Clone(_gameManager.AllPlayers).ToList());
            
            SetWinner();
        }

        [RelayCommand]
        public void FinishUp()
        {
            _gameManager.Restart();
        }

        private void SetWinner()
        {
            _winner = WinnerLabel == "Die Werwölfe haben gewonnen." ? RoleType.Villain : RoleType.Villager;

            if (WinnerLabel == "Das Liebespaar hat gewonnen.")
            {
                _winner = RoleType.Couple;
            }
             
            if (_winner == RoleType.Villain)
            {
                BackGroundPicture = "night_mountains.png";
                BackGroundColor = "#0592A5";
                BackGroundAspect = Aspect.Center;
            }
            else
            {
                BackGroundPicture = "day_mountains2.png";
                BackGroundColor = "#FFCF7D";
                BackGroundAspect = Aspect.AspectFill;
            }

            foreach (RolePresentation player in AllPlayers)
            {
                if (_winner == RoleType.Villager)
                {
                    if (player.Type == RoleType.LarryPlus && !player.IsCouple)
                    {
                        player.IsWinner = true;
                    }
                }
                if (player.Type == _winner)
                {
                    player.IsWinner = true;
                }

                if (_winner == RoleType.Couple)
                {
                    if (player.IsCouple)
                    {
                        player.IsWinner = true;
                    }
                }
            }
        }
    }
}
