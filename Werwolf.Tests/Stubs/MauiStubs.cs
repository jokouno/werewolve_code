namespace Microsoft.Maui.Controls
{
    public class Shell
    {
        public static Shell Current { get; } = new Shell();
        public System.Threading.Tasks.Task GoToAsync(string route) => System.Threading.Tasks.Task.CompletedTask;
    }

    public class ContentPage {}
}

namespace Werwolf
{
    public partial class NightBeginningPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class PlayerTurnMenuPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class PlayerTurnOverviewPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class DayBeginningPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class DayDeadPlayerPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class DiscussionPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class DiscussionTimerPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class GameOverPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class MainPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class RoleSelectionPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class SeherPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class SpecialPowerPage : Microsoft.Maui.Controls.ContentPage {}
    public partial class VillagerVotingPage : Microsoft.Maui.Controls.ContentPage {}
}
