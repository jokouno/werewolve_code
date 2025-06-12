
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Grabrauber : Role
    {
        private const string _name = nameof(Grabrauber);
        private const string _text = "Wähle einen Spieler aus, dessen Rolle du übernimmst, sollte dieser Spieler sterben.";
        private const string _avatar = "grabraeuber.webp";

        public Grabrauber()
        {
            Initialize(this);
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Grabrauber(Role role)
        {
            foreach (var field in typeof(Role).GetFields(
                         System.Reflection.BindingFlags.Public |
                         System.Reflection.BindingFlags.NonPublic |
                         System.Reflection.BindingFlags.Instance
                     ))
            {
                var value = field.GetValue(role);
                field.SetValue(this, value);
            }
            role.Reset();
            this.Reset();
            Initialize(this);
        }

        public static Grabrauber Initialize(Grabrauber role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.LarryPlus;
            role.IsAlive = true;
            role.HasPlayerSelection = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.StealRole;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;

            role.Actions = new List<PlayerAction>
            {
                new StealRole()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }

        protected override Role CreateInstance()
        {
            return new Grabrauber();
        }
    }
}
