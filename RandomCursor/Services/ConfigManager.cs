using System.Text.Json;
using RandomCursor.Config;

namespace RandomCursor.Services;

public static class ConfigManager
{
    public static AppConfig Load()
    {
        const string file =
            "appsettings.json";

        if (!File.Exists(file))
            return new AppConfig();


        string json =
            File.ReadAllText(file);


        return JsonSerializer
            .Deserialize<AppConfig>(json)
            ?? new AppConfig();
    }
}