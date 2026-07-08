using RandomCursor.Services;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace RandomCursor.GUI.Services;

public class TrayIconManager
{
    private readonly NotifyIcon _notifyIcon;


    public TrayIconManager()
    {
        _notifyIcon = new NotifyIcon
        {
            Icon =
new Icon(
    Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Resources",
        "RandomCursor.ico")),
            Visible = true,
            Text = "RandomCursor"
        };


        var menu =
            new ContextMenuStrip();

        var randomItem =
    new ToolStripMenuItem(
        "今すぐ変更");


        var openItem =
            new ToolStripMenuItem(
                "設定を開く");


        var exitItem =
            new ToolStripMenuItem(
                "終了");

        menu.Items.Add(
    randomItem);
        menu.Items.Add(openItem);
        menu.Items.Add(exitItem);


        _notifyIcon.ContextMenuStrip =
            menu;

        randomItem.Click +=
            (_, _) =>
            {
                RunRandomCursor();
            };

        openItem.Click +=
            (_, _) =>
            {
                OpenWindow();
            };


        exitItem.Click +=
            (_, _) =>
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();

                System.Windows.Application.Current.Shutdown();
            };
    }


    private void OpenWindow()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(
            () =>
            {
                foreach (Window window in
                    System.Windows.Application.Current.Windows)
                {
                    if (window is MainWindow main)
                    {
                        main.Show();
                        main.WindowState =
                            WindowState.Normal;

                        main.Activate();

                        return;
                    }
                }


                var newWindow =
                    new MainWindow();

                newWindow.Show();
            });
    }
    private void RunRandomCursor()
    {
        try
        {
            var config = ConfigManager.Load();
            Logger.Initialize(config);

            var scheme = SchemeManager.GetRandom(config);

            if (scheme == null)
            {
                Logger.Error("No scheme found");
                return;
            }

            CursorManager.Apply(scheme);
            Logger.Info($"Applied scheme: {scheme.Name}");
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

}
