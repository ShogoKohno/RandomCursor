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
}