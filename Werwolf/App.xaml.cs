using Werwolf.Workflow;

namespace Werwolf
{
    public partial class App : Application
    {
        public App()
        {
            ExceptionLogger.Log("Start");
            try
            {
                InitializeComponent();

                MainPage = new AppShell();
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }
    }
}
