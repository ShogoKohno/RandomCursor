namespace RandomCursor.Services;

public static class Logger
{
    private static readonly string LogDirectory =
        Path.Combine(
            AppContext.BaseDirectory,
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


        // 1MB未満なら何もしない
        if (info.Length < 1024 * 1024)
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