using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class SchemeManager
{
    private const string SchemeKey =
        @"Control Panel\Cursors\Schemes";

    private const string Prefix = "ランダム_";

    private static readonly string HistoryFile =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
            "RandomCursorLast.txt");

    public static CursorScheme? SelectRandom()
    {
        List<CursorScheme> schemes = LoadSchemes();

        if (schemes.Count == 0)
            return null;

        string last = "";

        if (File.Exists(HistoryFile))
            last = File.ReadAllText(HistoryFile);

        if (schemes.Count > 1)
            schemes.RemoveAll(x => x.Name == last);

        Random random = new();

        CursorScheme selected =
            schemes[random.Next(schemes.Count)];

        File.WriteAllText(
            HistoryFile,
            selected.Name);

        return selected;
    }

    private static List<CursorScheme> LoadSchemes()
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(SchemeKey);

        List<CursorScheme> result = new();

        if (key == null)
            return result;

        foreach (string name in key.GetValueNames())
        {
            if (!name.StartsWith(Prefix))
                continue;

            string? value =
                key.GetValue(name) as string;

            if (value == null)
                continue;

            result.Add(
                new CursorScheme(
                    name,
                    value.Split(',')));
        }

        return result;
    }
}