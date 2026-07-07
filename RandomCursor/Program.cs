using RandomCursor.Services;

try
{
    var config =
        ConfigManager.Load();


    var scheme =
        SchemeManager.GetRandom(config);


    if (scheme == null)
    {
        Console.WriteLine(
            "対象スキームなし");
        return;
    }


    CursorManager.Apply(scheme);


    Logger.Write(
        config,
        scheme);


    Console.WriteLine(
        $"適用 : {scheme.Name}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}