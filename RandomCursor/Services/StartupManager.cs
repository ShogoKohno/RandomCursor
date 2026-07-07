using System.Diagnostics;
using System.IO;

namespace RandomCursor.Services;

public static class StartupManager
{
    private const string TaskName = "RandomCursor";


    public static void Enable(string exePath)
    {
        string arguments =
            "--startup-run";

        string taskCommand =
            $@"/Create /TN ""RandomCursor"" " +
            $@"/TR ""\""{exePath}\"" {arguments}"" " +
            "/SC ONLOGON " +
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
            "startup_debug.txt",
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
}