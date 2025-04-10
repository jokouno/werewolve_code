namespace Werwolf.Data.Actions
{
    public class Reveal : PlayerAction
    {
        private const string _name = nameof(Reveal);
        private const string _picture = "skull.svg";
        public Reveal()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.Reveal;
        }
    }
}