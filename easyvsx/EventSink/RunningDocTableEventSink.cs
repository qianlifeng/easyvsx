using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace easyvsx.EventSink
{
    public class RunningDocTableEventSink : IVsRunningDocTableEvents
    {
        #region 对外提供的事件


        public delegate void RunningDocTableHandle(uint docCookie);

        public delegate void OnBeforeDocumentWindowShowHandle(uint docCookie, int fFirstShow, IVsWindowFrame pFrame);

        /// <summary>
        /// 保存后触发
        /// </summary>
        public event RunningDocTableHandle OnAfterSaveDocEvent;

        public event OnBeforeDocumentWindowShowHandle OnBeforeDocumentWindowShowEvent;

        #endregion

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            if (OnAfterSaveDocEvent != null)
            {
                OnAfterSaveDocEvent(docCookie);
            }
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            if (OnBeforeDocumentWindowShowEvent != null)
            {
                OnBeforeDocumentWindowShowEvent(docCookie,fFirstShow,pFrame);
            }
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }
    }
}
