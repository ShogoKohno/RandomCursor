using Microsoft.Win32;
using System.Runtime.InteropServices;

public static class CursorManager
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SystemParametersInfo(
    uint uiAction,
    uint uiParam,
    IntPtr pvParam,
    uint fWinIni);

    private static readonly string[] CursorNames =
    {
        "Arrow",
        "Help",
        "AppStarting",
        "Wait",
        "Crosshair",
        "IBeam",
        "NWPen",
        "No",
        "SizeNS",
        "SizeWE",
        "SizeNWSE",
        "SizeNESW",
        "SizeAll",
        "UpArrow",
        "Hand",
        "Pin",
        "Person"
    };

    public static void Apply(CursorScheme scheme)
    {
        using RegistryKey? key =
            Registry.CurrentUser.CreateSubKey(
                @"Control Panel\Cursors");

        if (key == null)
            throw new Exception("Cursorsキーを開けません。");

        for (int i = 0; i < CursorNames.Length; i++)
        {
            if (i < scheme.Cursors.Length)
            {
                key.SetValue(
                    CursorNames[i],
                    scheme.Cursors[i]);
            }
        }

        key.SetValue("", scheme.Name);

        const uint SPI_SETCURSORS = 0x57;

        bool result = SystemParametersInfo(
            SPI_SETCURSORS,
            0,
            IntPtr.Zero,
            0);

        if (!result)
        {
            throw new Exception(
                $"SystemParametersInfo失敗 : {Marshal.GetLastWin32Error()}");
        }
    }
}