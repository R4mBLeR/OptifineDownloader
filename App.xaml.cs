using System.Windows;

namespace OptifineDownloader
{
    public partial class App : Application
    {
        

        public App()
        {
            this.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

    }
}
