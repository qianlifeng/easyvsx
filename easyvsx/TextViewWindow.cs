using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx
{
    public class TextViewWindow : NativeWindow
    {
        public TextViewWindow()
        {
           
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WinProcMessages.WM_KEYUP:
                    case WinProcMessages.WM_KEYDOWN:
                    case WinProcMessages.WM_LBUTTONUP:
                    case WinProcMessages.WM_RBUTTONUP:
                    case WinProcMessages.WM_MBUTTONUP:
                    case WinProcMessages.WM_XBUTTONUP:
                    case WinProcMessages.WM_LBUTTONDOWN:
                    case WinProcMessages.WM_MBUTTONDOWN:
                    case WinProcMessages.WM_RBUTTONDOWN:
                    case WinProcMessages.WM_XBUTTONDOWN:
                    case WinProcMessages.WM_LBUTTONDBLCLK:
                    case WinProcMessages.WM_MBUTTONDBLCLK:
                    case WinProcMessages.WM_RBUTTONDBLCLK:
                    case WinProcMessages.WM_XBUTTONDBLCLK:
                        base.WndProc(ref m);
                        break;

                    case WinProcMessages.WM_PAINT:
                        base.WndProc(ref m);
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public static class WinProcMessages
    {
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x101;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_XBUTTONUP = 0x20C;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_XBUTTONDOWN = 0x20B;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_MBUTTONDBLCLK = 0x0209;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_XBUTTONDBLCLK = 0x020D;
        public const int WM_PARENTNOTIFY = 0x0210;

        public const int WM_PAINT = 0x000F;
        public const int WM_ERASEBKGND = 0x0014;

        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;

        public const int WM_SETREDRAW = 0x000B;
    }

    public static partial class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern IntPtr EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        public static RECT GetClientRect(IntPtr hWnd)
        {
            RECT result = new RECT();
            GetClientRect(hWnd, out result);
            return result;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetUpdateRect(IntPtr hWnd, out RECT lpRect, bool erase);

        public static RECT GetUpdateRect(IntPtr hWnd, bool erase)
        {
            RECT result = new RECT();
            GetUpdateRect(hWnd, out result, erase);
            return result;
        }

        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);

        public static bool InvalidateRect(IntPtr hWnd, bool bErase)
        {
            return InvalidateRect(hWnd, IntPtr.Zero, bErase);
        }



        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ShowCaret(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ValidateRect(IntPtr hWnd, IntPtr rect);

        [DllImport("user32.dll")]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT rect);



        #region Helpers

        public static void TurnOffRedrawing(IntPtr hWnd)
        {
            SendMessage(hWnd, WinProcMessages.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        public static void TurnOnRedrawing(IntPtr hWnd)
        {
            SendMessage(hWnd, WinProcMessages.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
        }

        #endregion
    }
}
