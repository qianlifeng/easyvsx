using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace easyVSSmartTag.SmartTagAction
{
    public class GenerateFieldSmartTagAction : ISmartTagAction
    {
        private ITrackingSpan trackingSpan;
        private string m_upper;
        private string displayText;
        private ITextSnapshot snapShot;

        public GenerateFieldSmartTagAction(ITrackingSpan span)
        {
            trackingSpan = span;
            snapShot = span.TextBuffer.CurrentSnapshot;
            m_upper = span.GetText(snapShot).ToUpper();
            displayText = "生成属性";
        }

        public string DisplayText
        {
            get { return displayText; }
        }
        public ImageSource Icon
        {
            get { return null; }
        }

        public bool IsEnabled
        {
            get { return true; }
        }

        public ISmartTagSource Source
        {
            get;
            private set;
        }

        public ReadOnlyCollection<SmartTagActionSet> ActionSets
        {
            get { return null; }
        }

        public void Invoke()
        {
            trackingSpan.TextBuffer.Replace(trackingSpan.GetSpan(snapShot), m_upper);
        }
    }
}
