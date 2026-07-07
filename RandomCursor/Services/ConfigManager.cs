using System.Text.Json;
using RandomCursor.Config;

namespace RandomCursor.Services;

public static class ConfigManager
{
    private static readonly string ConfigDirectory =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
            "RandomCursor");


    private static readonly string FileName =
        Path.Combine(
            ConfigDirectory,
            "appsettings.json");

    private static readonly string BackupFileName =
    Path.Combine(
        ConfigDirectory,
        "appsettings.backup.json");
    public static AppConfig Load()
    {

        try
        {
            if (!File.Exists(FileName))
            {
                var config =
                    new AppConfig();

                Save(config);

                return config;
            }


            string json =
                File.ReadAllText(FileName);


            return JsonSerializer
                .Deserialize<AppConfig>(json)
                ?? new AppConfig();
        }
        catch (JsonException)
        {
            Console.WriteLine(
    "設定ファイルが破損しています。バックアップを作成します。");

            if (File.Exists(FileName))
            {
                File.Copy(
                    FileName,
                    BackupFileName,
                    true);
            }

            var config =
                new AppConfig();

            Save(config);

            return config;
        }
    }
    public static void Save(
    AppConfig config)
    {
        Directory.CreateDirectory(
    ConfigDirectory);

        string json =
            JsonSerializer.Serialize(
                config,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });


        File.WriteAllText(
            FileName,
            json);
    }
}