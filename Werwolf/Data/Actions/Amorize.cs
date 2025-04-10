
namespace Werwolf.Data.Actions
{
    public class Amorize : PlayerAction
    {
        private const string _name = nameof(Amorize);
        private const string _picture = "amorize.svg";
        public Amorize()
        {
            ActionName = _name;
            ActionPicture = _picture;
            ActionType = ActionType.Amorize;
        }
    }
}
