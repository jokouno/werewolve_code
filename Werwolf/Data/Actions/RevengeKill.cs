namespace Werwolf.Data.Actions
{
    public class RevengeKill : PlayerAction
    {
        private const string _name = nameof(RevengeKill);
        private const string _picture = "";
        public RevengeKill()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.RevengeKill;
        }
    }
}