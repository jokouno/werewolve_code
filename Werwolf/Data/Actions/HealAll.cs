namespace Werwolf.Data.Actions
{
    public class HealAll : PlayerAction
    {
        private const string _name = nameof(HealAll);
        private const string _picture = "heal.svg";
        public HealAll()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.HealAll;
        }
    }
}