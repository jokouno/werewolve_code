
using Werwolf.Data;
using Werwolf.Data.Actions;
using Werwolf.ViewModel;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Werwolf.Workflow
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    public class GameManager
    {
        private static readonly GameManager _instance = new GameManager();
        private List<Role> _roles;
        private List<Role> _player;
        private List<string> _names;
        private List<Role> _deadPlayers;
        private List<Role> _nextDeadPlayers;
        private int _dayCount;
        private int _nightCount;
        private List<RolePresentation> _selectedRoles;
        private Dictionary<string, int> _villageVotings;
        public int CurrentPlayerCount;
        public string NextPlayerInfo;

        public static readonly List<Role> AllRoles = new List<Role>
        {
            new AlteSchrulle(),
            new Amor(),
            new Doctor(),
            new Dorfbewohner(),
            new Grabrauber(),
            new Hexe(),
            new KittenWerwolf(),
            new Raecher(),
            new Seherin(),
            new Data.Werwolf()
        };

        public List<RolePresentation> SelectedRoles
        {
            get => _selectedRoles;
            set => _selectedRoles = value;
        }

        public static GameManager Instance => _instance;
        public List<Role> Roles => _roles;
        public List<Role> Player => _player.Where(x => x.IsAlive).ToList();
        public List<Role> AllPlayers => _player;
        public List<string> Names => _names;
        public Role CurrentPlayer => Player[CurrentPlayerCount];
        public string DayCounter => $"{_dayCount++}. Tag";
        public string NightCounter => $"{_nightCount++}. Nacht";
        public bool IsGameOver;
        public List<Role> DeadPlayers => _deadPlayers;
        public DeadPlayerPageType DeadPlayerPageType;
        public string WinnerLabel;

        public GameManager()
        {
            _roles = new List<Role>();
            _player = new List<Role>();
            _names = new List<string>();
            _deadPlayers = new List<Role>();
            _nextDeadPlayers = new List<Role>();
            NextPlayerInfo = string.Empty;
            WinnerLabel = string.Empty;
            _dayCount = 1;
            _nightCount = 1;
            _selectedRoles = new List<RolePresentation>();
            _villageVotings = new Dictionary<string, int>();
            DeadPlayerPageType = DeadPlayerPageType.None;
            IsGameOver = false;
            RestartGameStats();
        }

        public async void StartGame(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(StartGame), roles);
            try
            {
                CurrentPlayerCount = -1;
                IsGameOver = false;
                _roles = roles;
                InitializeRoles();

#if DEBUG
                for (int i = 0; i < _player.Count; i++)
                {
                    switch (_player[i].PlayerName)
                    {
                        case nameof(AlteSchrulle):
                            _player[i] = new AlteSchrulle(_player[i]);
                            break;
                        case nameof(Dorfbewohner):
                            _player[i] = new Dorfbewohner(_player[i]);
                            break;
                        case nameof(Doctor):
                            _player[i] = new Doctor(_player[i]);
                            break;
                        case nameof(Hexe):
                            _player[i] = new Hexe(_player[i]);
                            break;
                        case nameof(Raecher):
                            _player[i] = new Raecher(_player[i]);
                            break;
                        case nameof(Seherin):
                            _player[i] = new Seherin(_player[i]);
                            break;
                        case "Werwolf":
                            _player[i] = new Data.Werwolf(_player[i]);
                            break;
                        case nameof(Amor):
                            _player[i] = new Amor(_player[i]);
                            break;
                        case "KittenWerwolf":
                            _player[i] = new KittenWerwolf(_player[i]);
                            break;
                        case nameof(Grabrauber):
                            _player[i] = new Grabrauber(_player[i]);
                            break;
                    }
                }
#endif

                await Shell.Current.GoToAsync($"//{nameof(NightBeginningPage)}");
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void StartRandomRolesGame()
        {
            ExceptionLogger.Log(nameof(StartRandomRolesGame));
            try
            {
                CurrentPlayerCount = -1;
                IsGameOver = false;

                List<Role> randomRoles = new List<Role>();

                if (Names.Count <= 6)
                {
                    randomRoles.Add(new Data.Werwolf());

                    for (int i = 0; i < Names.Count - 1; i++)
                    {
                        randomRoles.Add(GetRandomRole());
                    }
                }
                else if (Names.Count <= 12)
                {
                    randomRoles.Add(new Data.Werwolf());
                    randomRoles.Add(new Data.Werwolf());
                    randomRoles.Add(new Data.Werwolf());

                    for (int i = 0; i < Names.Count - 3; i++)
                    {
                        randomRoles.Add(GetRandomRole());
                    }
                }
                else
                {
                    randomRoles.Add(new Data.Werwolf());
                    randomRoles.Add(new Data.Werwolf());
                    randomRoles.Add(new Data.Werwolf());
                    randomRoles.Add(new Data.Werwolf());

                    for (int i = 0; i < Names.Count - 4; i++)
                    {
                        randomRoles.Add(GetRandomRole());
                    }
                }

                Roles.Clear();
                _roles = randomRoles;

                InitializeRoles();

                await Shell.Current.GoToAsync($"//{nameof(NightBeginningPage)}");
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public Role GetRandomRole()
        {
            ExceptionLogger.Log(nameof(GetRandomRole));
            try
            {
                Random random = new Random();
                int randomNumber = random.Next(0, AllRoles.Count - 1);

                switch (AllRoles[randomNumber])
                {
                    case AlteSchrulle:
                        return new AlteSchrulle();
                    case Dorfbewohner:
                        return new Dorfbewohner();
                    case Doctor:
                        return new Doctor();
                    case Hexe:
                        return new Hexe();
                    case Raecher:
                        return new Raecher();
                    case Seherin:
                        return new Seherin();
                    case Data.Werwolf:
                        return new Data.Werwolf();
                    case Amor:
                        return new Amor();
                    case KittenWerwolf:
                        return new KittenWerwolf();
                    case Grabrauber:
                        return new Grabrauber();
                }

                return new Dorfbewohner();
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void NextRole()
        {
            ExceptionLogger.Log(nameof(NextRole));
            try
            {
                if (CurrentPlayerCount == Player.Count)
                {
                    return;
                }

                CurrentPlayerCount++;
                while (!CurrentPlayer.IsAlive)
                {
                    CurrentPlayerCount++;
                }

                await Shell.Current.GoToAsync($"{nameof(PlayerTurnOverviewPage)}?CurrentPlayerName={CurrentPlayer.PlayerName}");
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void NextPlayerVote()
        {
            ExceptionLogger.Log(nameof(NextPlayerVote));
            try
            {
                if (CurrentPlayerCount == Player.Count)
                {
                    return;
                }

                CurrentPlayerCount++;
                while (!CurrentPlayer.IsAlive)
                {
                    CurrentPlayerCount++;
                }

                await Shell.Current.GoToAsync(nameof(VillagerVotingPage));
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void EndPlayerVote()
        {
            ExceptionLogger.Log(nameof(EndPlayerVote), DayCounter, NightCounter);
            try
            {
                DeadPlayers.Clear();
                _nextDeadPlayers.Clear();

                string votedPlayer = GetOnlyHighestVotedPlayer(_villageVotings);

                if (!string.IsNullOrEmpty(votedPlayer))
                {
                    Role votedRole = Player.FirstOrDefault(role => role.PlayerName == votedPlayer)!;

                    if (votedRole != null)
                    {
                        Kill(votedRole);

                        ProcessDeadPlayers();

                        DeadPlayers.AddRange(_nextDeadPlayers);
                    }
                }

                DeadPlayerPageType = DeadPlayerPageType.DeadByVoting;

                Player.ForEach(x => x.IsAllowedToVote = true);
                Player.ForEach(x => x.VotedByCount = 0);
                _villageVotings.Clear();

                await Shell.Current.GoToAsync(nameof(DayDeadPlayerPage));
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public void VotForVillagerElection(List<RolePresentation> roles)
        {
            ExceptionLogger.Log(nameof(VotForVillagerElection), roles.Count);
            try
            {
                foreach (RolePresentation role in roles)
                {
                    Role selectedRole = Player.FirstOrDefault(x => x.PlayerName == role.PlayerName)!;
                    if (selectedRole != null)
                    {
                        selectedRole.VotedByCount++;
                    }

                    string name = role.PlayerName;
                    if (!_villageVotings.TryAdd(name, 1))
                    {
                        _villageVotings[name]++;
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public void SetNames(List<string> names)
        {
            ExceptionLogger.Log(nameof(SetNames), names.Count);
            try
            {
                _names = names;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public void InitializeRoles()
        {
            ExceptionLogger.Log(nameof(InitializeRoles));
            try
            {
                _player.Clear();

                foreach (Role role in Roles)
                {
                    _player.AddRange(role.Start());
                }

                if (_player.Count != Names.Count)
                {
                    throw new Exception();
                }

                Random random = new Random();
                _player = Player.OrderBy(_ => random.Next()).ToList();

                int count = 0;
                foreach (string name in Names)
                {
                    Player[count++].PlayerName = name;
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void OpenRole()
        {
            ExceptionLogger.Log(nameof(OpenRole), CurrentPlayer.PlayerName);
            try
            {
                if (CurrentPlayer.Connections.Any(x => x.ConnectionType == ConnectionType.Couple || x.ConnectionType == ConnectionType.Bite)
                    && !CurrentPlayer.IsOneTimeInfoHasBeenShown)
                {
                    if (CurrentPlayer.Connections.Any(x => x.ConnectionType == ConnectionType.Couple))
                    {
                        CurrentPlayer.SelectedPlayersForAction =
                            Player.Where(x => x.Connections.Any(y => y.ConnectionType == ConnectionType.Couple) && x.PlayerName != CurrentPlayer.PlayerName).
                                Select(x => x.PlayerName).ToList();
                    }

                    CurrentPlayer.IsOneTimeInfoHasBeenShown = true;

                    await Shell.Current.GoToAsync(nameof(SpecialPowerPage));
                }
                else
                {
                    await Shell.Current.GoToAsync(nameof(PlayerTurnMenuPage));
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void ShowAnotherInfo(string info)
        {
            ExceptionLogger.Log(nameof(ShowAnotherInfo), info);
            try
            {
                NextPlayerInfo = info;
                await Shell.Current.GoToAsync(nameof(SpecialPowerPage));
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public void StartNightMenu()
        {
            ExceptionLogger.Log(nameof(StartNightMenu));
            try
            {
                NextRole();
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void EndNight()
        {
            ExceptionLogger.Log(nameof(EndNight), DayCounter, NightCounter);
            try
            {
                DeadPlayers.Clear();
                _nextDeadPlayers.Clear();
                ProcessNight();

                await Shell.Current.GoToAsync(nameof(DayBeginningPage));
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void RevealDeadPlayers()
        {
            ExceptionLogger.Log(nameof(RevealDeadPlayers), DayCounter, NightCounter);
            try
            {
                _deadPlayers = _nextDeadPlayers;

                foreach (Role deadPlayer in DeadPlayers)
                {
                    deadPlayer.IsAlive = false;
                }

                DeadPlayerPageType = DeadPlayerPageType.DeadByNight;

                await Shell.Current.GoToAsync(nameof(DayDeadPlayerPage));
                DeadPlayers.Clear();
                _nextDeadPlayers.Clear();
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void StartDiscussionInfo()
        {
            ExceptionLogger.Log(nameof(StartDiscussionInfo));
            try
            {
                await Shell.Current.GoToAsync(nameof(DiscussionPage));
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void StartDiscussion()
        {
            ExceptionLogger.Log(nameof(StartDiscussion));
            try
            {
                CurrentPlayerCount = -1;
                await Shell.Current.GoToAsync(nameof(DiscussionTimerPage));
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void StartNight()
        {
            ExceptionLogger.Log(nameof(StartNight), DayCounter, NightCounter);
            try
            {
                CurrentPlayerCount = -1;
                RolePresentation.HideAllAvatars();
                foreach (Role player in DeadPlayers)
                {
                    player.IsAlive = false;
                }
                DeadPlayers.Clear();
                _nextDeadPlayers.Clear();
                await Shell.Current.GoToAsync($"//{nameof(NightBeginningPage)}");
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void DoSpecialPower()
        {
            ExceptionLogger.Log(nameof(DoSpecialPower));
            try
            {
                await Shell.Current.GoToAsync(nameof(SeherPage));
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public void ProcessActions()
        {
            ExceptionLogger.Log(nameof(ProcessActions), NightCounter);
            try
            {
                // Kill

                foreach (Role toKill in Player.Where(x => x.SelectedFor.Contains(ActionType.Kill)).ToList())
                {
                    if (!toKill.SelectedFor.Contains(ActionType.Heal))
                    {
                        Kill(toKill);
                    }
                }

                ProcessDeadPlayers();

                foreach (Role toHeal in Player.Where(x => x.SelectedFor.Contains(ActionType.Heal)).ToList())
                {
                    if (toHeal.Connections.Any())
                    {
                        // Only heal player, if there is no couple partner already dead
                        foreach (Connection connection in toHeal.Connections)
                        {
                            if (!(connection.ConnectionType == ConnectionType.Couple && _nextDeadPlayers.Any(x => connection.DiesToo != null && connection.DiesToo.Contains(x.PlayerName))))
                            {
                                _nextDeadPlayers.Remove(toHeal);
                            }
                        }
                    }
                    else
                    {
                        _nextDeadPlayers.Remove(toHeal);
                    }
                }

                // Bite
                foreach (Role toBite in Player.Where(x => x.SelectedFor.Contains(ActionType.Bite)).ToList())
                {
                    if (toBite.IsAlive)
                    {
                        int index = AllPlayers.FindIndex(x => x.PlayerName == toBite.PlayerName && x.IsAlive);

                        if (index != -1)
                        {
                            Data.Werwolf bittenOne = new Data.Werwolf(_player[index]);

                            _player.RemoveAt(index);
                            _player.Insert(index, bittenOne);

                            bittenOne.Connections.RemoveAll(x => x.ConnectionType == ConnectionType.Bite);
                        }

                        toBite.Connections.RemoveAll(x => x.ConnectionType == ConnectionType.Bite);
                    }
                }

                // Grabräuber
                foreach (Role stealRole in Player.Where(x => x.SelectedFor.Contains(ActionType.StealRole)).ToList())
                {
                    if (!_nextDeadPlayers.Contains(stealRole))
                    {
                        continue;
                    }

                    if (stealRole != null)
                    {
                        // Player selected is dead -> steal role

                        List<Role> stolen = new List<Role>();
                        List<Role> stealer = Player.Where(x => x.Connections.Any(x => x.From == stealRole && x.ConnectionType == ConnectionType.StealRole)).ToList();

                        foreach (Role steal in stealer)
                        {
                            switch (stealRole)
                            {
                                case AlteSchrulle:
                                    stolen.Add(new AlteSchrulle(steal));
                                    break;
                                case Dorfbewohner:
                                    stolen.Add(new Dorfbewohner(steal));
                                    break;
                                case Doctor:
                                    stolen.Add(new Doctor(steal));
                                    break;
                                case Hexe:
                                    stolen.Add(new Hexe(steal));
                                    break;
                                case Raecher:
                                    stolen.Add(new Raecher(steal));
                                    break;
                                case Seherin:
                                    stolen.Add(new Seherin(steal));
                                    break;
                                case Data.Werwolf:
                                    stolen.Add(new Data.Werwolf(steal));
                                    break;
                                case Amor:
                                    stolen.Add(new Amor(steal));
                                    break;
                                case KittenWerwolf:
                                    stolen.Add(new KittenWerwolf(steal));
                                    break;
                                case Grabrauber:
                                    stolen.Add(new Grabrauber(steal));
                                    break;
                                default:
                                    stolen = null!;
                                    break;
                            }
                        }

                        if (stolen != null && stolen.Any())
                        {
                            foreach (Role steal in stolen)
                            {
                                int index = AllPlayers.FindIndex(x => x.PlayerName == steal.PlayerName);

                                if (index != -1)
                                {
                                    // connection hinzufügen für Info page

                                    AllPlayers.RemoveAt(index);
                                    AllPlayers.Insert(index, steal);
                                }

                                steal.Connections.RemoveAll(x => x.ConnectionType == ConnectionType.StealRole);
                            }
                        }
                    }
                }

                foreach (Role deadPlayer in _nextDeadPlayers.ToList())
                {
                    deadPlayer.IsAlive = false;
                    DeadPlayers.Add(deadPlayer);
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessDeadPlayers()
        {
            ExceptionLogger.Log(nameof(ProcessDeadPlayers));
            try
            {
                bool newDeadPlayer = true;
                while (newDeadPlayer)
                {
                    newDeadPlayer = false;
                    foreach (Role deadPlayer in _nextDeadPlayers.ToList())
                    {
                        foreach (Connection deadPlayerConnection in deadPlayer.Connections)
                        {
                            if (deadPlayerConnection.To != deadPlayer)
                            {
                                continue;
                            }

                            if (deadPlayerConnection.ConnectionType == ConnectionType.RevengeKill
                                || deadPlayerConnection.ConnectionType == ConnectionType.Couple)
                            {
                                if (deadPlayerConnection.DiesToo != null)
                                    foreach (string diesToo in deadPlayerConnection.DiesToo)
                                    {
                                        Role diesTooRole = AllPlayers?.FirstOrDefault(x => x.PlayerName == diesToo)!;

                                        if (diesTooRole != null && !_nextDeadPlayers.Contains(diesTooRole))
                                        {
                                            _nextDeadPlayers.Add(diesTooRole);
                                            newDeadPlayer = true;
                                        }
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public void ProcessNight()
        {
            ExceptionLogger.Log(nameof(ProcessNight));
            try
            {
                // ----------- Process all kill categories -----------

                // Instant Kill
                List<Role> instantKillRoles = Player.Where(x => x.ActionType == ActionType.InstantKill).ToList();
                ProcessInstantKill(instantKillRoles);

                // Kill
                List<Role> killRoles = Player.Where(x => x.ActionType == ActionType.Kill).ToList();
                ProcessKill(killRoles);

                // Revenge Kill
                List<Role> revengeKills = AllPlayers.Where(x => x.ActionType == ActionType.RevengeKill).ToList();
                ProcessRevengeKill(revengeKills);

                // ----------- Process all heal categories -----------

                // Heal
                List<Role> healRoles = Player.Where(x => x.ActionType == ActionType.Heal).ToList();
                ProcessHeal(healRoles);

                // Heal All
                List<Role> healAll = Player.Where(x => x.ActionType == ActionType.HealAll).ToList();
                ProcessHealAll(healAll);

                // ----------- Process special categories  -----------

                // Amorize
                List<Role> amorized = AllPlayers.Where(x => x.ActionType == ActionType.Amorize).ToList();
                ProcessAmorize(amorized);

                //ProcessNewDeadPlayer();

                // ----------- No one dies after this line -----------
                // NotAllowedToVote
                List<Role> noVoteAllowed = AllPlayers.Where(x => x.ActionType == ActionType.NoVoteAllowed).ToList();
                ProcessNoVoteAllowed(noVoteAllowed);

                // Bite -> auch dead Player bearbeiten
                List<Role> bites = AllPlayers.Where(x => x.ActionType == ActionType.Bite).ToList();
                ProcessBite(bites);

                // Grabräuber
                List<Role> grabRaubers = Player.Where(x => x.ActionType == ActionType.StealRole).ToList();
                ProcessGrabrauber(grabRaubers);

                // ----------- must be alive roles to do action  -----------

                // Dead Player | Couple dies | Revenge Kill 
                //List<Role> newDeadPlayers = ProcessDeadPlayers();
                //AddDeadPlayers(newDeadPlayers);

                ProcessActions();

                CheckGameOver();
                Player.ForEach(x => x.VotedByCount = 0);
                Player.Where(x => x.ActionType != ActionType.Heal && x.ActionType != ActionType.RevengeKill).ToList().ForEach(x => x.SelectedPlayersForAction = Enumerable.Empty<string>().ToList());
                Player.ForEach(x => x.SelectedFor.Clear());

                foreach (Role hexe in Player.Where(x => x.RoleName == nameof(Hexe)))
                {
                    hexe.ActionType = ActionType.None;
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessInstantKill(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessInstantKill), roles.Count);
            try
            {
                if (roles.Any(x => x.ActionType != ActionType.InstantKill) || !roles.Any())
                {
                    return;
                }

                foreach (Role player in roles)
                {
                    foreach (string toAction in player.SelectedPlayersForAction)
                    {
                        var selectedPlayer = AllPlayers.FirstOrDefault(x => x.PlayerName == toAction);

                        if (selectedPlayer != null)
                        {
                            selectedPlayer.SelectedFor.Add(ActionType.Kill);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessKill(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessKill), roles.Count);
            try
            {
                Dictionary<string, int> votedKill = new Dictionary<string, int>();

                if (roles.Any(x => x.ActionType != ActionType.Kill) || !roles.Any())
                {
                    return;
                }

                foreach (Role player in roles)
                {
                    if (player.SelectedPlayersForAction.Any())
                    {
                        foreach (string toAction in player.SelectedPlayersForAction)
                        {
                            if (!votedKill.TryAdd(toAction, 1))
                            {
                                votedKill[toAction]++;
                            }
                        }
                    }
                }

                string votedPlayer = GetHighestVotedPlayer(votedKill);

                if (votedPlayer != string.Empty)
                {
                    Role votedToKill = AllPlayers.FirstOrDefault(x => x.PlayerName == votedPlayer)!;

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                    if (votedToKill != null)
                    {
                        votedToKill.SelectedFor.Add(ActionType.Kill);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessRevengeKill(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessRevengeKill), roles.Count);
            try
            {
                if (roles.Any(x => x.ActionType != ActionType.RevengeKill) || !roles.Any())
                {
                    return;
                }

                foreach (Role role in roles)
                {
                    role.Connections.RemoveAll(x => x.ConnectionType == ConnectionType.RevengeKill && x.From == role);

                    foreach (string toDieToo in role.SelectedPlayersForAction)
                    {
                        Role toRevengeDie = AllPlayers.FirstOrDefault(x => x.PlayerName == toDieToo)!;

                        if (toRevengeDie != null)
                        {
                            role.Connections.Add(new Connection(ConnectionType.RevengeKill, role, role, new List<string> { toRevengeDie.PlayerName }));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessHeal(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessHeal), roles.Count);
            try
            {
                if (roles.Any(x => x.ActionType != ActionType.Heal) || !roles.Any())
                {
                    return;
                }

                foreach (Role player in roles)
                {
                    foreach (string toAction in player.SelectedPlayersForAction)
                    {
                        Role toHeal = AllPlayers.FirstOrDefault(x => x.PlayerName == toAction)!;

                        if (toHeal != null)
                        {
                            toHeal.SelectedFor.Add(ActionType.Heal);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessNoVoteAllowed(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessNoVoteAllowed), roles.Count);
            try
            {
                AllPlayers.ForEach(x => x.IsAllowedToVote = true);
                if (roles.Any(x => x.ActionType != ActionType.NoVoteAllowed) || !roles.Any())
                {
                    return;
                }

                foreach (Role player in roles)
                {
                    foreach (string toAction in player.SelectedPlayersForAction)
                    {
                        Role notAllowedToVote = AllPlayers.FirstOrDefault(x => x.PlayerName == toAction)!;

                        if (notAllowedToVote != null)
                        {
                            notAllowedToVote.IsAllowedToVote = false;
                            notAllowedToVote.SelectedFor.Add(ActionType.NoVoteAllowed);
                        }
                    }
                    player.SelectedPlayersForAction.Clear();
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessHealAll(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessHealAll), roles.Count);
            try
            {
                if (roles.Any(x => x.ActionType != ActionType.HealAll) || !roles.Any())
                {
                    return;
                }

                foreach (Role role in AllPlayers)
                {
                    role.SelectedFor.Add(ActionType.Heal);
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessAmorize(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessAmorize), roles.Count);
            try
            {
                if (roles.Any(x => x.ActionType != ActionType.Amorize)
                    || !roles.Any())
                {
                    return;
                }

                foreach (Role role in roles)
                {
                    if (!role.IsAlive
                        || role?.SelectedPlayersForAction?.Count != 2)
                    {
                        continue;
                    }
                    Role amori1 = AllPlayers.FirstOrDefault(x => x.PlayerName == role.SelectedPlayersForAction[0])!;
                    Role amori2 = AllPlayers.FirstOrDefault(x => x.PlayerName == role.SelectedPlayersForAction[1])!;

                    if (amori1 != null && amori2 != null)
                    {
                        amori1.Connections.Add(new Connection(ConnectionType.Couple, role, amori1, new List<string> { amori2.PlayerName }));
                        amori2.Connections.Add(new Connection(ConnectionType.Couple, role, amori2, new List<string> { amori1.PlayerName }));

                        amori1.SelectedFor.Add(ActionType.Amorize);
                        amori2.SelectedFor.Add(ActionType.Amorize);
                    }

                    role.SelectedPlayersForAction.Clear();
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessBite(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessBite), roles.Count);
            try
            {
                if (roles.Any(x => x.ActionType != ActionType.Bite) || !roles.Any())
                {
                    return;
                }

                for (int y = 0; y < roles.Count; y++)
                {
                    string toBite = roles[y].SelectedPlayersForAction.FirstOrDefault() ?? string.Empty;
                    int index = AllPlayers.FindIndex(x => x.PlayerName == toBite && x.IsAlive);

                    if (index != -1)
                    {
                        roles[y].ActionType = ActionType.Kill;

                        AllPlayers[index].SelectedFor.Add(ActionType.Bite);
                        AllPlayers[index].Connections.Add(new Connection(ConnectionType.Bite, roles[y], AllPlayers[index]));
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void ProcessGrabrauber(List<Role> roles)
        {
            ExceptionLogger.Log(nameof(ProcessGrabrauber), roles.Count);
            try
            {
                if (!roles.Any())
                {
                    return;
                }

                foreach (Role role in roles)
                {
                    role.Connections.RemoveAll(x => x.ConnectionType == ConnectionType.StealRole && x.To == role);

                    string toStealRole = role.SelectedPlayersForAction.FirstOrDefault() ?? string.Empty;
                    Role toStealPlayer = AllPlayers.FirstOrDefault(player => player.PlayerName == toStealRole)!;

                    if (toStealPlayer != null)
                    {
                        toStealPlayer.SelectedFor.Add(ActionType.StealRole);

                        role.Connections.Add(new Connection(ConnectionType.StealRole, toStealPlayer, role));
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private string GetHighestVotedPlayer(Dictionary<string, int> voteDictionary)
        {
            ExceptionLogger.Log(nameof(GetHighestVotedPlayer), voteDictionary.Count);
            try
            {
                if (!voteDictionary.Any())
                {
                    return string.Empty;
                }

                int maxValue = voteDictionary.Values.Max();

                List<string> keysWithMaxValue = voteDictionary.Where(pair => pair.Value == maxValue)
                    .Select(pair => pair.Key)
                    .ToList();

                Random random = new Random();

                string votedPlayer = keysWithMaxValue[random.Next(keysWithMaxValue.Count)];

                return votedPlayer;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private string GetOnlyHighestVotedPlayer(Dictionary<string, int> voteDictionary)
        {
            ExceptionLogger.Log(nameof(GetOnlyHighestVotedPlayer), voteDictionary.Count);
            try
            {
                if (!voteDictionary.Any())
                {
                    return string.Empty;
                }

                int maxValue = voteDictionary.Values.Max();

                List<string> keysWithMaxValue = voteDictionary.Where(pair => pair.Value == maxValue)
                    .Select(pair => pair.Key)
                    .ToList();

                if (keysWithMaxValue.Count >= 2)
                {
                    return string.Empty;
                }

                Random random = new Random();

                string votedPlayer = keysWithMaxValue[random.Next(keysWithMaxValue.Count)];

                return votedPlayer;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void CheckGameOver()
        {
            ExceptionLogger.Log(nameof(CheckGameOver), DayCounter, NightCounter);
            try
            {
                string gameOver = CheckSpecificGameOver();
                if (!string.IsNullOrEmpty(gameOver))
                {
                    IsGameOver = true;
                    WinnerLabel = gameOver;

                    await Shell.Current.GoToAsync(nameof(GameOverPage));
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        } 

        private string CheckSpecificGameOver()
        {
            ExceptionLogger.Log(nameof(CheckSpecificGameOver), DayCounter, NightCounter);
            try
            {
                // Default Cases

                if (Player.All(x => x.Type == RoleType.Villager || x.Type == RoleType.LarryPlus))
                {
                    return "Die Dorfbewohner haben gewonnen.";
                }

                if (Player.All(x => x.Type == RoleType.Villain))
                {
                    return "Die Werwölfe haben gewonnen.";
                }


                // Special Cases

                if (Player.Count == 2)
                {
                    // Alte Schrulle hat Werwolf gemuted
                    if (Player.FirstOrDefault(x => x.Type == RoleType.Villain)?.IsAllowedToVote == false && Player.Any(x => x.RoleName != nameof(Data.Werwolf)))
                    {
                        return "Die Dorfbewohner haben gewonnen.";
                    }

                    // Rächer nimmt Werwolf mit in den Tod
                    if (Player.All(x => x.IsAllowedToVote && Player.Any(y => y.RoleName == nameof(Raecher))))
                    {
                        return "Unentschieden. Alle Spieler sind gestorben.";
                    }

                    // Couple lebt und gewinnt
                    if (Player.All(x => x.Connections.Any(y => y.ConnectionType == ConnectionType.Couple)))
                    {
                        return "Das Liebespaar hat gewonnen.";
                    }
                }

                // Werwölfe killen in der Nacht den Dorfbewohner -> Ende 
                if (Player.Count(x => x.Type != RoleType.Villain) == 1)
                {
                    return "Die Werwölfe haben gewonnen.";
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void Kill(Role roleToAdd)
        {
            if (roleToAdd == null)
            {
                return;
            }

            ExceptionLogger.Log(nameof(Kill), roleToAdd.PlayerName);
            try
            {
                if (!_nextDeadPlayers.Contains(roleToAdd))
                {
                    _nextDeadPlayers.Add(roleToAdd);
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        public async void Restart()
        {
            ExceptionLogger.Log(nameof(Restart));
            try
            {
                RestartGameStats();

                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private void RestartGameStats()
        {
            try
            {
                _player.Clear();
                Player.Clear();
                Roles.Clear();
                DeadPlayers.Clear();
                _nextDeadPlayers.Clear();
                IsGameOver = false;
                _nightCount = 1;
                _dayCount = 1;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }
    }
}
