 using Werwolf.ViewModel;

 namespace Werwolf
{
    public partial class MainPage : ContentPage
    {
        public MainPage(EnterNameViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
