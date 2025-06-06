
using Werwolf.Data;
using Werwolf.Data.Actions;
using Werwolf.ViewModel;
using Microsoft.Maui.Controls;

namespace Werwolf.Workflow
{
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

        public async void StartRandomRolesGame()
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

        public Role GetRandomRole()
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

        public async void NextRole()
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

        public async void NextPlayerVote()
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

        public async void EndPlayerVote()
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

                    ProcessNewDeadPlayer();
                }
            }

            DeadPlayerPageType = DeadPlayerPageType.DeadByVoting;

            Player.ForEach(x => x.IsAllowedToVote = true);
            Player.ForEach(x => x.VotedByCount = 0);
            _villageVotings.Clear();

            await Shell.Current.GoToAsync(nameof(DayDeadPlayerPage));
        }

        public void VotForVillagerElection(List<RolePresentation> roles)
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

        public void SetNames(List<string> names)
        {
            _names = names;
        }

        public void InitializeRoles()
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

        public async void OpenRole()
        {
            if (CurrentPlayer.Connections.Any(x => x == Connection.Couple || x == Connection.Bite) 
                && !CurrentPlayer.IsOneTimeInfoHasBeenShown)
            {
                if (CurrentPlayer.Connections.Any(x => x == Connection.Couple))
                {
                    CurrentPlayer.SelectedPlayersForAction =
                        Player.Where(x => x.Connections.Any(y => y == Connection.Couple) && x.PlayerName != CurrentPlayer.PlayerName).
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

        public async void ShowAnotherInfo(string info)
        {
            NextPlayerInfo = info;
            await Shell.Current.GoToAsync(nameof(SpecialPowerPage));
        }
        public void StartNightMenu()
        {
            NextRole();
        }

        public async void EndNight()
        {
            DeadPlayers.Clear();
            _nextDeadPlayers.Clear();
            ProcessNight();

            await Shell.Current.GoToAsync(nameof(DayBeginningPage));
        }

        public async void RevealDeadPlayers()
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

        public async void StartDiscussionInfo()
        {
            await Shell.Current.GoToAsync(nameof(DiscussionPage));
        }

        public async void StartDiscussion()
        {
            CurrentPlayerCount = -1;
            await Shell.Current.GoToAsync(nameof(DiscussionTimerPage));
        }

        public async void StartNight()
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

        public async void DoSpecialPower()
        {
            await Shell.Current.GoToAsync(nameof(SeherPage));
        }

        public void ProcessAction()
        {

        }

        public void ProcessNight()
        {
            // ----------- Process all kill categories -----------

            // Instant Kill
            List<Role> instantKillRoles = Player.Where(x => x.ActionType == ActionType.InstantKill).ToList();
            ProcessInstantKill(instantKillRoles);

            // Kill
            List<Role> killRoles = Player.Where(x => x.ActionType == ActionType.Kill).ToList();
            ProcessKill(killRoles);

            // Revenge Kill
            List<Role> revengeKills = _player.Where(x => x.ActionType == ActionType.RevengeKill).ToList();
            ProcessRevengeKill(revengeKills);

            // Dead Player | Couple dies | Revenge Kill 
            List<Role> newDeadPlayers = ProcessDeadPlayers();
            AddDeadPlayers(newDeadPlayers);

            // ----------- Process all heal categories -----------

            // Heal
            List<Role> healRoles = Player.Where(x => x.ActionType == ActionType.Heal).ToList();
            ProcessHeal(healRoles);

            // Heal All
            List<Role> healAll = Player.Where(x => x.ActionType == ActionType.HealAll).ToList();
            ProcessHealAll(healAll);

            // ----------- No one dies after this line -----------
            // ----------- Process special categories  -----------

            // Amorize
            List<Role> amorized = _player.Where(x => x.ActionType == ActionType.Amorize).ToList();
            ProcessAmorize(amorized);

            ProcessNewDeadPlayer();

            // NotAllowedToVote
            List<Role> noVoteAllowed = _player.Where(x => x.ActionType == ActionType.NoVoteAllowed).ToList();
            ProcessNoVoteAllowed(noVoteAllowed);

            // Bite -> auch dead Player bearbeiten
            List<Role> bites = _player.Where(x => x.ActionType == ActionType.Bite).ToList();
            ProcessBite(bites);

            // ----------- must be alive roles to do action  -----------
            
            CheckGameOver();
            Player.ForEach(x => x.VotedByCount = 0);
            Player.Where(x => x.ActionType != ActionType.Heal && x.ActionType != ActionType.RevengeKill).ToList().ForEach(x => x.SelectedPlayersForAction = Enumerable.Empty<string>().ToList());

            foreach (Role hexe in Player.Where(x => x.RoleName == nameof(Hexe)))
            {
                hexe.ActionType = ActionType.None;
            }
        }

        public void ProcessNewDeadPlayer()
        {
            try
            {
                List<Role> newDeadPlayers = ProcessDeadPlayers();
                int count = 0;
                while (newDeadPlayers.Any() && count < 5)
                {
                    AddDeadPlayers(newDeadPlayers);
                    newDeadPlayers = new List<Role>();
                    newDeadPlayers = ProcessDeadPlayers();
                    count++;
                }

                _deadPlayers = _nextDeadPlayers;
                _deadPlayers.ForEach(x => x.IsAlive = false);
            }
            catch (Exception)
            {
                Console.WriteLine("Error beim ProcessNewDeadPlayer");
            }

            // Grabräuber
            List<Role> grabRaubers = Player.Where(x => x.ActionType == ActionType.StealRole).ToList();
            ProcessGrabrauber(grabRaubers);
        }

        private void ProcessInstantKill(List<Role> roles)
        {
            if (roles.Any(x => x.ActionType != ActionType.InstantKill) || !roles.Any())
            {
                return;
            }

            foreach (Role player in roles)
            {
                foreach (string toAction in player.SelectedPlayersForAction)
                {
                    AddDeadPlayers(Player.Where(x => x.PlayerName == toAction).ToList());
                }
            }
        }

        private void ProcessKill(List<Role> roles)
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
                Role votedToKill = Player.FirstOrDefault(x => x.PlayerName == votedPlayer)!;
                Kill(votedToKill);
            }
        }

        private void ProcessRevengeKill(List<Role> roles)
        {
            if (roles.Any(x => x.ActionType != ActionType.RevengeKill) || !roles.Any())
            {
                return;
            }

            foreach (Role role in roles)
            {
                role.DiesToo = new List<Role>();

                foreach (string toDieToo in role.SelectedPlayersForAction)
                {
                    Role toRevengeDie = Player.FirstOrDefault(x => x.PlayerName == toDieToo)!;

                    if (toRevengeDie != null)
                    {
                        role.DiesToo.Add(toRevengeDie);
                    }
                }
            }
        }

        private void ProcessHeal(List<Role> roles)
        {
            if (roles.Any(x => x.ActionType != ActionType.Heal) || !roles.Any())
            {
                return;
            }

            foreach (Role player in roles)
            {
                foreach (string toAction in player.SelectedPlayersForAction)
                {
                    Role toHeal = _player.FirstOrDefault(x => x.PlayerName == toAction)!;

                    if (toHeal != null)
                    {
                        toHeal.IsAlive = true;

                        foreach (Role toSaveToo in toHeal.DiesToo)
                        {
                            toSaveToo.IsAlive = true;
                            _nextDeadPlayers.RemoveAll(x => x.PlayerName == toSaveToo.PlayerName);
                        }
                    }

                    _nextDeadPlayers.RemoveAll(x => x.PlayerName == toAction);
                }
            }
        }

        private void ProcessNoVoteAllowed(List<Role> roles)
        {
            if (roles.Any(x => x.ActionType != ActionType.NoVoteAllowed) || !roles.Any())
            {
                return;
            }

            foreach (Role player in roles)
            {
                foreach (string toAction in player.SelectedPlayersForAction)
                {
                    Role notAllowedToVote = _player.FirstOrDefault(x => x.PlayerName == toAction)!;

                    if (notAllowedToVote != null)
                    {
                        notAllowedToVote.IsAllowedToVote = false;
                    }
                }
                player.SelectedPlayersForAction.Clear();
            }
        }

        private void ProcessHealAll(List<Role> roles)
        {
            if (roles.Any(x => x.ActionType != ActionType.HealAll) || !roles.Any())
            {
                return;
            }

            _nextDeadPlayers.ForEach(x => x.IsAlive = true);
            _nextDeadPlayers.Clear();
        }

        private void ProcessAmorize(List<Role> roles)
        {
            if (roles.Any(x => x.ActionType != ActionType.Amorize) || !roles.Any())
            {
                return;
            }

            foreach (Role role in roles)
            {
                if (!role.IsAlive)
                {
                    continue;
                }
                Role amori1 = _player.FirstOrDefault(x => x.PlayerName == role.SelectedPlayersForAction[0])!;
                Role amori2 = _player.FirstOrDefault(x => x.PlayerName == role.SelectedPlayersForAction[1])!;

                if (amori1 != null && amori2 != null)
                {
                    amori1.Connections.Add(Connection.Couple);
                    amori2.Connections.Add(Connection.Couple);

                    amori1.DiesToo.Add(amori2);
                    amori2.DiesToo.Add(amori1);
                }

                role.SelectedPlayersForAction.Clear();
            }
        }

        private void ProcessBite(List<Role> roles)
        {
            if (roles.Any(x => x.ActionType != ActionType.Bite) || !roles.Any())
            {
                return;
            }

            for (int y = 0; y < roles.Count; y++)
            {
                string toBite = roles[y].SelectedPlayersForAction.FirstOrDefault() ?? string.Empty;
                int index = _player.FindIndex(x => x.PlayerName == toBite && x.IsAlive);

                if (_nextDeadPlayers.All(deadPlayer => deadPlayer.PlayerName != toBite) && index != -1)
                {
                    Data.Werwolf bittenOne = new Data.Werwolf(_player[index]);

                    bittenOne.Connections.Add(Connection.Bite);

                    _player.RemoveAt(index);
                    _player.Insert(index, bittenOne);

                    roles[y].ActionType = ActionType.Kill;
                }
            }
        }

        private List<Role> ProcessDeadPlayers()
        {
            if (!_nextDeadPlayers.Any())
            {
                return Enumerable.Empty<Role>().ToList();
            }

            List<Role> newDeadPlayer = new List<Role>();

            foreach (Role deadPlayer in _nextDeadPlayers)
            {
                if (deadPlayer.Connections != null)
                {
                    if (deadPlayer.Connections.Any(x => x == Connection.Couple))
                    {
                        if (deadPlayer.DiesToo != null)
                        {
                            foreach (Role role in deadPlayer.DiesToo)
                            {
                                if (!_nextDeadPlayers.Contains(role))
                                {
                                    newDeadPlayer.AddRange(deadPlayer.DiesToo);
                                }
                            }
                        }
                    }
                }

                if (deadPlayer.ActionType == ActionType.RevengeKill)
                {
                    if (deadPlayer.DiesToo != null)
                    {
                        foreach (Role role in deadPlayer.DiesToo)
                        {
                            if (!_nextDeadPlayers.Contains(role))
                            {
                                newDeadPlayer.AddRange(deadPlayer.DiesToo);
                            }
                        }
                    }
                }
            }

            return newDeadPlayer;
        }

        private void ProcessGrabrauber(List<Role> roles)
        {
            if (!roles.Any())
            {
                return;
            } 

            foreach (Role role in roles)
            {
                string toStealRole = role.SelectedPlayersForAction.FirstOrDefault() ?? string.Empty;
                Role toStealPlayer = _nextDeadPlayers.FirstOrDefault(deadPlayer => deadPlayer.PlayerName == toStealRole)!;

                if (toStealRole != null)
                {
                    // Player selected is dead -> steal role

                    Role stolen;

                    switch (toStealPlayer)
                    {
                        case AlteSchrulle:
                            stolen = new AlteSchrulle(role);
                            break;
                        case Dorfbewohner:
                            stolen = new Dorfbewohner(role);
                            break;
                        case Doctor:
                            stolen = new Doctor(role);
                            break;
                        case Hexe:
                            stolen = new Hexe(role);
                            break;
                        case Raecher:
                            stolen = new Raecher(role);
                            break;
                        case Seherin:
                            stolen = new Seherin(role);
                            break;
                        case Data.Werwolf:
                            stolen = new Data.Werwolf(role);
                            break;
                        case Amor:
                            stolen = new Amor(role);
                            break;
                        case KittenWerwolf:
                            stolen = new KittenWerwolf(role);
                            break;
                        case Grabrauber:
                            stolen = new Grabrauber(role);
                            break;
                        default:
                            stolen = null!;
                            break;
                    }

                    if (stolen != null)
                    {
                        int index = _player.FindIndex(x => x.PlayerName == role.PlayerName && x.IsAlive);

                        if (index != -1)
                        {
                            // connection hinzufügen für Info page

                            _player.RemoveAt(index);
                            _player.Insert(index, stolen);
                        }
                    }
                }
            }
        }

        private string GetHighestVotedPlayer(Dictionary<string, int> voteDictionary)
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

        private string GetOnlyHighestVotedPlayer(Dictionary<string, int> voteDictionary)
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

        public async void CheckGameOver()
        {
            string gameOver = CheckSpecificGameOver();
            if (!string.IsNullOrEmpty(gameOver))
            {
                IsGameOver = true;
                WinnerLabel = gameOver;

                await Shell.Current.GoToAsync(nameof(GameOverPage));
            }
        } 

        private string CheckSpecificGameOver()
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
                if (Player.All(x => x.Connections.Any(y => y == Connection.Couple)))
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

        private void AddDeadPlayers(List<Role> rolesToAdd)
        {
            foreach (Role roleToAdd in rolesToAdd)
            {
                Kill(roleToAdd);
            }
        }

        private void Kill(Role roleToAdd)
        {
            if (roleToAdd == null)
            {
                return;
            }

            if (!_nextDeadPlayers.Contains(roleToAdd))
            {
                _nextDeadPlayers.Add(roleToAdd);
            }
        }

        public async void Restart()
        {
            RestartGameStats();

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        private void RestartGameStats()
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
    }
}
