using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Data.Actions;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class PlayerTurnMenuViewModel : ObservableObject
    {
        private GameManager _gameManager;
        private Role? _lastSelectedPlayer;

        [ObservableProperty] public string roleName;
        [ObservableProperty] public string roleText;
        [ObservableProperty] public string roleAvatar;
        [ObservableProperty] public string continueLabel;
        [ObservableProperty] public bool isPlayerSelectionEnabled;
        [ObservableProperty] public bool isMultiplePlayerSelectionEnabled;
        [ObservableProperty] public bool isActionSelectionEnabled;
        [ObservableProperty] public bool isFinishUpButtonEnabled;
        [ObservableProperty] public bool isActionFinishUpButtonEnabled;
        [ObservableProperty] public bool isKillingCountEnabled;
        [ObservableProperty] public ObservableCollection<RolePresentation> playerList;
        [ObservableProperty] public ObservableCollection<PlayerAction> playerActions;
        [ObservableProperty] public ActionType actionType;

        public PlayerTurnMenuViewModel(GameManager gameManager)
        {
            try 
            {
                _gameManager = gameManager;
                roleName = _gameManager.CurrentPlayer.RoleName;
                roleText = _gameManager.CurrentPlayer.Text;
                roleAvatar = _gameManager.CurrentPlayer.Avatar;
                // ReSharper disable once MergeIntoPattern
                isPlayerSelectionEnabled = !_gameManager.CurrentPlayer.HasActionSelection && _gameManager.CurrentPlayer.HasPlayerSelection;
                IsActionSelectionEnabled = _gameManager.CurrentPlayer.HasActionSelection;
                isActionFinishUpButtonEnabled = false;
                isKillingCountEnabled = _gameManager.CurrentPlayer.Type ==  RoleType.Villain;
                ActionType = _gameManager.CurrentPlayer.ActionType;
                IsMultiplePlayerSelectionEnabled = _gameManager.CurrentPlayer.HasMultiplePlayerSelection;

                if (!_gameManager.CurrentPlayer.HasUsedOneTimeAction)
                {
                    playerList = new ObservableCollection<RolePresentation>(RolePresentation.Clone(_gameManager.Player)
                        .Where(player => player.PlayerName != _gameManager.CurrentPlayer.PlayerName)
                        .ToList());

                    if (ActionType == ActionType.Heal)
                    {
                        playerList.Add(RolePresentation.Clone(_gameManager.CurrentPlayer));

                        foreach (string selected in _gameManager.CurrentPlayer.SelectedPlayersForAction)
                        {
                            RolePresentation notSelectable = playerList.FirstOrDefault(x => x.PlayerName == selected)!;
                            if (notSelectable != null)
                            {
                                playerList.Remove(notSelectable);
                            }
                        }

                        _gameManager.CurrentPlayer.SelectedPlayersForAction.Clear();
                    }

                    if (ActionType == ActionType.Amorize)
                    {
                        _gameManager.CurrentPlayer.HasUsedOneTimeAction = true;
                        IsFinishUpButtonEnabled = false;

                        RolePresentation currentPlayer = RolePresentation.Clone(_gameManager.CurrentPlayer);

                        playerList.Add(currentPlayer);
                    }

                    ObservableCollection<PlayerAction> actions =
                        new ObservableCollection<PlayerAction>(_gameManager.CurrentPlayer.Actions);
                    playerActions = actions;

                    IsFinishUpButtonEnabled = !IsPlayerSelectionEnabled && !IsMultiplePlayerSelectionEnabled;

                    if (RoleName == nameof(KittenWerwolf))
                    {
                        IsFinishUpButtonEnabled = false;
                    }
                }
                else if (_gameManager.CurrentPlayer.Connections.Any(x => x == Connection.Bite))
                {
                    playerList = new ObservableCollection<RolePresentation>(RolePresentation.Clone(_gameManager.Player)
                        .Where(player => player.PlayerName != _gameManager.CurrentPlayer.PlayerName)
                        .ToList());
                }
                else
                {
                    IsFinishUpButtonEnabled = true;
                }

                ContinueLabel = IsActionSelectionEnabled ? "Überspringen" : "Weiter";
                IsActionFinishUpButtonEnabled = false;

                RolePresentation.RevealSameTeamAvatar(_gameManager.CurrentPlayer.PlayerName);
            }
            catch (Exception)
            {
                Stop();
            }
        }

        [RelayCommand]
        public void FinishUp()
        {
            IsFinishUpButtonEnabled = false;
            if (ActionType == ActionType.Reveal)
            {
                _gameManager.DoSpecialPower();
                return;
            }

            RolePresentation.HideAllAvatars();

            if (_lastSelectedPlayer != null && ActionType == ActionType.Kill)
            {
                _lastSelectedPlayer.VotedByCount++;
            }

            if (_gameManager.Player.Count - 1 == _gameManager.CurrentPlayerCount)
            {
                _gameManager.EndNight();
                return;
            }
            _gameManager.NextRole();
        }

        [RelayCommand]
        public void ActionFinishUp()
        {
            if (IsActionFinishUpButtonEnabled)
            {
                IsActionFinishUpButtonEnabled = false;
            }
            if (ActionType == ActionType.HealAll)
            {
                _gameManager.CurrentPlayer.DoAction(Enumerable.Empty<string>().ToList(), ActionType);
                FinishUp();
                return;
            }

            IsActionSelectionEnabled = false;
            IsPlayerSelectionEnabled = true;
            IsFinishUpButtonEnabled = false;
            ContinueLabel = "Weiter";
        }

        public void SetSelectedPlayers(List<string> names)
        {
            _gameManager.CurrentPlayer.DoAction(names, ActionType);
        }

        public void SetSelectedPlayer(string name)
        {
            RolePresentation selectedPlayer = PlayerList.FirstOrDefault(player => player.PlayerName == name)!;

            if (selectedPlayer != null)
            {
                _lastSelectedPlayer = _gameManager.Player.FirstOrDefault(x => x.PlayerName == selectedPlayer.PlayerName)!;

                IsFinishUpButtonEnabled = true;
                _gameManager.CurrentPlayer.DoAction(new List<string> { selectedPlayer.PlayerName }, ActionType);
            }
        }

        public void SetSelectedAction(PlayerAction action)
        {
            ActionType = action.ActionType;
            IsActionFinishUpButtonEnabled = true;
        }

        private async void Stop()
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}
