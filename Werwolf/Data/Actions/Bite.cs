
namespace Werwolf.Data.Actions
{
    public class Bite : PlayerAction
    {
        private const string _name = nameof(Bite);
        private const string _picture = "bite.svg";
        public Bite()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.Bite;
        }
    }
}
