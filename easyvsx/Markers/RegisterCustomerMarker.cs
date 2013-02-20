using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;

namespace easyvsx.Markers
{
    public class RegisterCustomerMarker : RegistrationAttribute
    {
        public override void Register(RegistrationContext context)
        {
            string rf = context.RootFolder;
            Key markerkey = context.CreateKey("Text Editor\\External Markers\\{" + GuidConstants.BaseMarkerGuid + "}");
            markerkey.SetValue("", "My Custom Text Marker");
            markerkey.SetValue("Service", "{" + typeof(BaseMarker).GUID + "}");
            markerkey.SetValue("DisplayName", "My Custom Text Marker");
            markerkey.SetValue("Package", "{" + GuidConstants.guidPackageTestPkgString + "}");
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            context.RemoveKey("Text Editor\\External Markers\\{" + GuidConstants.BaseMarkerGuid + "}");
        }
    }
}
