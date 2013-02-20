using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx.EventSink
{
    /// <summary>
    /// 负责注册每个文档（需要使用IConnectionPointContainer注册，才会触发里面的事件）
    /// </summary>
    public class TextManagerEventSink : IVsTextManagerEvents
    {
        #region - 变量 -

        private List<Type> commandFilters = new List<Type>();

        #endregion

        #region - 暴露事件 -

        public delegate void RegisterHandler1(IVsTextView view);

        /// <summary>
        /// 注册textView时触发
        /// </summary>
        public event RegisterHandler1 RegisterViewEvent;

        #endregion

        #region - 属性  -

        /// <summary>
        /// 命令过滤器列表
        /// </summary>
        public List<Type> CommandFilters
        {
            get { return commandFilters; }
            set { commandFilters = value; }
        }

        #endregion

        #region - 事件 -

        /// <summary>
        /// Fired when an external marker type is registered.
        /// </summary>
        /// <param name="iMarkerType">[in] External marker type that was registered.</param>
        public void OnRegisterMarkerType(int iMarkerType)
        {

        }

        /// <summary>
        /// Fires when a view is registered.
        /// </summary>
        /// <param name="pView">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"/> interface identifying the view that was registered. </param>
        public void OnRegisterView(IVsTextView pView)
        {
            // If this view belongs to a document that had no views before the document
            // has been opened. It's time to notify the CloneDetectiveManager about it.
            // However there is a problem: The text buffer represented by textLines has
            // not been initialized yet, i.e. the file name is not set and the file
            // content is not loaded yet. So we need to subscribe to the text buffer to
            // get notified when the file load procedure completed.

            //IVsTextLines textLines;
            //ErrorHandler.ThrowOnFailure(pView.GetBuffer(out textLines));
            //if (textLines == null)
            //{
            //    return;
            //}

            //TextBufferDataEventSink textBufferDataEventSink = new TextBufferDataEventSink();
            //IConnectionPoint connectionPoint;
            //uint cookie;

            //IConnectionPointContainer textBufferData = (IConnectionPointContainer)textLines;
            //Guid interfaceGuid = typeof(IVsTextBufferDataEvents).GUID;
            //textBufferData.FindConnectionPoint(ref interfaceGuid, out connectionPoint);
            //connectionPoint.Advise(textBufferDataEventSink, out cookie);

            //textBufferDataEventSink.TextLines = textLines;
            //textBufferDataEventSink.ConnectionPoint = connectionPoint;
            //textBufferDataEventSink.Cookie = cookie;

            //添加filter到view
            foreach (Type filterType in CommandFilters)
            {
                CommandFilter filter = Activator.CreateInstance(filterType) as CommandFilter;
                if (filter != null)
                {
                    pView.AddCommandFilter(filter, out filter.NextCommandTarget);
                }
            }

            //TextViewWindow.FromHandle(pView.GetWindowHandle());


            RegisterHandler1 handler = RegisterViewEvent;
            if (handler != null) handler(pView);
        }

        /// <summary>
        /// Fires when a view is unregistered.
        /// </summary>
        /// <param name="pView">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"/> interface identifying the view that was unregistered.</param>
        public void OnUnregisterView(IVsTextView pView)
        {
            foreach (Type filterType in CommandFilters)
            {
                CommandFilter filter = Activator.CreateInstance(filterType) as CommandFilter;
                if (filter != null)
                {
                    pView.RemoveCommandFilter(filter);
                }
            }
        }

        /// <summary>
        /// Fires when the user's global preferences are changed.
        /// </summary>
        /// <param name="pViewPrefs">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.VIEWPREFERENCES"/> structure. This structure provides the current settings for the view. If this is non-null, preferences that specifically affect text view behavior have changed.</param><param name="pFramePrefs">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.FRAMEPREFERENCES"/> structure, which allows the frame to control whether the view shows horizontal or vertical scroll bars. If this is non-NULL, preferences that specifically affect code windows have changed.</param><param name="pLangPrefs">[in] Pointer to the relevant language as specified by the <paramref name="szFileType"/> and <paramref name="guidLang"/> members of the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.LANGPREFERENCES"/> structure. If this is non-null, preferences that affect a specific language's common settings have changed.</param><param name="pColorPrefs">[in] Specifies color preferences. If non-null, the <paramref name="pguidColorService"/> member of the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.FONTCOLORPREFERENCES"/> structure indicates which colorable item provider is associated with the <paramref name="pColorTable"/> member. If this is non-null, preferences that affect the colors or font used by a text view have changed.</param>
        public void OnUserPreferencesChanged(VIEWPREFERENCES[] pViewPrefs, FRAMEPREFERENCES[] pFramePrefs, LANGPREFERENCES[] pLangPrefs, FONTCOLORPREFERENCES[] pColorPrefs)
        {
        }

        #endregion

        #region - 方法 -

        /// <summary>
        /// 为每个打开的文件自动添加filter
        /// </summary>
        public void AddCommandFilter<T>() where T : CommandFilter, new()
        {
            CommandFilters.Add(typeof(T));
        }

        /// <summary>
        /// 移除一个filter
        /// </summary>
        /// <param name="filter"></param>
        public void RemoveCommandFilter<T>() where T : CommandFilter, new()
        {
            foreach (Type commandFilterType in CommandFilters)
            {
                if (commandFilterType == typeof(T))
                {
                    CommandFilters.Remove(commandFilterType);
                }
            }
        }

        #endregion
    }
}
