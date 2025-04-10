
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class Dorfbewohner : Role
    {
        private const string _name = nameof(Dorfbewohner);
        private const string _text = "Tagsüber disskutierst du mit den anderen Mitspielern, " +
                                     "wer ein Werwolf sein könnte und wählst," +
                                     "gemeinsam mit den anderen Spielern, " +
                                     "einen Spieler aus, der umgebracht werden soll";
        private const string _avatar = "dorfbewohner.png";

        public Dorfbewohner()
        {
            Initialize(this);
            DiesToo = new List<Role>();
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public Dorfbewohner(Role role)
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

        public static Dorfbewohner Initialize(Dorfbewohner role)
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
            return new Dorfbewohner();
        }
    }
}
