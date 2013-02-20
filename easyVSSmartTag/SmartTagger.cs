using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Language.Intellisense;
using easyVSSmartTag.SmartTagAction;
using easyvsx.VSObject;
using EnvDTE;

namespace easyVSSmartTag
{
    public class SmartTagger : ITagger<EasyVSSmartTag>, IDisposable
    {
        #region - 变量 -

        private ITextBuffer textBuffer;
        private ITextView textView;
        private SmartTaggerProvider smartTaggerProvider;
        private bool isDisposed;
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        #endregion

        #region - 构造函数 -

        public SmartTagger(ITextBuffer buffer, ITextView view, SmartTaggerProvider provider)
        {
            textBuffer = buffer;
            textView = view;
            smartTaggerProvider = provider;

            //  textView.LayoutChanged += OnLayoutChanged;
            textView.Caret.PositionChanged += new EventHandler<CaretPositionChangedEventArgs>(Caret_PositionChanged);
        }

        #endregion

        #region - 事件 -

        void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            ITextSnapshot snapshot = e.TextView.TextSnapshot;

            SnapshotSpan span = new SnapshotSpan(snapshot, new Span(0, snapshot.Length));
            EventHandler<SnapshotSpanEventArgs> handler = this.TagsChanged;
            if (handler != null)
            {
                handler(this, new SnapshotSpanEventArgs(span));
            }
        }

        #endregion

        #region - 方法 -

        public IEnumerable<ITagSpan<EasyVSSmartTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            ITextSnapshot snapshot = textBuffer.CurrentSnapshot;
            if (snapshot.Length == 0)
            {
                yield break; //don't do anything if the buffer is empty
            }

            VSTextView tv = new VSTextView(VSTextView.ActiveTextView);
            int line, col;
            tv.GetCursorPositon(out line, out col);

            if (VSDocument.ActiveDocument != null)
            {
                //EditPoint p = tv.GetEditPoint(line, col);
                //CodeElement c = VSDocument.ActiveDocument.ProjectItem.FileCodeModel.CodeElementFromPoint(p, vsCMElement.vsCMElementProperty);
                //if (c != null)
                //{

                //}
                //CodeElements elements = VSDocument.ActiveDocument.ProjectItem.FileCodeModel .CodeElements;
                //foreach (CodeElement element in elements)
                //{

                //    if (element.Kind == vsCMElement.vsCMElementNamespace)
                //    {
                //        CodeNamespace ns = (CodeNamespace)element;
                //        foreach (CodeElement elem in ns.Members)
                //        {
                //            if (elem is CodeClass)
                //            {
                //                CodeClass cls = elem as CodeClass;
                //                foreach (CodeElement member in cls.Members)
                //                    if (member is CodeProperty)
                //                    {
                //                        CodeType memberType = ((member as CodeProperty)).Type.CodeType;
                //                    }
                //            }
                //        }
                //    }
                //}


                //set up the navigator
                ITextStructureNavigator navigator = smartTaggerProvider.NavigatorService.GetTextStructureNavigator(textBuffer);

                foreach (var span in spans)
                {
                    ITextCaret caret = textView.Caret;
                    SnapshotPoint point;

                    if (caret.Position.BufferPosition > 0)
                    {
                        point = caret.Position.BufferPosition - 1;
                    }
                    else
                    {
                        yield break;
                    }

                    TextExtent extent = navigator.GetExtentOfWord(point);
                    //don't display the tag if the extent has whitespace
                    if (extent.IsSignificant)
                    {
                        yield return new TagSpan<EasyVSSmartTag>(extent.Span, new EasyVSSmartTag(GetSmartTagActions(extent.Span)));
                    }
                    else
                    {
                        yield break;
                    }
                }
            }


        }

        private ReadOnlyCollection<SmartTagActionSet> GetSmartTagActions(SnapshotSpan span)
        {
            List<SmartTagActionSet> actionSetList = new List<SmartTagActionSet>();
            List<ISmartTagAction> actionList = new List<ISmartTagAction>();

            ITrackingSpan trackingSpan = span.Snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeInclusive);

            actionList.Add(new GenerateFieldSmartTagAction(trackingSpan));

            SmartTagActionSet actionSet = new SmartTagActionSet(actionList.AsReadOnly());
            actionSetList.Add(actionSet);
            return actionSetList.AsReadOnly();
        }

        //private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        //{
        //    ITextSnapshot snapshot = e.NewSnapshot;
        //    //don't do anything if this is just a change in case
        //    if (!snapshot.GetText().ToLower().Equals(e.OldSnapshot.GetText().ToLower()))
        //    {
        //        SnapshotSpan span = new SnapshotSpan(snapshot, new Span(0, snapshot.Length));
        //        EventHandler<SnapshotSpanEventArgs> handler = this.TagsChanged;
        //        if (handler != null)
        //        {
        //            handler(this, new SnapshotSpanEventArgs(span));
        //        }
        //    }
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // textView.LayoutChanged -= OnLayoutChanged;
                    textView.Caret.PositionChanged -= Caret_PositionChanged;
                    textView = null;
                }

                isDisposed = true;
            }
        }

        #endregion
    }
}
