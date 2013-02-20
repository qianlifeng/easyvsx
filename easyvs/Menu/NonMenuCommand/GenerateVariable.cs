using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using easyvsx;
using Easy;
using System.Windows.Forms;
using easyvsx.VSObject;
using EnvDTE;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.CSharp.Services.Language.Interop;
using Microsoft.RestrictedUsage.CSharp.Core;
using Microsoft.RestrictedUsage.CSharp.Syntax;
using Microsoft.RestrictedUsage.CSharp.Semantics;
using Microsoft.RestrictedUsage.CSharp.Compiler.IDE;
using Microsoft.VisualStudio.CSharp.Services.Language.CallHierarchy.SearchEngine;

namespace easyVS.Menu.NonMenuCommand
{
    [CommandID(GuidList.NonMenuCommand_CmdSetString, PkgCmdIDList.NonMenuCommand_GenerateVariable)]
    public class GenerateVariable : MenuCommand
    {
        #region - Constructor -

        public GenerateVariable(PackageBase package)
            : base(package)
        {
        }

        #endregion

        #region - Methods -

        [DllImport("CSLangSvc.dll", PreserveSig = false)]
        public static extern void LangService_GetDteProject(ILangService langService, Project dteProject, out IProject project);

        [DllImport("CSLangSvc.dll", PreserveSig = false)]
        public static extern void LangService_GetInstance(out ILangService langService);

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            int curLineIndex, curColumnIndex;
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            if (textView == null)
            {
                MessageBox.Show("textView is null");
                return;
            }

            textView.GetCursorPositon(out curLineIndex, out curColumnIndex);

            //帮助：http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/c62a70eb-d84c-4410-890d-b41a86b2b55f/
            ILangService langService;
            LangService_GetInstance(out langService);
            if (langService == null)
            {
                MessageBox.Show("langService is null");
                return;
            }

            IDECompilerHost compilerHost = new IDECompilerHost();

            IProject prj;
            Project currentPro = VSDocument.ActiveDocument.ProjectItem.ContainingProject;
            LangService_GetDteProject(langService, currentPro, out prj);
            if (prj == null)
            {
                MessageBox.Show("prj is null");
                return;
            }

            FileName fileName = new Microsoft.RestrictedUsage.CSharp.Core.FileName(VSDocument.ActiveDocument.FullName);
            CSRPOSDATA data = new CSRPOSDATA() { LineIndex = curLineIndex, ColumnIndex = curColumnIndex };
            CSharpMember m = Utilities.GetMemberAtLocation(compilerHost, prj, fileName.Value, data);
            
            if (m != null && m.ReturnType != null)
            {

                string returnName = m.ReturnType.GetFormattedName(InteropTypeFormatFlags.TypeFormatFlags_BestNameGUI);

                returnName = FilterReturnName(returnName);
                if (string.IsNullOrEmpty(returnName))
                {
                    return;
                }

                int insertCol = GetInsertPosition(textView, curLineIndex, curColumnIndex);
                textView.InsertText(returnName + " v =", curLineIndex, insertCol);
                textView.SelectText(curLineIndex, insertCol + returnName.Length + 1, curLineIndex, insertCol + returnName.Length + 2);
            }

          
        }

        private string FilterReturnName(string returnName)
        {
            if (returnName == "void")
            {
                return string.Empty;
            }

            return returnName;
        }

        private int GetInsertPosition(VSTextView textView, int curLineIndex, int curColumnIndex)
        {
            //在鼠标之前的第一个空格的地方插入
            int col = curColumnIndex;
            string text = textView.GetOneLineText(curLineIndex);
            while (text[col] != ' ')
            {
                col--;
            }

            return col;
        }

        #endregion
    }
}
