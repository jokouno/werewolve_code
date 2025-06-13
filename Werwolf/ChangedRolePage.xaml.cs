using Werwolf.ViewModel;

namespace Werwolf;

public partial class ChangedRolePage : ContentPage
{
    public ChangedRolePage(ChangedRoleViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        ShowExitConfirmationDialog();
        return true;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 1. Fade-In
        await RoleFrame.FadeTo(1,
            500);

        // 2. Scale-Bump (anstupsen)
        await RoleFrame.ScaleTo(1.2,
            300,
            Easing.CubicInOut);

        // 3. Zurück auf Normal-Scale mit Bounce-Effekt
        await RoleFrame.ScaleTo(1.0,
            500,
            Easing.BounceOut);
    }

    private async void ShowExitConfirmationDialog()
    {
        bool shouldExit = await DisplayAlert("Spiel abbrechen",
            "Möchten Sie das Spiel wirklich abbrechen?",
            "Ja", "Nein");

        if (shouldExit)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}