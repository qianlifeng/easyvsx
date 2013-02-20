using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;

namespace easyvsx.Markers
{
    [Guid(GuidConstants.BaseMarkerProviderGuidString)]
    [ComVisible(true)]
    public class MarkerTypeProvider : IVsTextMarkerTypeProvider,  Microsoft.VisualStudio.OLE.Interop.IServiceProvider
    {
       
        /// <summary>
        /// Allows you to return a pointer to your <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsPackageDefinedTextMarkerType"/> implementation for a custom marker type.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="pguidMarker">[in] Pointer to a GUID identifying the external marker type. </param>
        /// <param name="ppMarkerType">[out] Pointer to the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsPackageDefinedTextMarkerType"/> implementation for the external marker type.</param>
        public int GetTextMarkerType(ref Guid pguidMarker, out IVsPackageDefinedTextMarkerType ppMarkerType)
        {
            if (pguidMarker == GuidConstants.BaseMarkerProviderGuid)
            {
                ppMarkerType = new BaseMarker();
                return VSConstants.S_OK;
            }
            ppMarkerType = null;
            return VSConstants.E_UNEXPECTED;
        }

        public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            ppvObject = IntPtr.Zero;
            if (guidService == GuidConstants.BaseMarkerProviderGuid && riid == typeof(IVsTextMarkerTypeProvider).GUID)
            {
                IntPtr tmp = Marshal.GetIUnknownForObject(this);
                return Marshal.QueryInterface(tmp, ref riid, out ppvObject);
            }
            return VSConstants.E_NOINTERFACE;
        }

    }
}
