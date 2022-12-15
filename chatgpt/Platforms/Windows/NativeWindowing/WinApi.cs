using System.Runtime.InteropServices;

namespace chatgpt.Platforms.Windows.NativeWindowing;

/// <summary>
/// Win32 API imports.
/// </summary>
internal static class WinApi
{
    private const string User32 = "user32.dll";

    /// <summary>
    /// Creates, updates or deletes the taskbar icon.
    /// </summary>
    [DllImport("shell32.Dll", CharSet = CharSet.Unicode)]
    public static extern bool Shell_NotifyIcon(NotifyCommand cmd, [In] ref NotifyIconData data);


    [DllImport("user32.dll")]
    public static extern bool DestroyMenu(IntPtr hmenu);
	

    [DllImport("user32.dll")]
    public static extern IntPtr CreatePopupMenu();


    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern bool AppendMenu(IntPtr hMenu, MenuFlags uFlags, uint uIDNewItem, string lpNewItem);

		
    [Flags]
    public enum MenuFlags : uint
    {
        MF_STRING = 0,
        MF_BYPOSITION = 0x400,
        MF_SEPARATOR = 0x800,
        MF_REMOVE = 0x1000,
    }

    [Flags]
    public enum UFLAGS : uint
    {
        TPM_LEFTALIGN = 0x0000,
        TPM_CENTERALIGN = 0x0004,
        TPM_RIGHTALIGN = 0x0008,
        TPM_TOPALIGN = 0x0000,
        TPM_VCENTERALIGN = 0x0010,
        TPM_BOTTOMALIGN = 0x0020,
        TPM_HORIZONTAL = 0x0000,
        TPM_VERTICAL = 0x0040,
        TPM_NONOTIFY = 0x0080,
        TPM_RETURNCMD = 0x0100
    }
    /// <summary>
    /// Creates the helper window that receives messages from the taskar icon.
    /// </summary>
    [DllImport(User32, EntryPoint = "CreateWindowExW", SetLastError = true)]
    public static extern IntPtr CreateWindowEx(int dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
        [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, int dwStyle, int x, int y,
        int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance,
        IntPtr lpParam);


    /// <summary>
    /// Processes a default windows procedure.
    /// </summary>
    [DllImport(User32)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wparam, IntPtr lparam);

    /// <summary>
    /// Registers the helper window class.
    /// </summary>
    [DllImport(User32, EntryPoint = "RegisterClassW", SetLastError = true)]
    public static extern short RegisterClass(ref WindowClass lpWndClass);

    /// <summary>
    /// Registers a listener for a window message.
    /// </summary>
    /// <param name="lpString"></param>
    /// <returns>uint</returns>
    [DllImport(User32, EntryPoint = "RegisterWindowMessageW")]
    public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    /// <summary>
    /// Used to destroy the hidden helper window that receives messages from the
    /// taskbar icon.
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns>bool</returns>
    [DllImport(User32, SetLastError = true)]
    public static extern bool DestroyWindow(IntPtr hWnd);


    /// <summary>
    /// Gives focus to a given window.
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns>bool</returns>
    [DllImport(User32)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);


    /// <summary>
    /// Gets the maximum number of milliseconds that can elapse between a
    /// first click and a second click for the OS to consider the
    /// mouse action a double-click.
    /// </summary>
    /// <returns>The maximum amount of time, in milliseconds, that can
    /// elapse between a first click and a second click for the OS to
    /// consider the mouse action a double-click.</returns>
    [DllImport(User32, CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern int GetDoubleClickTime();


    /// <summary>
    /// Gets the screen coordinates of the current mouse position.
    /// </summary>
    [DllImport(User32, SetLastError = true)]
    public static extern bool GetPhysicalCursorPos(ref Point lpPoint);


  

    [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetCursorPos")]
    public static extern bool GetCursorPos(out PointInt32 pt);

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct PointInt32
    {
        public int X;
        public int Y;

        public PointInt32(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [DllImport("user32.dll")]
    public static extern uint TrackPopupMenuEx(IntPtr hmenu, UFLAGS uFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);


}