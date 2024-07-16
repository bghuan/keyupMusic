using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace keyupMusic;

public static class pagedown_edge
{
    public static void yo(object sender, KeyEventArgs e)
    {
        Keys[] keys = { Keys.PageDown, Keys.PageUp };

        if (keys[0].Equals(e.KeyCode))
        {

        }
        else if (keys[1].Equals(e.KeyCode))
        {

        }

        IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄

        string windowTitle = GetWindowText(hwnd);
        Console.WriteLine("当前活动窗口名称: " + windowTitle);
    }
// 导入user32.dll中的GetForegroundWindow函数
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    // 导入user32.dll中的GetWindowText函数
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    // 获取窗口标题的辅助方法
    private static string GetWindowText(IntPtr hWnd)
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        if (GetWindowText(hWnd, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }
}