using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx.EventSink
{
    public sealed class TextBufferDataEventSink : IVsTextBufferDataEvents
    {

        private IVsTextLines textLines;

        public IVsTextLines TextLines
        {
            get
            {
                return textLines;
            }
            set
            {
                textLines = value;
                callDocumentOpened();
            }
        }

        public IConnectionPoint ConnectionPoint { get; set; }

        public uint Cookie { get; set; }

        #region - IVsTextBufferDataEvents 实现 -

        public void OnFileChanged(uint grfChange, uint dwFileAttrs) { }

        public int OnLoadCompleted(int fReload)
        {
            ConnectionPoint.Unadvise(Cookie);

            //callDocumentOpened();
            MessageBox.Show("TextBufferDataEventSink => OnLoadCompleted");
            return VSConstants.S_OK;
        }

        #endregion


        private void callDocumentOpened()
        {
            bool sharp = IsCSharpOrCppOrC(TextLines);
        }

        private static bool IsCSharpOrCppOrC(IVsTextLines textLines)
        {
            Guid languageServiceId;
            textLines.GetLanguageServiceID(out languageServiceId);
            //return GuidList.CSHARP_LANGUAGE_GUID.Equals(languageServiceId);
            return true;
        }
    }
}
