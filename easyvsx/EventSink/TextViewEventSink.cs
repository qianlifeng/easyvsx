using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx.EventSink
{
    public class TextViewEventSink : IVsSelectionEvents, IVsTextViewEvents
    {
        #region - 供事件使用的委托 -

        public delegate void SelectionHandler1(
                 IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld,
                 IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew);

        public delegate void ScrollInfoHandler1(
            IVsTextView pView, int iBar, int iMinUnit, int iMaxUnits, int iVisibleUnits, int iFirstVisibleUnit);


        #endregion

        #region - 对外调用的事件 -

        /// <summary>
        /// VS中选择的焦点变化时触发
        /// </summary>
        public event SelectionHandler1 SelectionChangedEvent;

        public event ScrollInfoHandler1 ChangeScrollInfoEvent;

      

        #endregion

        #region Implementation of IVsSelectionEvents

        public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
        {
            SelectionHandler1 handler = SelectionChangedEvent;
            if (handler != null) handler(pHierOld, itemidOld, pMISOld, pSCOld, pHierNew, itemidNew, pMISNew, pSCNew);

            return VSConstants.S_OK;
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            return VSConstants.S_OK;
        }

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            return VSConstants.S_OK;
        }

        #endregion

        #region Implementation of IVsTextViewEvents

        /// <summary>
        /// Notifies a client when a view receives focus.
        /// </summary>
        /// <param name="pView">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"/> interface.</param>
        public void OnSetFocus(IVsTextView pView)
        {
        }

        /// <summary>
        /// Notifies a client when a view loses focus.
        /// </summary>
        /// <param name="pView">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"/> interface.</param>
        public void OnKillFocus(IVsTextView pView)
        {
        }

        /// <summary>
        /// Notifies a client when a view is attached to a new buffer.
        /// </summary>
        /// <param name="pView">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"/> interface.</param><param name="pBuffer">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextLines"/> interface.</param>
        public void OnSetBuffer(IVsTextView pView, IVsTextLines pBuffer)
        {
        }

        /// <summary>
        /// Notifies a client when the scrolling information is changed.
        /// </summary>
        /// <param name="pView">[in] Pointer to a view object.</param><param name="iBar">[in] Integer value referring to the bar.</param><param name="iMinUnit">[in] Integer value for the minimum units.</param><param name="iMaxUnits">[in] Integer value for the maximum units.</param><param name="iVisibleUnits">[in] Integer value for the visible units.</param><param name="iFirstVisibleUnit">[in] Integer value for the first visible unit.</param>
        public virtual void OnChangeScrollInfo(IVsTextView pView, int iBar, int iMinUnit, int iMaxUnits, int iVisibleUnits, int iFirstVisibleUnit)
        {
            ScrollInfoHandler1 handler = ChangeScrollInfoEvent;
            if (handler != null) handler(pView, iBar, iMinUnit, iMaxUnits, iVisibleUnits, iFirstVisibleUnit);
        }

        /// <summary>
        /// Notifies the client when a change of caret line occurs.
        /// </summary>
        /// <param name="pView">[in] Pointer to a view object.</param><param name="iNewLine">[in] Integer containing the new line.</param><param name="iOldLine">[in] Integer containing the old line.</param>
        public void OnChangeCaretLine(IVsTextView pView, int iNewLine, int iOldLine)
        {
        }

        #endregion
    }
}
