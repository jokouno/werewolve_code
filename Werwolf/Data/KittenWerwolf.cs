
using Werwolf.Data.Actions;

namespace Werwolf.Data
{
    public class KittenWerwolf : Role
    {
        private const string _name = nameof(KittenWerwolf);

        private const string _text = "Du kannst einmalig einen beliebigen Spieler beißen. " +
                                     "Dieser wird dann zum Werwolf. Anonsten stimmst du jede Nacht für einen Spieler ab, " +
                                     "der umgebracht werden soll.";

        private const string _avatar = "kittenwerwolf.webp";

        public KittenWerwolf()
        {
            Initialize(this);
            DiesToo = new List<Role>();
            Connections = new List<Connection>();
            IsAllowedToVote = true;
        }

        public KittenWerwolf(Role role)
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

        public static KittenWerwolf Initialize(KittenWerwolf role)
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
            role.HasActionSelection = true;

            role.Actions = new List<PlayerAction>
            {
                new Bite(),
                new Kill()
            };

            return role;
        }

        public override void DoAction(List<string> playerNames, ActionType actionType)
        {
            SelectedPlayersForAction = playerNames;
            ActionType = actionType;

            if (actionType == ActionType.Bite)
            {
                Actions.RemoveAll(x => x.ActionType == actionType);
                HasActionSelection = false;
            }
        }


        protected override Role CreateInstance()
        {
            return new KittenWerwolf();
        }
    }
}