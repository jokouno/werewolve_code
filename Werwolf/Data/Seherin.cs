
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Seherin : Role
    {
        private const string _name = nameof(Seherin);
        private const string _text = "Wähle einen Spieler aus, dessen Rolle du gerne aufdecken möchtest.";
        private const string _avatar = "seherin.webp";

        public Seherin()
        {
            Initialize(this);
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Seherin(Role role)
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
            Initialize(this);
        }

        public static Seherin Initialize(Seherin role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.LarryPlus;
            role.IsAlive = true;
            role.HasPlayerSelection = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.Reveal;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;

            role.Actions = new List<PlayerAction>
            {
                new Reveal()
            };

            return role;
        }

        protected override Role CreateInstance()
        {
            return new Seherin();
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }
    }
}
