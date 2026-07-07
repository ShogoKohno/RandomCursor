using Microsoft.Win32;
using RandomCursor.Config;
using RandomCursor.Models;

namespace RandomCursor.Services;

public static class SchemeManager
{
    private const string RegistryPath =
        @"Control Panel\Cursors\Schemes";


    /// <summary>
    /// ランダムにスキームを取得
    /// </summary>
    public static CursorScheme? GetRandom(
        AppConfig config)
    {
        List<CursorScheme> schemes =
            Load(config.Prefix);


        if (schemes.Count == 0)
            return null;


        if (config.AvoidPrevious)
        {
            string? last =
                HistoryManager.Load();


            if (schemes.Count > 1)
            {
                schemes.RemoveAll(
                    x => x.Name == last);
            }
        }


        CursorScheme selected =
            schemes[
                Random.Shared.Next(
                    schemes.Count)];


        HistoryManager.Save(
            selected.Name);


        return selected;
    }


    /// <summary>
    /// 全スキーム取得
    /// </summary>
    public static List<CursorScheme> GetAll(
        AppConfig config)
    {
        return Load(config.Prefix);
    }


    /// <summary>
    /// 名前指定で取得
    /// </summary>
    public static CursorScheme? GetByName(
        AppConfig config,
        string? name)
    {
        if (string.IsNullOrEmpty(name))
            return null;


        return Load(config.Prefix)
            .FirstOrDefault(
                x => x.Name == name);
    }


    /// <summary>
    /// レジストリからスキーム読み込み
    /// </summary>
    private static List<CursorScheme> Load(
        string prefix)
    {
        List<CursorScheme> result =
            new();


        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                RegistryPath);


        if (key == null)
            return result;


        foreach (string name in key.GetValueNames())
        {
            if (!name.StartsWith(prefix))
                continue;


            string? value =
                key.GetValue(name)
                as string;


            if (string.IsNullOrEmpty(value))
                continue;


            result.Add(
                new CursorScheme(
                    name,
                    value.Split(',')));
        }


        return result;
    }
}