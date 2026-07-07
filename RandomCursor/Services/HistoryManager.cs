namespace RandomCursor.Services;

public static class HistoryManager
{
    private static readonly string FilePath =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
            "RandomCursor",
            "LastScheme.txt");


    public static string? Load()
    {
        if (!File.Exists(FilePath))
            return null;

        return File.ReadAllText(FilePath);
    }


    public static void Save(string name)
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(FilePath)!);

        File.WriteAllText(
            FilePath,
            name);
    }
}