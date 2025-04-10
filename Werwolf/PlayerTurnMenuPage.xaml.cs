using Werwolf.Data;
using Werwolf.Data.Actions;
using Werwolf.ViewModel;

namespace Werwolf;

public partial class PlayerTurnMenuPage : ContentPage
{
    public PlayerTurnMenuPage(PlayerTurnMenuViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        var selectedItem = e.CurrentSelection.FirstOrDefault() as RolePresentation;
        var vm = BindingContext as PlayerTurnMenuViewModel;

        if (selectedItem == null)
            return;

        if (selectedItem.IsSameTeam)
        {
            collectionView.SelectedItem = null;
            if (vm != null)
            {
                vm.IsFinishUpButtonEnabled = false;
            }

            return;
        }

        vm?.SetSelectedPlayer(selectedItem.PlayerName);
    }

    private void OnMultipleSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        List<RolePresentation> selectedPlayers = e.CurrentSelection.Cast<RolePresentation>().ToList();
        var vm = BindingContext as PlayerTurnMenuViewModel;

        if (selectedPlayers == null || !selectedPlayers.Any())
            return;

        if (selectedPlayers.Count == 2)
        {
            vm.IsFinishUpButtonEnabled = true;
            vm?.SetSelectedPlayers(selectedPlayers.Select(x => x.PlayerName).ToList());
        }
        else
        {
            vm.IsFinishUpButtonEnabled = false;
        }
    }

    private void OnActionSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as PlayerAction;
        var vm = BindingContext as PlayerTurnMenuViewModel;

        if (selectedItem == null)
            return;

        if (vm != null)
        {
            vm.IsActionFinishUpButtonEnabled = true;
            vm?.SetSelectedAction(selectedItem);
        }
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
}