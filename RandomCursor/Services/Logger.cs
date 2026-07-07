using RandomCursor.Config;
using RandomCursor.Models;

namespace RandomCursor.Services;

public static class Logger
{
    public static void Write(
        AppConfig config,
        CursorScheme scheme)
    {
        if (!config.WriteLog)
            return;


        File.AppendAllText(
            config.LogFile,
            $"{DateTime.Now} : {scheme.Name}\n");
    }
}