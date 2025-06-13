using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Handlers;
using Werwolf.ViewModel;
using Werwolf.Workflow;

namespace Werwolf
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

#endif
            builder.Services.AddSingleton<GameManager>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<EnterNameViewModel>();

            builder.Services.AddTransient<RoleSelectionPage>();
            builder.Services.AddTransient<RoleSelectionViewModel>();

            builder.Services.AddTransient<PlayerTurnOverviewPage>();
            builder.Services.AddTransient<PlayerTurnOverviewViewModel>();

            builder.Services.AddTransient<PlayerTurnMenuPage>();
            builder.Services.AddTransient<PlayerTurnMenuViewModel>();

            builder.Services.AddTransient<DayBeginningPage>();
            builder.Services.AddTransient<DayBeginningViewModel>();

            builder.Services.AddTransient<DayDeadPlayerPage>();
            builder.Services.AddTransient<DayDeadPlayerViewModel>();

            builder.Services.AddTransient<DiscussionPage>();
            builder.Services.AddTransient<DiscussionViewModel>();

            builder.Services.AddTransient<DiscussionTimerPage>();
            builder.Services.AddTransient<DiscussionTimerViewModel>();

            builder.Services.AddTransient<VillagerVotingPage>();
            builder.Services.AddTransient<VillagerVotingViewModel>();

            builder.Services.AddTransient<NightBeginningPage>();
            builder.Services.AddTransient<NightBeginningViewModel>();

            builder.Services.AddTransient<GameOverPage>();
            builder.Services.AddTransient<GameOverViewModel>();

            builder.Services.AddTransient<SpecialPowerPage>();
            builder.Services.AddTransient<SpecialPowerViewModel>();

            builder.Services.AddTransient<SeherPage>();
            builder.Services.AddTransient<SeherViewModel>();

            builder.Services.AddTransient<ChangedRolePage>();
            builder.Services.AddTransient<ChangedRoleViewModel>();

            builder.Services.AddTransient<SettingsMenuPage>();
            builder.Services.AddTransient<SettingsMenuViewModel>();

            return builder.Build();
        }
    }
}
