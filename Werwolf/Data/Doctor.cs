
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Doctor : Role
    {
        private const string _name = nameof(Doctor);
        private const string _text = "Wähle einen Spieler aus, den du beschützen möchtest. Du kannst nicht in zwei aufeinander folgenden Runden den selben Spieler beschützen.";
        private const string _avatar = "doctor.png";

        public Doctor()
        {
            Initialize(this);
            DiesToo = new List<Role>();
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Doctor(Role role)
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

        public static Doctor Initialize(Doctor role)
        {
            role.RoleName = _name;
            role.Count = 1;
            role.Type = RoleType.LarryPlus;
            role.IsAlive = true;
            role.HasPlayerSelection = true;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.Heal;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;

            role.Actions = new List<PlayerAction>
            {
                new Heal()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }

        protected override Role CreateInstance()
        {
            return new Doctor();
        }
    }
}
