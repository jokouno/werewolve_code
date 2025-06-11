
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public abstract class Role
    {
        private const string PlayerAvatarPng = "player.png";
        public string RoleName;
        public int Count;
        public RoleType Type;
        public string PlayerName;
        public bool IsAlive;
        public bool HasPlayerSelection;
        public bool HasActionSelection;
        public string Text;
        public string Avatar;
        public string PlayerAvatar = PlayerAvatarPng;
        public TeamVisability Visability;
        public List<string> SelectedPlayersForAction;
        public List<PlayerAction> Actions;
        public ActionType ActionType;
        public bool IsAllowedToVote;
        public int VotedByCount;
        public bool HasUsedOneTimeAction;
        public bool HasMultiplePlayerSelection;
        public bool IsOneTimeInfoHasBeenShown;
        public List<Connection> Connections = new List<Connection>();
        public List<ActionType> SelectedFor = new List<ActionType>();

        protected abstract Role CreateInstance();

        public List<Role> Start()
        {
            List<Role> players = new List<Role>();
            for (int count = 0; count < Count; count++)
            {
                players.Add(CreateInstance());
            }

            return players;
        }

        public void Reset()
        {
            this.HasPlayerSelection = false;
            this.HasActionSelection = false;
            this.SelectedPlayersForAction.Clear();
            this.Actions.Clear();
            this.IsAllowedToVote = true;
            this.HasUsedOneTimeAction = false;
            this.HasMultiplePlayerSelection = false;
            this.IsOneTimeInfoHasBeenShown = false;
        }

        public abstract void DoAction(List<string> playerNames, ActionType actionType);
    }
}
