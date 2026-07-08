using Microsoft.Win32;
using System.Runtime.InteropServices;
using RandomCursor.Models;

namespace RandomCursor.Services;

public static class CursorManager
{
    private const uint SpiSetCursors = 0x57;
    private const uint SpifUpdateIniFile = 0x01;
    private const uint SpifSendChange = 0x02;

    private static readonly string[] Names =
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


    public static void Apply(
        CursorScheme scheme)
    {
        using RegistryKey key =
            Registry.CurrentUser.CreateSubKey(
                @"Control Panel\Cursors")!;


        for (int i = 0; i < Names.Length; i++)
        {
            if (i < scheme.Cursors.Length)
            {
                key.SetValue(
                    Names[i],
                    scheme.Cursors[i]);
            }
        }


        key.SetValue(
            "",
            scheme.Name);


        bool applied =
            SystemParametersInfo(
                SpiSetCursors,
                0,
                IntPtr.Zero,
                0);

        if (!applied)
        {
            Logger.Error(
                $"SystemParametersInfo returned false. Win32Error={Marshal.GetLastWin32Error()}");
        }
    }



    [DllImport(
        "user32.dll",
        SetLastError = true)]
    private static extern bool SystemParametersInfo(
        uint action,
        uint param,
        IntPtr data,
        uint flags);
}
