using Werwolf.ViewModel;

namespace Werwolf;

public partial class GameOverPage : ContentPage
{
	public GameOverPage(GameOverViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}