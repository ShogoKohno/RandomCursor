using RandomCursor.GUI.Services;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Threading;

namespace RandomCursor.GUI
{   


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private Mutex?
    _mutex;

        private TrayIconManager?
    _trayIcon;

        protected override void OnStartup(
            StartupEventArgs e)
        {
            const string mutexName =
    "RandomCursor.GUI";

            _mutex =
                new Mutex(
                    true,
                    mutexName,
                    out bool createdNew);


            if (!createdNew)
            {
                Shutdown();
                return;
            }

            base.OnStartup(e);


            _trayIcon =
                new TrayIconManager();


            var window =
                new MainWindow();


            window.Hide();
        }
    }

}
