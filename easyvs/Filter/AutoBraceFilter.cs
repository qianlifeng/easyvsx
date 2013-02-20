/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 1:51:51
 *
 *
 ******************************************************************************/
using System;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using easyvsx;
using System.Runtime.InteropServices;
using easyvsx.VSObject;
using easyVS.Forms.Setting.UC;

namespace Easy.Filter
{
    public class AutoBraceFilter : CommandFilter
    {
        #region - 方法 -

        public override void OnAfterExecCommand(Guid pguidCmdGroup, uint nCmdid, uint nCmdExecopt, IntPtr pVain, IntPtr pVaout)
        {
            if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
            {
                switch (nCmdid)
                {
                    case (uint)VSConstants.VSStd2KCmdID.TYPECHAR:
                        object obj = Marshal.GetObjectForNativeVariant(pVain);
                        var key = (ushort)obj;
                        switch (key)
                        {
                            case '(':
                                InsertPair(")");
                                break;

                            case '[':
                                InsertPair("]");
                                break;

                            case '{':
                                InsertPair("}");
                                break;

                            case '"':
                                InsertPair("\"");
                                break;

                            case '\'':
                                InsertPair("'");
                                break;

                            case '<':
                                InsertPair(">");
                                break;

                        }

                        break;
                }
            }
        }

        private static void InsertPair(string pair)
        {
            if (BaseUCSetting.SettingModel.AutoBraceModel.OpenAutoBrace)
            {
                int currentLineIndex, currentColIndex;
                VSTextView textview = new VSTextView(VSTextView.ActiveTextView);
                textview.GetCursorPositon(out currentLineIndex, out currentColIndex);
                textview.InsertText(pair, currentLineIndex, currentColIndex);
                textview.MoveCursorTo(currentLineIndex, currentColIndex);
            }
        }

        private void ShowSuggest()
        {

            if (VSTextView.ActiveTextView == null) return;

            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);

            //检测到前面的类型名字
            //如果类型后面隔了一个空格这进行提示，否则不提示
            int cursorLine;
            int cursorCol;
            textView.GetCursorPositon(out cursorLine, out cursorCol);
            string before = textView.GetText(cursorLine, cursorCol - 1, cursorLine, cursorCol);
        }

        #endregion

    }
}