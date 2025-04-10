namespace Werwolf.Data.Actions
{
    public class Kill : PlayerAction
    {
        private const string _name = nameof(Kill);
        private const string _picture = "skull.svg";
        public Kill()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.Kill;
        }
    }
}