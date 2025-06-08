using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Hexe : Role
    {
        private const string _name = nameof(Hexe);
        private const string _text = "Verwende das Gift, um einen Spieler umzubringen, " +
                                     "oder den Heiltrank, um das Opfer der Werwölfe in " +
                                     "dieser Nacht zu retten. Du hast beide Tränke nur einmal.";
        private const string _avatar = "hexe.png";

        public Hexe()
        {
            Initialize(this);
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Hexe(Role role)
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

        public static Hexe Initialize(Hexe role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.Villager;
            role.IsAlive = true;
            role.HasPlayerSelection = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.None;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = true;

            role.Actions = new List<PlayerAction>
            {
                new HealAll(),
                new InstantKill()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
            ActionType = actionType;

            Actions.RemoveAll(x => x.ActionType == actionType);
        }


        protected override Role CreateInstance()
        {
            return new Hexe();
        }
    }
}