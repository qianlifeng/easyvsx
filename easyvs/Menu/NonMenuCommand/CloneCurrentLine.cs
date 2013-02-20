/******************************************************************************
 *  Author：       SQ1000
 *  CreateDate：   17/02/2012 10:37:49 AM
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using easyvsx;
using easyvsx.VSObject;
using Easy;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using EnvDTE;
using EnvDTE80;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.CSharp.Services.Language.Interop;

namespace EasyCodeGenerate.Menu.NonMenuCommand
{
    [CommandID(GuidList.NonMenuCommand_CmdSetString, PkgCmdIDList.NonMenuCommand_CloneCurrentLine)]
    /// <summary>
    /// 克隆当前行到下一行
    /// </summary>
    public class CloneCurrentLine : MenuCommand
    {
        #region - Constructor -

        public CloneCurrentLine(PackageBase package)
            : base(package)
        {
        }

        #endregion

        #region - Methods -

        protected override void OnQueryStatus(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {

        }

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            TextSelection selection = VSTextView.ActiveTextSelection;
            if (selection != null)
            {
                string selectedText = selection.Text;
                if (string.IsNullOrEmpty(selectedText))
                {
                    VSTextView view = new VSTextView(VSTextView.ActiveTextView);
                    if (view != null)
                    {
                        selectedText = view.GetOneLineText(selection.TopPoint.Line - 1);
                        view.InsertText(selectedText, selection.TopPoint.Line, 0);
                    }
                }
                else
                {
                    selection.Insert(selectedText + "\n" + selectedText);
                }
            }

        }

        #endregion

    }
}
