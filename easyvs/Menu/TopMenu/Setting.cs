using System.Windows.Forms;
using easyvsx;
using easyvsx.VSObject;
using easyVS.Forms;
using Easy;

namespace EasyCodeGenerate.Menu.TopMenu
{
    [CommandID(GuidList.guidPackageTestCmdSetString, PkgCmdIDList.cmdSetting)]
    public class Setting : MenuCommand
    {
        public Setting(PackageBase package)
            : base(package)
        {
        }

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            SettingFrm frm = new SettingFrm();
            frm.ShowDialog();
        }

    }
}
