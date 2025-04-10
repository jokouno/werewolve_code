using Werwolf.Data;
using Werwolf.ViewModel;

namespace Werwolf;

public partial class VillagerVotingPage : ContentPage
{
	public VillagerVotingPage(VillagerVotingViewModel vm)
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
            "Möchten Sie das Spiel wirklich abbrechen?",
            "Ja", "Nein");

        if (shouldExit)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as RolePresentation;
        var vm = BindingContext as VillagerVotingViewModel;

        if (selectedItem == null)
            return;

        vm?.SetSelectedPlayer(selectedItem.PlayerName);
    }

}