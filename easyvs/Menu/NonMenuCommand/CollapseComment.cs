using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using EasyCodeGenerate.Core;
using easyvsx.VSObject;
using easyVS.Filter;
using Microsoft.VisualStudio;
using EnvDTE;
using EnvDTE80;
using System.Windows.Forms;

namespace easyVS.Menu.NonMenuCommand
{
    /// <summary>
    /// 增强的注释折叠
    /// 存在的问题，可以使用CreateHiddenRegion来创建一个自定义的region
    /// </summary>
    public class CollapseComment
    {
        static CommentHiddenClient client = new CommentHiddenClient();

        public void TextManagerEventSink_RegisterViewEvent(IVsTextView view)
        {
            //VSHiddenTextManager hiddenTextManager = new VSHiddenTextManager(view,client);
            //hiddenTextManager.CreateHiddenRegion("test", new TextSpan() { iStartLine = 66, iStartIndex = 8, iEndLine = 68, iEndIndex = 21 });
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



