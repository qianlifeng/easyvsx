using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx.Intellisense
{
    /// <summary>
    /// 智能提示类，作为TextView.UpdateCompletionStatus方法的第一个参数传递
    /// </summary>
    public class CompletionIntellisense : IVsCompletionSet
    {
        /// <summary>
        /// 自动提示列表集合
        /// </summary>
        public List<CompletionItem> CompletionList { get; set; }

        public CompletionIntellisense(List<CompletionItem> list)
        {
            if (list == null)
            {
                throw new NullReferenceException("list 不能为空");
            }
            CompletionList = list;
        }

        #region Implementation of IVsCompletionSet

        /// <summary>
        /// Returns flags indicating specific behaviors of this completion set.
        /// </summary>
        /// <returns>
        /// Returns one or more flags from the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.UpdateCompletionFlags"/> enumeration (specifically, the flags beginning with CSF_).
        /// </returns>
        public uint GetFlags()
        {
            return (uint)(UpdateCompletionFlags.UCS_COMPLETEWORD);
        }

        /// <summary>
        /// Returns the number of items in the completion set.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        public int GetCount()
        {
            return CompletionList.Count;
        }

        /// <summary>
        /// Returns the text of a completion set item as it appears in the completion set list.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. 
        /// If it fails, it returns an error code.
        /// </returns>
        /// <param name="iIndex">[in] Index of completion set item to return display text for.</param>
        /// <param name="ppszText">[out] Returns a string containing the display text.</param>
        /// <param name="piGlyph">[out] Returns an integer identifying the glyph to display next to the completion item. </param>
        public int GetDisplayText(int iIndex, out string ppszText, int[] piGlyph)
        {
            ppszText = CompletionList[iIndex].DisplayText;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the list of images (glyphs) supported by the completion set.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="phImages">[out] Returns a handle to the image list associated with the completion set.</param>
        public int GetImageList(out IntPtr phImages)
        {
            phImages = IntPtr.Zero;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns text describing the indicated item in the completion set.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="iIndex">[in] Index identifying the item in the completion set to provide description text for.</param><param name="pbstrDescription">[out] Returns a string containing the description text.</param>
        public int GetDescriptionText(int iIndex, out string pbstrDescription)
        {
            pbstrDescription = CompletionList[iIndex].Description;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Determines where to display the completion set list in the editor.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>.
        ///  If it fails, it returns an error code.
        /// </returns>
        /// <param name="piLine">[out] Returns the line number of the characters that should not be obscured.</param>
        /// <param name="piStartCol">[out] Returns the column number of the first character that should not be obscured.</param>
        /// <param name="piEndCol">[out] Returns the last character in the span that should not be obscured. This must be on the same line as <paramref name="piLine"/>.
        /// </param>
        public int GetInitialExtent(out int piLine, out int piStartCol, out int piEndCol)
        {
            piLine = 100;
            piStartCol = 10;
            piEndCol = 100;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Determines the index of the closest matching completion set, given what has been typed so far.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pszSoFar">[in] A string containing the text typed by the user.
        /// </param><param name="iLength">[in] Integer containing the length of the string.
        /// </param><param name="piIndex">[out] Returns an integer specifying the index.</param>
        /// <param name="pdwFlags">[out] Returns the type of match completed. For a list of 
        /// <paramref name="pdwFlags"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.UpdateCompletionFlags"/>.</param>
        public int GetBestMatch(string pszSoFar, int iLength, out int piIndex, out uint pdwFlags)
        {
            piIndex = 0;
            pdwFlags = (uint)UpdateCompletionFlags.UCS_COMPLETEWORD;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Determines how text is completed.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pszSoFar">[in] The text typed so far.</param><param name="iIndex">[in] Index identifying the match completion set item.</param><param name="fSelected">[in] Indicates whether a completion item is selected in the completion box. If true, then the value of the <paramref name="pszSoFar"/> parameter is replaced by the text returned by <see cref="M:Microsoft.VisualStudio.TextManager.Interop.IVsCompletionSet.GetDisplayText(System.Int32,System.String@,System.Int32[])"/>. If true, this indicates that an <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/> return with the value of <paramref name="pbstrCompleteWord"/> equal to <paramref name="pszSoFar"/> is appropriate default behavior. The default value of <paramref name="fSelected"/> is true.</param><param name="cCommit">[in] Last character that was typed.</param><param name="pbstrCompleteWord">[out] Returns the complete word.</param>
        public int OnCommit(string pszSoFar, int iIndex, int fSelected, ushort cCommit, out string pbstrCompleteWord)
        {
            //没有选择提示列表，则继续显示选择列表进行提示
            if (fSelected == 0)
            {
                pbstrCompleteWord = string.Empty;
                return VSConstants.S_FALSE;
            }

            pbstrCompleteWord = pszSoFar + CompletionList[iIndex].CommitText;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Releases the completion set when it is no longer needed.
        /// </summary>
        public void Dismiss()
        {

        }

        #endregion
    }
}
