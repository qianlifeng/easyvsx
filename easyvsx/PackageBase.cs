/******************************************************************************
 *  Author：       SQ1000
 *  CreateDate：   17/02/2012 11:13:56 AM
 *
 *
 ******************************************************************************/
using System;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using easyvsx.EventSink;
using easyvsx.Markers;

namespace easyvsx
{
    public class PackageBase : Package
    {
        private uint solutionEventCookie;
        private uint selectionEventCookie;
        private uint runningDocEventCookie;
        private IConnectionPoint tmConnectionPoint;
        private uint tmConnectionCookie;
        private TextViewEventSink textViewEvent = new TextViewEventSink();
        private RunningDocTableEventSink docTableEvent = new RunningDocTableEventSink();
        private TextManagerEventSink textManagerEventSink = new TextManagerEventSink();
        protected HookVS hookVs = new HookVS();

        /// <summary>
        /// textview事件集合
        /// </summary>
        public TextViewEventSink TextViewEvent
        {
            get { return textViewEvent; }
        }

        public RunningDocTableEventSink RunningDocTableEvent
        {
            get { return docTableEvent; }
        }

        /// <summary>
        /// TextManager管理类，提供text view的注册
        /// </summary>
        public TextManagerEventSink TextManagerEventSink
        {
            get { return textManagerEventSink; }
        }

        protected override void Initialize()
        {
            base.Initialize();

            //绑定textView事件
            IVsMonitorSelection monitorSelection = (IVsMonitorSelection)GetService(typeof(SVsShellMonitorSelection));
            ErrorHandler.ThrowOnFailure(monitorSelection.AdviseSelectionEvents(textViewEvent, out selectionEventCookie));

            //绑定RunningDocTable事件
            IVsRunningDocumentTable DocTables = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));
            DocTables.AdviseRunningDocTableEvents(docTableEvent, out runningDocEventCookie);

            // And we can query for the text manager service as we're surely running in
            // interactive mode. So this is the right time to register for text manager events.
            IConnectionPointContainer textManager = (IConnectionPointContainer)GetService(typeof(SVsTextManager));
            Guid interfaceGuid = typeof(IVsTextManagerEvents).GUID;
            textManager.FindConnectionPoint(ref interfaceGuid, out tmConnectionPoint);
            tmConnectionPoint.Advise(TextManagerEventSink, out tmConnectionCookie);

            //todo:通过win32形式检测键盘输入
            hookVs.InitHook();
        }

        public object GetServices(Type serviceType)
        {
            return GetService(serviceType);
        }
    }
}
