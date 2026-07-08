using RandomCursor.GUI.Services;
using RandomCursor.Services;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Windows;

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
            if (e.Args.Contains("--startup-run"))
            {
                var config = ConfigManager.Load();
                Logger.Initialize(config);

                var scheme = SchemeManager.GetRandom(config);
                if (scheme != null)
                {
                    CursorManager.Apply(scheme);
                    Logger.Info($"Applied scheme: {scheme.Name}");
                }

                Shutdown();
                return;
            }

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


            StartupManager.RefreshIfEnabled();


            _trayIcon =
                new TrayIconManager();


            var window =
                new MainWindow();


            window.Hide();
        }
    }

}
