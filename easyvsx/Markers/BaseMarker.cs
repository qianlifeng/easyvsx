using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx.Markers
{
    [Guid(GuidConstants.BaseMarkerGuidString)]
    [ComVisible(true)]
    public class BaseMarker : IVsPackageDefinedTextMarkerType, IVsMergeableUIItem
    {
        #region Implementation of IVsPackageDefinedTextMarkerType

        /// <summary>
        /// Returns the appearance, location, and coloring of a custom marker type as a bit filed.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pdwVisualFlags">[out] A bitwise OR of flags indicating the appearance, location, and coloring of a marker. For a list of <paramref name="pdwVisualFlags"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.MARKERVISUAL"/>. 
        ///                 </param>
        public virtual int GetVisualStyle(out uint pdwVisualFlags)
        {
            pdwVisualFlags = (uint)(MARKERVISUAL.MV_COLOR_ALWAYS | MARKERVISUAL.MV_BORDER);
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the default foreground and background colors for a marker.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="piForeground">[out] Pointer to the default foreground color. For a list of <paramref name="piForeground"/> values, see COLORINDEX4C54D3F1-4AEB-497F-8311-2AB027C8BAD8.
        ///                 </param><param name="piBackground">[out] Pointer to the default background color. For a list of <paramref name="piBackground"/> values, see COLORINDEX4C54D3F1-4AEB-497F-8311-2AB027C8BAD8.
        ///                 </param>
        public virtual int GetDefaultColors(COLORINDEX[] piForeground, COLORINDEX[] piBackground)
        {
            piForeground[0] = COLORINDEX.CI_USERTEXT_FG;
            piBackground[0] = COLORINDEX.CI_USERTEXT_BK;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the default line attributes for a custom marker type.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="piLineColor">[out] Pointer to the default line color. For a list of <paramref name="piLineColor"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.COLORINDEX"/>. 
        ///                 </param><param name="piLineIndex">[out] Pointer to the default line style. For a list of <paramref name="piLineIndex"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.LINESTYLE"/>.
        ///                 </param>
        public virtual int GetDefaultLineStyle(COLORINDEX[] piLineColor, LINESTYLE[] piLineIndex)
        {
            piLineIndex[0] = LINESTYLE.LI_SOLID;
            piLineColor[0] = COLORINDEX.CI_MAGENTA;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Specifies additional modifications to text appearance determined by the marker.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pdwFontFlags">[out] Pointer to additional font options for markers. For a list of <paramref name="pdwFontFlags"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.FONTFLAGS"/>.
        ///                 </param>
        public virtual int GetDefaultFontFlags(out uint pdwFontFlags)
        {
            pdwFontFlags = (uint)FONTFLAGS.FF_DEFAULT;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Draws a glyph in the given display context and bounding rectangle using the provided colors.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="hdc">[in] Handle to a display device context that defines the visible region of interest.
        ///                 </param><param name="pRect">[in] Pointer to a RECT structure that defines the bounding rectangle for the marker.
        ///                 </param><param name="iMarkerType">[in] Integer containing the marker type.
        ///                 </param><param name="pMarkerColors">[in] Pointer to a marker colors object.
        ///                 </param><param name="dwGlyphDrawFlags">[in] Options for drawing the glyph in the widget margin. For a list of <paramref name="dwGlyphDrawFlags"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.GLYPHDRAWFLAGS"/></param><param name="iLineHeight">[in] Integer specifying the line height.
        ///                 </param>
        public virtual int DrawGlyphWithColors(IntPtr hdc, RECT[] pRect, int iMarkerType, IVsTextMarkerColorSet pMarkerColors, uint dwGlyphDrawFlags, int iLineHeight)
        {
            return VSConstants.E_NOTIMPL;
        }

        /// <summary>
        /// Controls how the marker tracks text when edits occur.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pdwFlags">[out] Pointer to flags specifying how the marker tracks text when edits occur. For a list of <paramref name="pdwFlags"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.MARKERBEHAVIORFLAGS"/>.
        ///                 </param>
        public virtual int GetBehaviorFlags(out uint pdwFlags)
        {
            pdwFlags = (uint)(MARKERBEHAVIORFLAGS.MB_DEFAULT);
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the priority index for the custom marker type, with the highest priority value receiving the topmost placement.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="piPriorityIndex">[out] Pointer to the priority index for the type of text marker. For a list of <paramref name="piPriorityIndex"/> values, see <see cref="T:Microsoft.VisualStudio.TextManager.Interop.MARKERTYPE"/>.
        ///                 </param>
        public virtual int GetPriorityIndex(out int piPriorityIndex)
        {
            piPriorityIndex = 200; // same as MARKERTYPE.MARKER_BOOKMARK;
            return VSConstants.S_OK;
        }

        #endregion

        #region Implementation of IVsMergeableUIItem

        /// <summary>
        /// Returns non-localized item name, used for comparison in inter-language merging of items.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pbstrNonLocalizeName">[out] String containing the canonical name.
        ///                 </param>
        public virtual int GetCanonicalName(out string pbstrNonLocalizeName)
        {
            pbstrNonLocalizeName = "Easy VSX Base Marker";
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the localized item name used for display in UI.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pbstrDisplayName">[out] String containing the display name.
        ///                 </param>
        public virtual int GetDisplayName(out string pbstrDisplayName)
        {
            pbstrDisplayName = "Easy VSX Base Marker";
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the merging priority.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="piMergingPriority">[out] Priority
        ///                 </param>
        public virtual int GetMergingPriority(out int piMergingPriority)
        {
            piMergingPriority = 0x2001;
            return VSConstants.S_OK;
        }

        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pbstrDesc">[out]
        ///                 </param>
        public virtual int GetDescription(out string pbstrDesc)
        {
            pbstrDesc = "Easy VSX Base Marker";
            return VSConstants.S_OK;
        }

        #endregion
    }
}
