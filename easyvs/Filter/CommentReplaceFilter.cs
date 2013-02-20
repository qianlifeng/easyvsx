using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using easyvsx;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;
using easyvsx.VSObject;
using easyVS.Core;
using System.Windows.Forms;

namespace easyVS.Filter
{
    public class CollapseXmlComment : CommandFilter
    {
        public override bool OnBeforeExecCommand(Guid pguidCmdGroup, uint nCmdid, uint nCmdExecopt, IntPtr pVain, IntPtr pVaout)
        {
            if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
            {
                switch (nCmdid)
                {
                    case (uint)VSConstants.VSStd2KCmdID.OUTLN_TOGGLE_CURRENT:
                        CommentHiddenClient client = new CommentHiddenClient();
                        VSHiddenTextManager hiddenTextManager = new VSHiddenTextManager();
                        hiddenTextManager.CreateHiddenRegion("test", new TextSpan() { iStartLine = 0,iStartIndex = 0,iEndLine = 5,iEndIndex = 0});
                        IList<TextSpan> tps = hiddenTextManager.GetHiddenRegions(hiddenTextManager.GetEnumHiddenRegions());
                        if (tps == null)
                        {
                            return true ;
                        }
                        foreach (TextSpan tp in tps)
                        {
                            hiddenTextManager.CreateHiddenRegion("test", tp);
                        }
                        return true;
                    //return ReplaceComment();
                }
            }

            // true表示执行系统命令，否则阻止系统命令执行
            return true;
        }

        private bool ReplaceComment()
        {
            //找到光标所在行的注释的textspan
            //在首尾添加增加的注释
            TextSpan textSpan = GetCommentTextSpanAtCurrentCaret();

            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);

            if (textView != null && textSpan.iStartLine != -1)
            {
                //查看是否已经被替换过了
                if (textView.GetOneLineText(textSpan.iStartLine + 1).Trim().StartsWith("///") &&
                    !textView.GetOneLineText(textSpan.iStartLine).Trim().StartsWith("#region"))
                {
                    //using (VSUndo.StartUndo())
                    //{
                    //    string spaceStr = GetSpaceBeforeComment(textView.GetOneLineText(textSpan.iStartLine + 1));
                    //    textView.InsertText("\r\n" + spaceStr + "#region test", textSpan.iStartLine, 0);
                    //    textView.InsertText(spaceStr + "#endregion\r\n", textSpan.iEndLine + 1, 0);
                    //    textView.SetCursorPositon(textSpan.iStartLine + 1, spaceStr.Length);
                    //    VSDocument.SaveActiveDocument();
                    //    DelayExecute.Execute(3000, () => 
                    //    {
                    //        VSBase.ExecuteCommand((uint)VSConstants.VSStd2KCmdID.OUTLN_TOGGLE_CURRENT);
                    //    });
                    //    return false;
                    //}

                }
            }

            return true;
        }

        private string GetSpaceBeforeComment(string p)
        {
            int i = 0;
            while (p[i] == ' ')
            {
                i++;
            }
            return new string(' ', i);
        }

        private TextSpan GetCommentTextSpanAtCurrentCaret()
        {
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            if (textView != null)
            {
                int line, col;
                textView.GetCursorPositon(out line, out col);


                int topIndex = line;
                int bottomIndex = line;

                string topLineText = textView.GetOneLineText(topIndex);
                while (!string.IsNullOrEmpty(topLineText) && topLineText.Trim().StartsWith("///"))
                {
                    topIndex--;
                    topLineText = textView.GetOneLineText(topIndex);
                }

                string bottomLineText = textView.GetOneLineText(bottomIndex);
                while (!string.IsNullOrEmpty(topLineText) && bottomLineText.Trim().StartsWith("///"))
                {
                    bottomIndex++;
                    bottomLineText = textView.GetOneLineText(bottomIndex);
                }

                return new TextSpan() { iStartLine = topIndex, iEndLine = bottomIndex };
            }

            return new TextSpan() { iStartLine = -1, iEndLine = -1 };
        }
    }

    /// <summary>
    /// 所有用于创建的hidden区域，都会在这里进行高级处理(client control)
    /// </summary>
    public class CommentHiddenClient : IVsHiddenTextClient
    {
        public int ExecMarkerCommand(IVsHiddenRegion pHidReg, int iItem)
        {
            MessageBox.Show("ExecMarkerCommand");
            return VSConstants.S_OK;
        }

        public int GetMarkerCommandInfo(IVsHiddenRegion pHidReg, int iItem, string[] pbstrText, uint[] pcmdf)
        {
            MessageBox.Show("GetMarkerCommandInfo");
            return VSConstants.S_OK;
        }

        public int GetTipText(IVsHiddenRegion pHidReg, string[] pbstrText = null)
        {
            //pHidReg.Invalidate((uint)CHANGE_HIDDEN_REGION_FLAGS.chrNonUndoable);
            TextSpan[] textSpans = new TextSpan[1];
            pHidReg.GetSpan(textSpans);
            pbstrText[0] = DateTime.Now.ToString();

            return VSConstants.S_OK;
        }

        public int MakeBaseSpanVisible(IVsHiddenRegion pHidReg, TextSpan[] pBaseSpan)
        {
            MessageBox.Show("MakeBaseSpanVisible");
            return VSConstants.S_OK;

        }

        public void OnBeforeSessionEnd()
        {
            MessageBox.Show("MakeBaseSpanVisible");
        }

        public void OnHiddenRegionChange(IVsHiddenRegion pHidReg, HIDDEN_REGION_EVENT EventCode, int fBufferModifiable)
        {
            MessageBox.Show(EventCode.ToString());
        }
    }
}
