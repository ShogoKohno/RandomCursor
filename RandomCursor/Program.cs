CursorScheme? scheme =
    SchemeManager.SelectRandom();

if (scheme == null)
{
    Console.WriteLine("対象スキームがありません。");
    return;
}

CursorManager.Apply(scheme);

Console.WriteLine(
    $"適用しました : {scheme.Name}");