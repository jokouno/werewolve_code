namespace Werwolf.Data.Actions
{
    public class NoVote : PlayerAction
    {
        private const string _name = nameof(NoVote);
        private const string _picture = "";
        public NoVote()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.NoVoteAllowed;
        }
    }
}