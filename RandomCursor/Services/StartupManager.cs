using Microsoft.Win32;

namespace RandomCursor.Services;

public static class StartupManager
{
    private const string RunKey =
        @"Software\Microsoft\Windows\CurrentVersion\Run";

    private const string AppName =
        "RandomCursor";


    /// <summary>
    /// スタートアップ登録
    /// </summary>
    public static void Enable(string exePath)
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKey,
                writable: true);


        if (key == null)
            throw new Exception(
                "Startup registry unavailable");


        key.SetValue(
            AppName,
            $"\"{exePath}\" --startup-run");
    }



    /// <summary>
    /// スタートアップ解除
    /// </summary>
    public static void Disable()
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKey,
                writable: true);


        if (key == null)
            return;


        key.DeleteValue(
            AppName,
            false);
    }



    /// <summary>
    /// 登録状態確認
    /// </summary>
    public static bool IsEnabled()
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKey);


        if (key == null)
            return false;


        return key.GetValue(
            AppName) != null;
    }
}