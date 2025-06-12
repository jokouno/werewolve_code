
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Raecher : Role
    {
        private const string _name = nameof(Raecher);
        private const string _text = "Wähle einen Spieler aus, den du mit in den Tod reißen möchtest, falls du stirbst.";
        private const string _avatar = "raecher.png";

        public Raecher()
        {
            Initialize(this);
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Raecher(Role role)
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

        public static Raecher Initialize(Raecher role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.LarryPlus;
            role.IsAlive = true;
            role.HasPlayerSelection = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.RevengeKill;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;

            role.Actions = new List<PlayerAction>
            {
                new RevengeKill()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }

        protected override Role CreateInstance()
        {
            return new Raecher();
        }
    }
}
