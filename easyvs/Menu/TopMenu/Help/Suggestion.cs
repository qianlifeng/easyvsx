using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using easyvsx;
using easyVS.Forms.Help;
using Easy;

namespace easyVS.Menu.TopMenu.Help
{
    [CommandID(GuidList.guidPackageTestCmdSetString, PkgCmdIDList.cmdSuggestion)]
    public class Suggestion : MenuCommand
    {
        public Suggestion(PackageBase package)
            : base(package)
        {

        }

        protected override void OnQueryStatus(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {

        }

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            SuggestFrm about = new SuggestFrm();
            about.ShowDialog();
        }
    }
}
