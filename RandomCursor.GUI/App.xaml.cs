using RandomCursor.GUI.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RandomCursor.GUI
{   


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private TrayIconManager?
    _trayIcon;

        protected override void OnStartup(
            StartupEventArgs e)
        {
            base.OnStartup(e);


            _trayIcon =
                new TrayIconManager();


            var window =
                new MainWindow();


            window.Hide();
        }
    }

}
