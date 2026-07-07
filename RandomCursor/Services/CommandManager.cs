namespace RandomCursor.Services;

public static class CommandManager
{
    public static string? GetCommand(
        string[] args)
    {
        if (args.Length == 0)
            return null;

        return args[0];
    }


    public static string? GetArgument(
        string[] args)
    {
        if (args.Length < 2)
            return null;

        return args[1];
    }
}