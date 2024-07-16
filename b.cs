// ... 其他命名空间引用
using System.Runtime.InteropServices;
using System.Windows.Forms;
using keyupMusic;

// ...

public static class RawInputHelper
{
    // RAWINPUTDEVICE结构体的定义
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDevice
    {
        public ushort usUsagePage;
        public ushort usUsage;
        public int dwFlags;
        public IntPtr hwndTarget;
    }

    // RAWINPUTHEADER结构体的定义
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTHEADER
    {
        public uint dwType;
        public uint dwSize;
        public IntPtr hDevice;
        public IntPtr wParam;
    }

    // RAWKEYBOARD结构体的定义（键盘输入）
    [StructLayout(LayoutKind.Explicit)]
    public struct RAWKEYBOARD
    {
        [FieldOffset(0)]
        public RAWKEYBOARDHEADER header;

        // ... 其他字段，如MakeCode, Flags, Reserved, VKey, Message等
    }

    // RAWKEYBOARDHEADER结构体的定义（键盘输入头）
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWKEYBOARDHEADER
    {
        public ushort bVKey;
        public ushort wScan;
        public uint dwFlags;
        public uint dwReserved;
        public ushort wTime;
        public ushort dwExtraInfo;
    }

    // ... 类似地，定义RAWMOUSE等结构体

    // Raw Input API函数的签名
    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint RegisterRawInputDevices(RawInputDevice[] pRawInputDevices, uint uiNumDevices, uint cbSize);

    // ... 其他Raw Input API函数的签名

    // 在你的Windows Forms应用程序中
    public partial class RawInputForm : Form
    {
        // ...

        // 注册设备以接收原始输入
        private void RegisterRawInputDevicesForKeyboardAndMouse()
        {
            // 创建两个RAWINPUTDEVICE实例，一个用于键盘，一个用于鼠标
            var keyboardDevice = new RawInputDevice
            {
                usUsagePage = 0x01, // HID_USAGE_PAGE_GENERIC
                usUsage = 0x06,     // HID_USAGE_GENERIC_KEYBOARD
                dwFlags = 0x100,    // RIDEV_INPUTSINK
                hwndTarget = this.Handle // 指向你的窗口句柄
            };

            var mouseDevice = new RawInputDevice
            {
                usUsagePage = 0x01, // HID_USAGE_PAGE_GENERIC
                usUsage = 0x02,     // HID_USAGE_GENERIC_MOUSE
                dwFlags = 0x100,    // RIDEV_INPUTSINK
                hwndTarget = this.Handle // 指向你的窗口句柄
            };

            var devices = new RawInputDevice[] { keyboardDevice, mouseDevice };
            var result = RegisterRawInputDevices(devices, (uint)devices.Length, (uint)Marshal.SizeOf(keyboardDevice));
            if (result == 0)
            {
                // 处理错误
                var error = Marshal.GetLastWin32Error();
                // ...
            }
        }

        // 处理WM_INPUT消息以获取原始输入数据
        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;
            if (m.Msg == WM_INPUT)
            {
                // 处理原始输入数据
                // ...
            }
            base.WndProc(ref m);
        }

        // ...

        public RawInputForm()
        {
            //InitializeComponent();
            // 初始化时注册设备
            RegisterRawInputDevicesForKeyboardAndMouse();
        }

        // ...
    }
}