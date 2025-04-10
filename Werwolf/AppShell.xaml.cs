namespace Werwolf
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RoleSelectionPage), typeof(RoleSelectionPage));
            Routing.RegisterRoute(nameof(PlayerTurnOverviewPage), typeof(PlayerTurnOverviewPage));
            Routing.RegisterRoute(nameof(PlayerTurnMenuPage), typeof(PlayerTurnMenuPage));
            Routing.RegisterRoute(nameof(DayBeginningPage), typeof(DayBeginningPage));
            Routing.RegisterRoute(nameof(DayDeadPlayerPage), typeof(DayDeadPlayerPage));
            Routing.RegisterRoute(nameof(DiscussionPage), typeof(DiscussionPage));
            Routing.RegisterRoute(nameof(DiscussionTimerPage), typeof(DiscussionTimerPage));
            Routing.RegisterRoute(nameof(VillagerVotingPage), typeof(VillagerVotingPage));
            Routing.RegisterRoute(nameof(NightBeginningPage), typeof(NightBeginningPage));
            Routing.RegisterRoute(nameof(GameOverPage), typeof(GameOverPage));
            Routing.RegisterRoute(nameof(SpecialPowerPage), typeof(SpecialPowerPage));
            Routing.RegisterRoute(nameof(SeherPage), typeof(SeherPage));
        }
    }
}
