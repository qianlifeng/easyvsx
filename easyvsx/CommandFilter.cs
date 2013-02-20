using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using easyvsx.Intellisense;

namespace easyvsx
{
    /// <summary>
    /// 命令过滤器基类
    /// </summary>
    public abstract class CommandFilter : IOleCommandTarget
    {
        #region - 变量 -

        //此变量在TextManagerEventSink的事件中被赋值
        public IOleCommandTarget NextCommandTarget;

        #endregion

        #region - 暴露的事件 -

        #region BeforeExecCommand

        public delegate bool CommandHandler(Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut);

        /// <summary>
        /// 在执行命令之前执行
        /// </summary>
        public event CommandHandler BeforeExecCommand;

        /// <summary>
        /// 在执行命令之后执行
        /// </summary>
        public event CommandHandler AfterExecCommand;

        /// <summary>
        /// 在执行命令之前执行
        /// </summary>
        public virtual bool OnBeforeExecCommand(Guid pguidCmdGroup, uint nCmdid, uint nCmdExecopt, IntPtr pVain, IntPtr pVaout)
        {
            CommandHandler handler = BeforeExecCommand;
            if (handler != null)
            {
                return handler(pguidCmdGroup, nCmdid, nCmdExecopt, pVain, pVaout);
            }
            return true; ;
        }

        /// <summary>
        /// 在执行命令之后执行
        /// </summary>
        public virtual void OnAfterExecCommand(Guid pguidCmdGroup, uint nCmdid, uint nCmdExecopt, IntPtr pVain, IntPtr pVaout)
        {
            CommandHandler handler = AfterExecCommand;
            if (handler != null) handler(pguidCmdGroup, nCmdid, nCmdExecopt, pVain, pVaout);
        }

        #endregion

        #endregion

        #region - 事件 -

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return NextCommandTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (OnBeforeExecCommand(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut) == false)
            {
                //阻止执行系统命令
                return VSConstants.S_OK;
            }

            int res = NextCommandTarget.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            OnAfterExecCommand(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            return res;
        }

        #endregion
    }
}
