using Werwolf.ViewModel;

namespace Werwolf;

public partial class DayBeginningPage : ContentPage
{
	public DayBeginningPage(DayBeginningViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        ShowExitConfirmationDialog();
        return true;
    }

    private async void ShowExitConfirmationDialog()
    {
        bool shouldExit = await DisplayAlert("Spiel abbrechen",
            "M�chten Sie das Spiel wirklich abbrechen?",
            "Ja", "Nein");

        if (shouldExit)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}