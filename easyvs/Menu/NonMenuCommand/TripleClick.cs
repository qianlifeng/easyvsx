using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using easyvsx;
using System.Windows.Forms;
using easyvsx.VSObject;
using System.Runtime.InteropServices;
using easyVS.Forms.Setting.UC;

namespace easyVS.Menu.NonMenuCommand
{
    public class TripleClick
    {
        private static int lastClickTickCount = 0;
        private static int count = 0;

        public static bool MouseEvent(int code, IntPtr wParam, IntPtr lParam)
        {
            if (BaseUCSetting.SettingModel.TripleClickModel.OpenTripleClick)
            {
                KeyMSG keys = (KeyMSG)Marshal.PtrToStructure(lParam, typeof(KeyMSG));//获取Key
                if ((int)wParam == HookVS.WM_LBUTTONUP)
                {
                    //SelectCurrentLine();
                    HandleMouseClick();
                }
            }
            return true ;
        }

        public static void HandleMouseClick()
        {
            //得到上次点击的时间
            //  < 预设间隔    
            //      计数器+1 == 4
            //         则执行事件
            //  > 预设间隔
            //      计数器 = 0

            int now;
            if (count == 0)
            {
                //第一次点击
                now = lastClickTickCount = System.Environment.TickCount;
                //VsOutput.ShowCustomerMessage("vs", "count == 0");
            }
            else
            {
                now = System.Environment.TickCount;
            }

            if (now - lastClickTickCount < 200)  //预设间隔
            {
                lastClickTickCount = now;
                //VsOutput.ShowCustomerMessage("vs", "连续点击" + (count+1) + "次");

                if (++count == 4)
                {
                    count = 0;
                    SelectCurrentLine();
                }
            }
            else
            {
                //VsOutput.ShowCustomerMessage("vs", "大于200ms");

                //能够执行到这里，说明正在进行第一次点击
                count = 0;
                //now = lastClickTickCount = System.Environment.TickCount;
            }
        }

        private static void SelectCurrentLine()
        {
            VSTextView text = new VSTextView(VSTextView.ActiveTextView);
            if (text != null)
            {
                int line = 0;
                int col = 0;
                text.GetCursorPositon(out line, out col);
                text.SelectText(line, 0, line, text.GetOneLineText(line).Length);
            }
        }
    }
}
