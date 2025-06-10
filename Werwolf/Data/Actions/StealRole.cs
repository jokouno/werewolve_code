
namespace Werwolf.Data.Actions
{
    public class StealRole : PlayerAction
    {
        private const string _name = nameof(StealRole);
        private const string _picture = "bite.svg";
        public StealRole()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.StealRole;
        }
    }
}
