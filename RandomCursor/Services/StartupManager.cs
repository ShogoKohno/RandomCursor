using System.Diagnostics;
using Microsoft.Win32;

namespace RandomCursor.Services;

public static class StartupManager
{
    private const string TaskName = "RandomCursor";
    private const string TaskNameGUI = "RandomCursor.GUI";
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string RunValueName = "RandomCursor";


    private static string GetCoreExecutablePath()
    {
        string path = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "RandomCursor.exe");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                "RandomCursor.exe Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅB",
                path);
        }

        return path;
    }


    private static string GetStartupCommand()
    {
        return $"\"{GetCoreExecutablePath()}\" --startup-run";
    }


    public static void Enable()
    {
        RemoveRegistryEntry();

        Process.Start(
            new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments =
                    $@"/Create /TN ""{TaskName}"" " +
                    $@"/TR ""{GetStartupCommand()}"" " +
                    "/SC ONLOGON /F",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();
    }


    public static void Disable()
    {
        Process.Start(
            new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments =
                    $@"/Delete /TN ""{TaskName}"" /F",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();

        RemoveRegistryEntry();
    }


    private static void RemoveScheduledTask()
    {
        Process.Start(
            new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments =
                    $@"/Delete /TN ""{TaskName}"" /F",
                CreateNoWindow = true,
                UseShellExecute = false
            });
    }


    public static bool IsEnabled()
    {
        return IsScheduledTaskRegistered();
    }


    private static bool IsScheduledTaskRegistered()
    {
        Process process =
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = "schtasks",
                    Arguments =
                        $@"/Query /TN ""{TaskName}""",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                })!;


        string output =
            process.StandardOutput.ReadToEnd();


        process.WaitForExit();


        return output.Contains(
            TaskName);
    }


    public static void RefreshIfEnabled()
    {
        if (IsEnabled())
        {
            Enable();
        }
    }
    private static void RemoveRegistryEntry()
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKeyPath,
                writable: true);

        key?.DeleteValue(
            RunValueName,
            throwOnMissingValue: false);
    }


    public static void SetTasktray()
    {
        string path = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "RandomCursor.GUI.exe");

        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKeyPath,
                true);

        key?.SetValue(
            TaskNameGUI,
            $"\"{path}\"");
    }
    public static void RemoveTasktray()
    {
        string path = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "RandomCursor.GUI.exe");

        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKeyPath,
                true);

        key?.DeleteValue(
            TaskNameGUI,
            throwOnMissingValue: false);
    }
    public static bool IsTasktraySeted()
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKeyPath);

        return key?.GetValue(TaskNameGUI) != null;
    }
}
