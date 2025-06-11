
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class RoleSelectionViewModel : ObservableObject
    {
        private List<Role> _roles;
        private List<Role> _allRoles;
        private int _playerCount;
        private GameManager _gameManager;

        public static List<Role>? SelectedRoles;

        #region Counts

        [ObservableProperty]
        public int dorfbewohnerCount;

        [ObservableProperty]
        public int werwolfCount;

        [ObservableProperty]
        public int doctorCount;

        [ObservableProperty]
        public int hexeCount;

        [ObservableProperty]
        public int raecherCount;

        [ObservableProperty]
        public int alteSchrulleCount;

        [ObservableProperty]
        public int seherinCount;

        [ObservableProperty]
        public int amorCount;

        [ObservableProperty]
        public int kittenWerwolfCount;

        [ObservableProperty]
        public int grabrauberCount;

        #endregion

        [ObservableProperty]
        public bool isStartGameButtonEnabled;


        public List<Role> Roles => _roles;
        public List<Role> AllRoles => _allRoles;

        public int playerCount
        {
            get => _playerCount;
            set => SetProperty(ref _playerCount, value);
        }

        public RoleSelectionViewModel(GameManager gameManager)
        {
            _gameManager = gameManager;
            _playerCount = _gameManager.PlayerEntries.Count;
            _roles = new List<Role>();
            _allRoles = GameManager.AllRoles;
            Dorfbewohner dorfbewohner = new Dorfbewohner();
            dorfbewohner.Count = playerCount;

            Roles.Add(dorfbewohner);
            RefreshCounts();
        }

        [RelayCommand]
        public void AddPlayerToRole(string name)
        {
            if (DorfbewohnerCount == 0)
            {
                return;
            }

            if (Roles.Any(x => x.RoleName == name))
            {
                Roles.FirstOrDefault(x => x.RoleName == name)!.Count++;
            }
            else
            {
                AddRole(name);
            }

            Roles.FirstOrDefault(x => x.RoleName == "Dorfbewohner")!.Count--;
            RefreshCounts();
        }

        [RelayCommand]
        public void DeletePlayerToRole(string name)
        {
            if (Roles.Any(x => x.RoleName == name))
            {
                if (Roles.FirstOrDefault(x => x.RoleName == name)!.Count <= 1)
                {
                    Roles.Remove(Roles.FirstOrDefault(x => x.RoleName == name)!);
                }
                else
                {
                    Roles.FirstOrDefault(x => x.RoleName == name)!.Count--;
                }
                Roles.FirstOrDefault(x => x.RoleName == "Dorfbewohner")!.Count++;
                RefreshCounts();
            }
        }

        [RelayCommand]
        public void StartGame()
        {
            _gameManager.StartGame(Roles);
        }


        [RelayCommand]
        public void StartRandomRolesGame()
        {
            _gameManager.StartRandomRolesGame();
        }

        #region Private methods

        private void AddRole(string name)
        {
            Role newRole = AllRoles.FirstOrDefault(x => x.RoleName == name)!;

            if (newRole != null)
            {
                _roles.Add(newRole);
            }
        }

        private int GetVillainCount()
        {
            int villainCount = 0;

            foreach (Role role in Roles)
            {
                if (role.Type == RoleType.Villain || role.Type == RoleType.SoloWinner)
                {
                    villainCount += role.Count;
                }
            }
            return villainCount;
        }

        private int GetVillagerCount()
        {
            int villagerCount = 0;

            foreach (Role role in Roles)
            {
                if (role.Type == RoleType.Villager || role.Type == RoleType.LarryPlus)
                {
                    villagerCount += role.Count;
                }
            }
            return villagerCount;
        }

        private void RefreshCounts()
        {
            DorfbewohnerCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Dorfbewohner))?.Count ?? 0;
            WerwolfCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Data.Werwolf))?.Count ?? 0;
            DoctorCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Doctor))?.Count ?? 0;
            HexeCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Hexe))?.Count ?? 0;
            RaecherCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Raecher))?.Count ?? 0;
            AlteSchrulleCount = Roles.FirstOrDefault(x => x.RoleName == nameof(AlteSchrulle))?.Count ?? 0;
            SeherinCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Seherin))?.Count ?? 0;
            AmorCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Amor))?.Count ?? 0;
            KittenWerwolfCount = Roles.FirstOrDefault(x => x.RoleName == nameof(KittenWerwolf))?.Count ?? 0;
            GrabrauberCount = Roles.FirstOrDefault(x => x.RoleName == nameof(Grabrauber))?.Count ?? 0;

            int villainCount = GetVillainCount();
            int villagerCount = GetVillagerCount();

            IsStartGameButtonEnabled = villainCount <= villagerCount - 2 && (villainCount >= 1);
        }

        #endregion
    }
}
