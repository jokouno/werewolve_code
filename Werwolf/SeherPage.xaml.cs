using Werwolf.ViewModel;

namespace Werwolf;

public partial class SeherPage : ContentPage
{
	public SeherPage(SeherViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
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