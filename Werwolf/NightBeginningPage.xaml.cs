using Werwolf.ViewModel;

namespace Werwolf;

public partial class NightBeginningPage : ContentPage
{
	public NightBeginningPage(NightBeginningViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        ShowExitConfirmationDialog();
        return true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is NightBeginningViewModel vm)
        {
            vm.OnPageAppearing();
        }
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