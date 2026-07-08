using System.Diagnostics;
using System.IO;

namespace RandomCursor.Services;

public static class StartupManager
{
    private const string TaskName = "RandomCursor";

    private static string GetCoreExecutablePath()
    {
        return Environment.ProcessPath
            ?? throw new InvalidOperationException("Executable path could not be resolved.");
    }


    public static void Enable()
    {
        string exePath =
    GetCoreExecutablePath();

        string arguments =
            "--startup-run";

        string taskCommand =
            $@"/Create /TN ""RandomCursor"" " +
            $@"/TR ""\""{exePath}\"" {arguments}"" " +
            "/SC ONLOGON " +
            "/DELAY 0000:30 " +
            "/F";


        var process = Process.Start(
            new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = taskCommand,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });


        string output =
            process!.StandardOutput.ReadToEnd();

        string error =
            process.StandardError.ReadToEnd();


        File.WriteAllText(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "startup_debug.txt"),
            $"OUTPUT:\n{output}\nERROR:\n{error}");
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
            });
    }


    public static bool IsEnabled()
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
