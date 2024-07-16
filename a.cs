// using System;
// using System.Runtime.InteropServices;

// namespace keyupMusic;

// public static class pagedown_edge2
// {
//     // 导入Raw Input相关的结构体和API函数

//     // RAWINPUTDEVICE结构体的定义
//     [StructLayout(LayoutKind.Sequential)]
//     public struct RawInputDevice
//     {
//         public ushort usUsagePage;
//         public ushort usUsage;
//         public int dwFlags;
//         public IntPtr hwndTarget;
//     }

//     // Raw Input API函数的签名
//     [DllImport("user32.dll")]
//     public static extern uint RegisterRawInputDevices(RawInputDevice[] pRawInputDevices, uint uiNumDevices, uint cbSize);

//     [DllImport("user32.dll")]
//     public static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, out RAWINPUT pData, out uint pcbSize, uint cbSizeHeader);

//     // ... 其他Raw Input API函数的签名

//     // RAWINPUT结构体的定义（这里只是部分，完整的定义可能包含更多字段）
//     [StructLayout(LayoutKind.Explicit)]
//     public struct RAWINPUT
//     {
//         [FieldOffset(0)]
//         public RAWINPUTHEADER header;

//         [FieldOffset(16)] // 假设header结构体的大小为16字节（这取决于实际定义）
//         public RAWMOUSE mouse;

//         // ... 其他可能的输入类型，如RAWKEYBOARD、RAWHID等
//     }

//     // ... 其他结构体的定义

//     // 在你的代码中
//     public class RawInputExample
//     {
//         // ...

//         // 注册设备以接收原始输入
//         public void RegisterDevice()
//         {
//             var rid = new RawInputDevice
//             {
//                 // 设置usUsagePage, usUsage, dwFlags等
//             };

//             var result = RegisterRawInputDevices(new[] { rid }, 1, Marshal.SizeOf(rid));
//             if (result == 0)
//             {
//                 // 处理错误
//             }
//         }

//         // 处理WM_INPUT消息以获取原始输入数据
//         protected override void WndProc(ref Message m)
//         {
//             if (m.Msg == /* WM_INPUT消息的常量值 */)
//             {
//                 // 调用GetRawInputData等函数来处理原始输入数据
//             }
//             base.WndProc(ref m);
//         }

//         // ...
//     }
// }