using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using easyvsx;
using Easy;
using EnvDTE;
using easyvsx.VSObject;
using System.Windows.Forms;

namespace easyVS.Menu.FileRightClick
{
    [CommandID(GuidList.FileMenuCmdSetString, PkgCmdIDList.FileMenu_RestartVS)]
    public class RestartVS : MenuCommand
    {

        public RestartVS(PackageBase package)
            : base(package)
        {
        }

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            VSBase.ReStartVS();
        }
    }
}
