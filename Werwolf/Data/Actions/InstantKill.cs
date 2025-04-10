namespace Werwolf.Data.Actions
{
    public class InstantKill : PlayerAction
    {
        private const string _name = nameof(InstantKill);
        private const string _picture = "skull.svg";
        public InstantKill()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.InstantKill; 
        }
    }
}
