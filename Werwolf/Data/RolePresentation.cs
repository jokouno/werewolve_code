
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Workflow;

namespace Werwolf.Data
{
    public partial class RolePresentation : ObservableObject
    {
        [ObservableProperty] public string name;
        [ObservableProperty] public int count;
        [ObservableProperty] public RoleType type;
        [ObservableProperty] public string playerName;
        [ObservableProperty] public bool isAlive;
        [ObservableProperty] public bool iSNotAlive;
        [ObservableProperty] public bool hasPlayerSelection;
        [ObservableProperty] public bool hasActionSelection;
        [ObservableProperty] public string text;
        [ObservableProperty] public string avatar;
        [ObservableProperty] public string defaultAvatar;
        [ObservableProperty] public string activeAvatar;
        [ObservableProperty] public TeamVisability visability;
        [ObservableProperty] public bool isAllowedToVote;
        [ObservableProperty] public static List<RolePresentation> allPlayer;
        [ObservableProperty] public int votedByCount;
        [ObservableProperty] public bool isSameTeam;
        [ObservableProperty] public bool isNotSameTeam;
        [ObservableProperty] public bool isWinner;
        [ObservableProperty] public bool isCouple;
        [ObservableProperty] public bool isBite;

        public static List<RolePresentation> Clone(List<Role> roles)
        {
            List<RolePresentation> presentation = new List<RolePresentation>();

            foreach (Role role in roles)
            {
                presentation.Add(Clone(role));
            }

            allPlayer = presentation;

            return presentation;
        }

        public static RolePresentation Clone(Role role)
        {
            RolePresentation presentation = new RolePresentation
            {
                name = role.RoleName,
                count = role.Count,
                type = role.Type,
                playerName = role.PlayerName,
                isAlive = role.IsAlive,
                iSNotAlive = !role.IsAlive,
                hasPlayerSelection = role.HasPlayerSelection,
                HasActionSelection = role.HasActionSelection,
                text = role.Text,
                avatar = role.Avatar,
                defaultAvatar = role.PlayerAvatar,
                activeAvatar = role.PlayerAvatar,
                Visability = role.Visability,
                isAllowedToVote = role.IsAllowedToVote,
                VotedByCount = role.VotedByCount,
                IsSameTeam = false,
                IsNotSameTeam = true,
                IsWinner = false
            };

            if (role.Connections.Any(x => x.ConnectionType == ConnectionType.Couple))
            {
                presentation.IsCouple = true;
            }
            if (role.Connections.Any(x => x.ConnectionType == ConnectionType.Bite))
            {
                presentation.IsBite = true;
            }

            return presentation;
        }

        [RelayCommand]
        public static void RevealAvatar(string playerName)
        {
            RolePresentation player = allPlayer.FirstOrDefault(x => x.playerName == playerName)!;

            if (player == null)
            {
                return;
            }

            player.ActiveAvatar = player.Avatar;
        }

        [RelayCommand]
        public static void RevealSameTeamAvatar(string playerName)
        {
            RolePresentation player = allPlayer.FirstOrDefault(x => x.playerName == playerName)!;

            if (player == null)
            {
                return;
            }

            if (player.Visability == TeamVisability.VisibleForTeam)
            {
                foreach (RolePresentation rolePresentation in allPlayer)
                {
                    if (rolePresentation.Name == player.Name)
                    {
                        rolePresentation.ActiveAvatar = rolePresentation.Avatar;
                        rolePresentation.IsSameTeam = true;
                        rolePresentation.IsNotSameTeam = false;
                    }
                    else if (rolePresentation.Type == RoleType.Villain && player.Type == RoleType.Villain)
                    {
                        rolePresentation.ActiveAvatar = rolePresentation.Avatar;
                        rolePresentation.IsSameTeam = true;
                        rolePresentation.IsNotSameTeam = false;
                    }
                    else if (rolePresentation.Visability == TeamVisability.VisibleForAll)
                    {
                        rolePresentation.ActiveAvatar = rolePresentation.Avatar;
                    }
                    else
                    {
                        rolePresentation.IsSameTeam = false;
                        rolePresentation.IsNotSameTeam = true;
                    }
                }
            }
        }

        [RelayCommand]
        public static void HideAllAvatars()
        {
            foreach (RolePresentation rolePresentation in allPlayer)
            {
                rolePresentation.ActiveAvatar = rolePresentation.DefaultAvatar;
            }
        }

        [RelayCommand]
        public void SetSameTeam(Role currentPlayer)
        {
            IsSameTeam = currentPlayer.RoleName == Name;
            IsNotSameTeam = !IsSameTeam;
        }
    }
}
