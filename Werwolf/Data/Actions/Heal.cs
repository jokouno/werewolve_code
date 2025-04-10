namespace Werwolf.Data.Actions
{
    public class Heal : PlayerAction
    {
        private const string _name = nameof(Heal);
        private const string _picture = "heal.svg";
        public Heal()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.Heal;
        }
    }
}