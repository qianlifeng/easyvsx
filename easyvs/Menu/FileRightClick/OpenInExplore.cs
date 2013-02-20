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
    [CommandID(GuidList.FileRightClickCmdSetString, PkgCmdIDList.FileRightClick_OpenInExplore)]
    public class OpenInExplore : MenuCommand
    {
        #region - 构造函数 -

        public OpenInExplore(PackageBase package)
            : base(package)
        {
        }

        #endregion

        #region - 方法 -

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            UIHierarchy solutionExplorer = VSBase.ApplicationObject.ToolWindows.SolutionExplorer;
            Array items = solutionExplorer.SelectedItems as Array;
            if (items.Length == 0)
            {
                return;
            }

            if (items.Length == 1)
            {
                //选择了一个文件
                UIHierarchyItem hierarchyItem = items.GetValue(0) as UIHierarchyItem;
                ProjectItem prjItem = hierarchyItem.Object as ProjectItem;
                string prjPath = prjItem.Properties.Item("FullPath").Value.ToString();

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "explorer";
                proc.StartInfo.Arguments = @"/select," + prjPath;
                proc.Start();
            }
        }

        #endregion
    }
}
