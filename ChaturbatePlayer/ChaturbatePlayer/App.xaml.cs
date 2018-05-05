using ChaturbatePlayer.Views;
using System.Diagnostics;
using System.Windows;
using WpfBindingErrors;

namespace ChaturbatePlayer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (Debugger.IsAttached)
                BindingExceptionThrower.Attach();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Shared.Instance.SaveSettings();
            base.OnExit(e);
        }

        void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindowView();
            MainWindow.Show();

            var splash = new SplashWindowView();
            splash.Show();
        }
    }
}
