using Werwolf.ViewModel;

namespace Werwolf;

public partial class SettingsMenuPage : ContentPage
{
    public SettingsMenuPage(SettingsMenuViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    public void OnSoundToggleChanged(object sender, ToggledEventArgs e)
    {
        var vm = BindingContext as SettingsMenuViewModel;

        vm?.SoundToggled();
    }
}