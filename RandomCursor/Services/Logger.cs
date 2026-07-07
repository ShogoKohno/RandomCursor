using RandomCursor.Config;

namespace RandomCursor.Services;

public static class Logger
{
    private static bool _enabled = true;
    private static int _maxLogSizeMB = 1;


    public static void Initialize(
        AppConfig config)
    {
        _enabled =
            config.WriteLog;

        _maxLogSizeMB =
            config.MaxLogSizeMB > 0
                ? config.MaxLogSizeMB
                : 1;
    }

    private static readonly string LogDirectory =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
            "RandomCursor",
            "Logs");


    private static readonly string LogFile =
        Path.Combine(
            LogDirectory,
            "RandomCursor.log");


    public static void Info(string message)
    {   
        Write(
            "INFO",
            message);
    }


    public static void Error(string message)
    {
        Write(
            "ERROR",
            message);
    }


    private static void Write(
        string level,
        string message)
    {
        if (!_enabled)
        {
            return;
        }

        try
        {
            Directory.CreateDirectory(
                LogDirectory);

            RotateIfNeeded();


            string log =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                $"{level}: {message}";


            File.AppendAllText(
                LogFile,
                log + Environment.NewLine);
        }
        catch
        {
            // ログ失敗ではアプリを停止しない
        }
    }
    private static void RotateIfNeeded()
    {
        if (!File.Exists(LogFile))
        {
            return;
        }


        FileInfo info =
            new FileInfo(LogFile);


        if (info.Length < _maxLogSizeMB * 1024 * 1024)
        {
            return;
        }


        string archiveDirectory =
            Path.Combine(
                LogDirectory,
                "Archive");


        Directory.CreateDirectory(
            archiveDirectory);


        string archiveFile =
            Path.Combine(
                archiveDirectory,
                $"RandomCursor_{DateTime.Now:yyyyMMdd_HHmmss}.log");


        File.Move(
            LogFile,
            archiveFile);
    }
}