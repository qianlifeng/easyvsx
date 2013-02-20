using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using easyvsx;
using Easy;
using easyvsx.VSObject;
using System.Windows.Forms;
using Microsoft.VisualStudio.TextManager.Interop;
using easyVS.Forms;
using System.Runtime.InteropServices;
using EnvDTE;

namespace easyVS.Menu.CodeRightClick
{
    [CommandID(GuidList.CodeRightClick_CmdSetString, PkgCmdIDList.CodeRightClick_MoveToRegion)]
    public class MoveToRegion : MenuCommand
    {
        public MoveToRegion(PackageBase package)
            : base(package)
        {

        }

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            VSCodeModel codeModel = new VSCodeModel();
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            if (string.IsNullOrEmpty(textView.GetSelectedText().Trim()))
            {
                MessageBox.Show("please select valid text");
                return;
            }

            //需要判断选中的内容属于哪个类或结构体下面
            TextSpan ts = textView.GetSelectedSpan();
            CodeElement classElement = GetClassElementSelectionBelong(textView, codeModel);
            if (classElement == null)
            {
                MessageBox.Show("Can't detect which class or struct the selection belongs to");
                return;
            }

            MoveToRegionForm form = new MoveToRegionForm(classElement);
            form.OnSelectRegion += form_OnSelectRegion;
            form.ShowDialog();
        }

        private void form_OnSelectRegion(RegionElement region)
        {
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            TextSpan ts = textView.GetSelectedSpan();

            string cutStr = textView.GetText(ts.iStartLine, 0, ts.iEndLine + 1, 0);
            //if (!cutStr.StartsWith("\r\n"))
            //{
            //    cutStr = "\r\n" + cutStr;
            //}
            //if (!cutStr.EndsWith("\r\n"))
            //{
            //    cutStr = cutStr + "\r\n";
            //}

            //判断要移动的文本和region的关系
            if (ts.iStartLine > region.EndLine)
            {
                //要移动的文本在region下面，此时需要先删除再插入。才能让传过来的insertLine有效
                textView.DeleteText(ts.iStartLine, 0, ts.iEndLine + 1, 0);
                textView.InsertText(cutStr, region.EndLine, 0);
                InsertBlankLineAroundInsert(region.EndLine, ts.iEndLine - ts.iStartLine);
            }
            else if (ts.iEndLine < region.StartLine)
            {
                //文本在region上面,先插入再删除
                textView.InsertText(cutStr, region.EndLine, 0);
                textView.DeleteText(ts.iStartLine, 0, ts.iEndLine + 1, 0);
                //文本删除后，因为region要往上移动，所以这里的region实际位置发生了变化
                InsertBlankLineAroundInsert(region.EndLine - (ts.iEndLine - ts.iStartLine) - 1, ts.iEndLine - ts.iStartLine);
            }
            else
            {
                MessageBox.Show("Selected text has intersection with this region, can't handle request.");
            }
        }
        /// <summary>
        /// 监测插入的文本位置上下是否有一行空白，如果没有则自动添加
        /// </summary>
        /// <param name="insertLine"></param>
        /// <param name="insertLineCount"></param>
        private void InsertBlankLineAroundInsert(int insertLine, int insertLineCount)
        {
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            int endLine = insertLine + insertLineCount + 1;
            if (!string.IsNullOrEmpty(textView.GetOneLineText(endLine)))
            {
                textView.InsertText("\r\n", endLine, 0);
            }

            int startLine = insertLine - 1;
            if (!string.IsNullOrEmpty(textView.GetOneLineText(startLine)))
            {
                textView.InsertText("\r\n", startLine + 1, 0);
            }

        }

        /// <summary>
        /// 得到选中部分所在的classElement
        /// </summary>
        /// <returns></returns>
        private CodeElement GetClassElementSelectionBelong(VSTextView textView, VSCodeModel codeModel)
        {
            TextSpan ts = textView.GetSelectedSpan();
            return codeModel.GetClassInCurrentFile().Where(o =>
                o.StartPoint.Line < ts.iStartLine
                && o.EndPoint.Line > ts.iEndLine).FirstOrDefault();
        }
    }
}
