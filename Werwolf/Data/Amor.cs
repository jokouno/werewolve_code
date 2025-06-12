using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Amor : Role
    {
        private const string _name = nameof(Amor);
        private const string _text = "Wähle zwei Spieler aus, die sich dann in einander verlieben. Das Liebespaar kann dann nur noch zu zweit gewinnen.";
        private const string _avatar = "amor.png";

        public Amor()
        {
            Initialize(this);
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Amor(Role role)
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

        public static Amor Initialize(Amor role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.LarryPlus;
            role.IsAlive = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.Amorize;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;
            role.HasPlayerSelection = false;
            role.HasMultiplePlayerSelection = true;
            role.HasUsedOneTimeAction = false;

            role.Actions = new List<PlayerAction>
            {
                new Amorize()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }

        protected override Role CreateInstance()
        {
            return new Amor();
        }
    }
}
