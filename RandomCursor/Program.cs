using RandomCursor.Models;
using RandomCursor.Services;


var config =
    ConfigManager.Load();


string? command =
    CommandManager.GetCommand(args);


CursorScheme? scheme;


switch (command)
{
    case "--startup-run":

        scheme =
            SchemeManager.GetRandom(config);

        break;

    case "--startup":

        StartupManager.Enable(
            Environment.ProcessPath!);

        Console.WriteLine(
            "Startup enabled");

        return;



    case "--remove-startup":

        StartupManager.Disable();

        Console.WriteLine(
            "Startup disabled");

        return;



    case "--startup-status":

        Console.WriteLine(
            StartupManager.IsEnabled()
                ? "Startup: Enabled"
                : "Startup: Disabled");

        return;

    case "--list":

        foreach (var s in
            SchemeManager.GetAll(config))
        {
            Console.WriteLine(s.Name);
        }

        return;


    case "--apply":

        string? name =
            CommandManager.GetArgument(args);

        scheme =
            SchemeManager.GetByName(
                config,
                name);

        break;


    default:

        scheme =
            SchemeManager.GetRandom(config);

        break;
}


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