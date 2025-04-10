
using System.Data;
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class AlteSchrulle : Role
    {
        private const string _name = nameof(AlteSchrulle);
        private const string _text = "Wähle einen Spieler aus, der tagsüber in der Diskussion nicht abstimemen darf.";
        private const string _avatar = "alte_schrulle.png";

        public AlteSchrulle()
        {
            Initialize(this);
            DiesToo = new List<Role>();
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public AlteSchrulle(Role role)
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

        public static AlteSchrulle Initialize(AlteSchrulle role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.LarryPlus;
            role.IsAlive = true;
            role.HasPlayerSelection = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.NoVoteAllowed;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;

            role.Actions = new List<PlayerAction>
            {
                new NoVote()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }

        protected override Role CreateInstance()
        {
            return new AlteSchrulle();
        }
    }
}
