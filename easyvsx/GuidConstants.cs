using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace easyvsx
{
    public class GuidConstants
    {
        public const string BaseMarkerProviderGuidString = "FC02C1B1-7CC5-4E01-8BC7-11B7F50609D5";
        public static readonly Guid BaseMarkerProviderGuid = new Guid(BaseMarkerProviderGuidString);

        public const string BaseMarkerGuidString = "F19B61D3-1646-4713-A0F5-2A0961333E99";
        public static readonly Guid BaseMarkerGuid = new Guid(BaseMarkerProviderGuidString);

        public const string guidPackageTestPkgString = "ad30597b-10a5-4848-a122-a71d2602eef3";
    }
}
