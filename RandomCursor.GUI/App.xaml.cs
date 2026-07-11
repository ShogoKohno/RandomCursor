using RandomCursor.GUI.Services;
using RandomCursor.Services;
using System.Configuration;
using System.Data;
using System.IO;
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
                try
                {
                    var config = ConfigManager.Load();
                    Logger.Initialize(config);

                    Logger.Info("Startup run requested. Waiting for desktop initialization.");
                    Thread.Sleep(TimeSpan.FromSeconds(15));

                    var scheme = SchemeManager.GetRandom(config);
                    if (scheme != null)
                    {
                        CursorManager.Apply(scheme);
                        Logger.Info($"Applied scheme: {scheme.Name}");

                        Thread.Sleep(TimeSpan.FromSeconds(15));
                        CursorManager.Apply(scheme);
                        Logger.Info($"Reapplied startup scheme: {scheme.Name}");
                    }
                    else
                    {
                        Logger.Error("No scheme found for startup run.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
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
