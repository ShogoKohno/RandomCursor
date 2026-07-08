using System.Diagnostics;
using Microsoft.Win32;

namespace RandomCursor.Services;

public static class StartupManager
{
    private const string TaskName = "RandomCursor";
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string RunValueName = "RandomCursor";

    private static string GetCoreExecutablePath()
    {
        return Environment.ProcessPath
            ?? throw new InvalidOperationException("Executable path could not be resolved.");
    }


    private static string GetStartupCommand()
    {
        return $"\"{GetCoreExecutablePath()}\" --startup-run";
    }


    public static void Enable()
    {
        using RegistryKey key =
            Registry.CurrentUser.CreateSubKey(
                RunKeyPath)!;

        key.SetValue(
            RunValueName,
            GetStartupCommand());

        RemoveScheduledTask();
    }


    public static void Disable()
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKeyPath,
                writable: true);

        key?.DeleteValue(
            RunValueName,
            throwOnMissingValue: false);

        RemoveScheduledTask();
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
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RunKeyPath);

        if (key?.GetValue(RunValueName) is string)
        {
            return true;
        }

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
}
