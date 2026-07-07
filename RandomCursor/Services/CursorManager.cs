using Microsoft.Win32;
using System.Runtime.InteropServices;
using RandomCursor.Models;

namespace RandomCursor.Services;

public static class CursorManager
{
    private static readonly string[] Names =
    {
        "",
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


        SystemParametersInfo(
            0x57,
            0,
            IntPtr.Zero,
            0);
    }



    [DllImport(
        "user32.dll")]
    private static extern bool SystemParametersInfo(
        uint action,
        uint param,
        IntPtr data,
        uint flags);
}