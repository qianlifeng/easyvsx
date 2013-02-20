using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using easyvsx.VSObject;
using System.Reflection;

namespace easyvsx
{
    public struct Point
    {
        public int X;
        public int Y;
    }

    public struct MOUSEHOOKSTRUCT
    {
        public Point pt;
        public IntPtr WindowHandle;
        public uint wHitTestCode;
        public UIntPtr dwExtraInfo;
    }

    /// <summary>
    /// 将API申明为.net结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyMSG
    {
        public int vkCode;//键符虚拟码
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    public struct MOUSEHOOKSTRUCTEX
    {
        public MOUSEHOOKSTRUCT mstruct;
        public uint mouseData;
    }

    public class HookVS
    {
        #region - WinMessageConstans -

        public const int WH_MOUSE = 7;
        public const int WH_KEYBOARD = 2;
        public const int WH_CALLWNDPROC = 4;
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_PAINT = 0x000F;
        public const int GWL_WNDPROC = (-4);
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_RBUTTONDOWN = 0x204;//鼠标右键 516（int）
        public const int WM_RBUTTONUP = 0x205;//右键弹起 517（int）
        public const int WM_MBUTTONDOWN = 0x207;//鼠标中健 519（int）
        public const int WM_MBUTTONUP = 0x208;//中健弹起 520（int）

        public const int WM_LBUTTONDBLCLK = 0x203;//双击左键 515（int）
        public const int WM_RBUTTONDBLCLK = 0x206;//双击右键 518（int）
        public const int WM_MBUTTONDBLCLK = 0x209;//双击中健 521（int）

        public const int WM_MOUSEMOVE = 0x0200;
        public const int HC_ACTION = 0;
        public const int VK_BACK = 0x08;
        public const int VK_LEFT = 0x25;
        public const int VK_RIGHT = 0x27;
        public const int VK_UP = 0x26;
        public const int VK_DOWN = 0x28;
        public const int VK_END = 0x23;
        public const int VK_HOME = 0x24;
        public const int VK_PRIOR = 0x21;
        public const int VK_NEXT = 0x22;
        public const int VK_CONTROL = 0x11;
        public const int KF_ALTDOWN = 1 << 29;
        public const int VK_DELETE = 0x7F;
        public const int WM_SETREDRAW = 0x000B;

        #endregion

        #region - HookWin32API -
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int code, HookProcHandle func, IntPtr hInstance, uint threadID);
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int code, LowLevelKeyboardProcDelegate func, IntPtr hInstance, uint threadID);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref MOUSEHOOKSTRUCTEX lParam);
        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern IntPtr GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
        [DllImport("user32")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newProc);
        [DllImport("user32")]
        private static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        #endregion


        #region 事件

        public delegate bool MouseEventHandle(int code, IntPtr wParam, IntPtr lParam);
        public event MouseEventHandle MouseEvent;

        public delegate bool KeyUpEventHandle(Keys key);
        public event KeyUpEventHandle KeyUpEvent;
        public event KeyUpEventHandle KeyDownEvent;

        #endregion

        delegate int HookProcHandle(int code, IntPtr wParam, IntPtr lParam);
        delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);
        private static HookProcHandle MouseProcDelegate = null;
        private static IntPtr mouseHook;
        private static LowLevelKeyboardProcDelegate KeyboardProcDelegate = null;
        private static IntPtr keyboardHook;

        public void InitHook()
        {
            uint id = GetCurrentThreadId();

            MouseProcDelegate += MouseProc;
            mouseHook = SetWindowsHookEx(WH_MOUSE, MouseProcDelegate, IntPtr.Zero, id);
            if (mouseHook.ToInt32() == 0)
            {
                MessageBox.Show("mouse hook failed");
            }
            KeyboardProcDelegate += KeyboardProc;
            using (System.Diagnostics.Process curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardProcDelegate, GetModuleHandle(curModule.ModuleName), 0);
                if (keyboardHook.ToInt32() == 0)
                {
                    MessageBox.Show("keboard hook failed");
                }
            }

        }

        private int MouseProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (MouseEvent != null && code >= 0)
            {
                if (!MouseEvent(code, wParam, lParam))
                {
                    //help:http://msdn.microsoft.com/en-us/library/windows/desktop/ms644988(v=vs.85).aspx
                    //返回一个非0值，防止系统把消息传递给钩子链中的下一个钩子
                    //阻止系统执行鼠标事件
                    return 1;
                }
            }
            return CallNextHookEx(mouseHook, code, wParam, lParam).ToInt32();
        }

        private IntPtr KeyboardProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)Marshal.ReadInt32(lParam);
                if (KeyUpEvent != null && wParam == (IntPtr)WM_KEYUP)
                {
                    if (!KeyUpEvent(key))
                    {
                        //返回一个非0值，防止系统把消息传递给钩子链中的下一个钩子
                        //阻止系统执行事件
                        return IntPtr.Add(IntPtr.Zero, 1);
                    }
                }
                else if (KeyDownEvent != null && wParam == (IntPtr)WM_KEYDOWN)
                {
                    if (!KeyDownEvent(key))
                    {
                        //阻止系统执行事件
                        return IntPtr.Add(IntPtr.Zero, 1);
                    }
                }

            }

            return CallNextHookEx(keyboardHook, code, wParam, lParam);
        }

        public void Unhook()
        {
            if (keyboardHook.ToInt32()  != 0)
            {
                if (!UnhookWindowsHookEx(keyboardHook))
                {
                    MessageBox.Show("unhook keyboard event failed");
                }
            }
        }
    }
}
