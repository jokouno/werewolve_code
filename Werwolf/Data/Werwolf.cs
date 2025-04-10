
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Werwolf : Role
    {
        private const string _name = nameof(Werwolf);
        private const string _text = "Wähle einen Spieler aus, der umgebracht werden" +
                                     "soll. Die anderen Werwölfe können deine Wahl sehen." +
                                     "Bei Stimmengleichheit, wird ein zufälliges " +
                                     "Opfer ausgewählt.";
        private const string _avatar = "werwolf.png";

        public Werwolf()
        {
            Initialize(this);
            DiesToo = new List<Role>();
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Werwolf(Role role)
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

        public static Werwolf Initialize(Werwolf role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.Villain;
            role.IsAlive = true;
            role.HasPlayerSelection = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.VisibleForTeam;
            role.ActionType = ActionType.Kill;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;
            role.HasMultiplePlayerSelection = false;

            role.Actions = new List<PlayerAction>
            {
                new Kill()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }

        protected override Role CreateInstance()
        {
            return new Werwolf();
        }
    }
}
