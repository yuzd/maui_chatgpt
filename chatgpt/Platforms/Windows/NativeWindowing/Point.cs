using System.Runtime.InteropServices;

namespace chatgpt.Platforms.Windows.NativeWindowing;

/// <summary>
/// Win API struct providing coordinates for a single point.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Point
{
    /// <summary>
    /// X coordinate.
    /// </summary>
    public int X;
    /// <summary>
    /// Y coordinate.
    /// </summary>
    public int Y;
}