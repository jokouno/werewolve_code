
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Dorfoberhaupt : Role
    {
        private const string _name = nameof(Dorfoberhaupt);
        private const string _text = "Als Dorfoberhaupt bist du ein ganz normaler Dorfbewohner, aber deine Stimme zählt bei der Tagwahl doppelt.";
        private const string _avatar = "crown.svg";

        public override int VillageVoteWeight => 2;

        public Dorfoberhaupt()
        {
            Initialize(this);
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Dorfoberhaupt(Role role)
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

        public static Dorfoberhaupt Initialize(Dorfoberhaupt role)
        {
            role.RoleName = _name;
            role.Count = 0;
            role.Type = RoleType.Villager;
            role.IsAlive = true;
            role.HasPlayerSelection = false;
            role.Text = _text;
            role.Avatar = _avatar;
            role.Visability = TeamVisability.NotVisible;
            role.ActionType = ActionType.None;
            role.SelectedPlayersForAction = new List<string>();
            role.HasActionSelection = false;

            role.Actions = Enumerable.Empty<PlayerAction>().ToList();

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
        }

        protected override Role CreateInstance()
        {
            return new Dorfoberhaupt();
        }
    }
}
