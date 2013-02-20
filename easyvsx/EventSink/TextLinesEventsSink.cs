using System.Windows.Forms;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx.EventSink
{
    public class TextLinesEventsSink : IVsTextLinesEvents 
    {
        public IVsTextLines TextLines { get; set; }

        #region Implementation of IVsTextLinesEvents

        /// <summary>
        /// Notifies the client when the content of a text line in the buffer has changed.
        /// </summary>
        /// <param name="pTextLineChange">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.TextLineChange"/> structure that defines the shape of the old and new text.</param><param name="fLast">[in] Obsolete; Do not use.</param>
        public void OnChangeLineText(TextLineChange[] pTextLineChange, int fLast)
        {
            MessageBox.Show("text line changed in file TextLinesEventSink");
        }

        /// <summary>
        /// Notifies the client when the text line attributes have been changed.
        /// </summary>
        /// <param name="iFirstLine">[in] First affected line, inclusive.</param><param name="iLastLine">[in] Last affected line, inclusive.</param>
        public void OnChangeLineAttributes(int iFirstLine, int iLastLine)
        {

        }

        #endregion
    }
}
