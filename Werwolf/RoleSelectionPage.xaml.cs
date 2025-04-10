using Werwolf.ViewModel;

namespace Werwolf;

public partial class RoleSelectionPage : ContentPage
{
	public RoleSelectionPage(RoleSelectionViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }

    public void OnToggleChanged(object sender, ToggledEventArgs e)
    {
        var vm = BindingContext as RoleSelectionViewModel;

        vm?.StartRandomRolesGame();
    }
}