using System.Runtime.InteropServices;
namespace keyupMusic;

class HotKey
{
    //如果函数执行成功，返回值不为0。
    //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool RegisterHotKey(
        IntPtr hWnd,        //要定义热键的窗口的句柄
        int id,           //定义热键ID（不能与其它ID重复）      
        KeyModifiers fsModifiers,  //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
        Keys vk           //定义热键的内容
    );
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnregisterHotKey(
        IntPtr hWnd,        //要取消热键的窗口的句柄
        int id           //要取消热键的ID
    );
    //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
    [Flags()]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Ctrl = 2,
        Shift = 4,
        WindowsKey = 8
    }
}

public class Hot : Form
{/// 
 /// 监视Windows消息
 /// 重载WndProc方法，用于实现热键响应
 /// 
 /// 
    protected override void WndProc(ref Message m)
    {
        const int WM_HOTKEY = 0x0312;
        //按快捷键 
        switch (m.Msg)
        {
            case WM_HOTKEY:
                switch (m.WParam.ToInt32())
                {
                    case 100:  //按下的是Ctrl+Shift+1
                               //此处填写快捷键响应代码     
                        Console.WriteLine("123");
                        break;
                    case 101:  //按下的是Ctrl+Shift+2
                               //此处填写快捷键响应代码
                        break;
                    case 102:  //按下的是Ctrl+Shift+3
                               //此处填写快捷键响应代码
                        break;
                    case 103:  //按下的是Alt+D
                               //此处填写快捷键响应代码
                        break;
                }
                break;
        }
        base.WndProc(ref m);
    }
    public void MainForm_Activated(object sender, EventArgs e)
    {
        //注册热键Ctrl+Shift+1，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
        HotKey.RegisterHotKey(Handle, 100, (int)HotKey.KeyModifiers.Ctrl + HotKey.KeyModifiers.Shift, Keys.D1);
        //注册热键Ctrl+Shift+2，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。
        HotKey.RegisterHotKey(Handle, 101, (int)HotKey.KeyModifiers.Ctrl + HotKey.KeyModifiers.Shift, Keys.D2);
        //注册热键Ctrl+Shift+3，Id号为102。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。
        HotKey.RegisterHotKey(Handle, 102, (int)HotKey.KeyModifiers.Ctrl + HotKey.KeyModifiers.Shift, Keys.D3);
        //注册热键Alt+D，Id号为103。HotKey.KeyModifiers.Alt也可以直接使用数字1来表示。
        HotKey.RegisterHotKey(Handle, 103, HotKey.KeyModifiers.Alt, Keys.D);
    }
    public void txtHotKey_KeyDown(object sender, KeyEventArgs e)
    {
        int HotKeyValue = 0;
        string HotKeyString = "";
        e.SuppressKeyPress = false;
        e.Handled = true;
        if (e.Modifiers != Keys.None)
        {
            switch (e.Modifiers)
            {
                case Keys.Control:
                    HotKeyString += "Ctrl + ";
                    HotKeyValue = (int)e.Modifiers;
                    break;
                case Keys.Alt:
                    HotKeyString += "Alt + ";
                    HotKeyValue = (int)e.Modifiers;
                    break;
                case Keys.Shift:
                    HotKeyString += "Shift + ";
                    HotKeyValue = (int)e.Modifiers;
                    break;
                case Keys.Control | Keys.Alt:
                    HotKeyString += "Ctrl + Alt + ";
                    HotKeyValue = (int)e.Modifiers;
                    break;
                case Keys.Control | Keys.Shift:
                    HotKeyString += "Ctrl + Shift + ";
                    HotKeyValue = (int)e.Modifiers;
                    break;
                case Keys.Alt | Keys.Shift:
                    HotKeyString += "Alt + Shift + ";
                    HotKeyValue = (int)e.Modifiers;
                    break;
                case Keys.Control | Keys.Alt | Keys.Shift:
                    HotKeyString += "Ctrl + Alt + Shift + ";
                    HotKeyValue = (int)e.Modifiers;
                    break;
            }
            if (e.KeyCode != Keys.None && e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.Menu && e.KeyCode != Keys.ShiftKey)
            {
                HotKeyString += KeyCodeToString(e.KeyCode);
                HotKeyValue += (int)e.KeyCode;
            }
        }
        else
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                HotKeyString = "无";
                HotKeyValue = -1;
            }
            else if (e.KeyCode != Keys.None)
            {
                HotKeyString = KeyCodeToString(e.KeyCode);
                HotKeyValue = (int)e.KeyCode;
            }
        }
        if (HotKeyValue == 0)
            HotKeyValue = -1;
        TextBox txtHotKey = (TextBox)sender;
        txtHotKey.Text = HotKeyString;
        txtHotKey.Tag = HotKeyValue;
        txtHotKey.SelectionStart = txtHotKey.Text.Length;
    }
    ///
    /// 将按键转换成相应字符
    ///
    /// 按键
    /// 字符
    public string KeyCodeToString(Keys KeyCode)
    {
        if (KeyCode >= Keys.D0 && KeyCode <= Keys.D9)
        {
            return KeyCode.ToString().Remove(0, 1);
        }
        else if (KeyCode >= Keys.NumPad0 && KeyCode <= Keys.NumPad9)
        {
            return KeyCode.ToString().Replace("Pad", "");
        }
        else
        {
            return KeyCode.ToString();
        }
    }
    ///
    /// 设置按键不响应
    ///
    public void txtHotKey_KeyPress(object sender, KeyPressEventArgs e)
    {
        e.Handled = true;
    }
    ///
    /// 释放按键后，若是无实际功能键，则置无
    ///
    public void txtHotKey_KeyUp(object sender, KeyEventArgs e)
    {
        CheckHotkey(sender);
    }
    ///
    /// 失去焦点后，若是无实际功能键，则置无
    ///
    public void txtHotKey_LostFocus(object sender, EventArgs e)
    {
        CheckHotkey(sender);
    }
    ///
    /// 检查是否无实际功能键，是则置无
    ///
    public void CheckHotkey(object sender)
    {
        TextBox txtHotKey = (TextBox)sender;
        if (txtHotKey.Text.EndsWith(" + ") || String.IsNullOrEmpty(txtHotKey.Text))
        {
            txtHotKey.Text = "无";
            txtHotKey.Tag = -1;
            txtHotKey.SelectionStart = txtHotKey.Text.Length;
        }
    }
}