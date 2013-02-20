using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using easyvsx;
using System.Windows.Forms;
using easyvsx.VSObject;
using easyVS.Forms.Setting.UC;

namespace easyVS.Menu.NonMenuCommand
{
    public class JumpInsert
    {
        #region - 方法 -

        public static bool KeyUpEvent(Keys key)
        {
            if (!BaseUCSetting.SettingModel.JumpInsertModel.OpenJumpInsert)
            {
                return true;
            }

            if (key == Keys.Enter && Control.ModifierKeys == Keys.Shift)
            {
                InsertBlackLineBefore();
                //返回false表示block系统执行键盘操作
                return false;
            }
            if (key == Keys.Enter && Control.ModifierKeys == Keys.Control)
            {
                InsertBlackLineAfter();
                //返回false表示block系统执行键盘操作
                return false;
            }
            return true;
        }

        public static bool KeyDownEvent(Keys key)
        {
            if (!BaseUCSetting.SettingModel.JumpInsertModel.OpenJumpInsert)
            {
                return true;
            }

            if (key == Keys.Enter && (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control))
            {
                //返回false表示block系统执行键盘操作
                return false;
            }

            return true;
        }

        private static void InsertBlackLineBefore()
        {
            int line, col;
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            textView.GetCursorPositon(out line, out col);
            textView.InsertText("\r\n", line, 0);
            textView.MoveCursorTo(line, textView.GetStartTextIndexOfCurrLine());
        }

        private static void InsertBlackLineAfter()

        {
            int line, col;
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            textView.GetCursorPositon(out line, out col);
            textView.InsertText("\r\n", line + 1, 0);
            textView.MoveCursorTo(line + 1, textView.GetStartTextIndexOfCurrLine());
        }

        #endregion
    }
}
