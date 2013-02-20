using System;
using System.Drawing;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Collections.Generic;
using Microsoft.VisualStudio;

namespace easyvsx.VSObject
{
    public class VSTextView : VSBase
    {
        #region - 变量 -

        private static IVsTextManager textManager;

        private IVsShortcutManager shortCutMananger;
        private static IVsTextBuffer activeTextBuffer;
        private IVsTextLines textLines;
        private TextViewWindow window;



        #endregion

        #region - 属性 -

        public IVsTextView TextView { get; set; }

        public static IVsTextManager TextManager
        {
            get
            {
                if (textManager == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsTextManager));
                    if (obj == null)
                    {
                        throw new ArgumentException("get textmanager failed in VSTextView");
                    }
                    textManager = obj as IVsTextManager;
                }
                return textManager;
            }
        }

        /// <summary>
        /// 获得当前活动的Visual Studio文本视图
        /// </summary>
        public static IVsTextView ActiveTextView
        {
            get
            {
                IVsTextView activeView = null;
                //只要TextManager不为空，则每次都获取最新的TextView对象
                if (TextManager != null)
                {
                    TextManager.GetActiveView(1, null, out activeView);
                }
                return activeView;
            }
        }

        //public static IVsTextBuffer ActiveTextBuffer
        //{
        //    get
        //    {
        //        IVsTextView activeTextView = ActiveTextView;
        //        return activeTextBuffer;
        //    }
        //}

        /// <summary>
        /// 获得TextView对象的TextLines属性
        /// </summary>
        public IVsTextLines TextLines
        {
            get
            {
                if (TextView != null)
                {
                    TextView.GetBuffer(out textLines);
                }
                return textLines;
            }
        }

        /// <summary>
        /// 获得当前的选中对象，如果没有文档则返回NULL，如果当前没有选中返回""
        /// </summary>
        public static TextSelection ActiveTextSelection
        {
            get
            {
                if (ApplicationObject.ActiveDocument == null)
                {
                    return null;
                }
                return ApplicationObject.ActiveDocument.Selection as TextSelection;
            }
        }

        public IVsShortcutManager ShortCutManagerObject
        {
            get
            {
                if (TextManager != null && shortCutMananger == null)
                {
                    TextManager.GetShortcutManager(out shortCutMananger);
                }
                return shortCutMananger;
            }
        }

        public static TextDocument ActiveTextDocument
        {
            get { return ApplicationObject.ActiveDocument.Object("TextDocument") as TextDocument; }
        }

        /// <summary>
        /// 将一系列文本操作置为一组对象便于undo
        /// </summary>
        public static UndoContext UndoContext
        {
            get
            {
                return ApplicationObject.UndoContext;
            }
        }

        /// <summary>
        /// 获得此textView的窗口句柄
        /// </summary>
        public IntPtr WindowHandle
        {
            get
            {
                if (TextView != null)
                {
                    return TextView.GetWindowHandle();
                }
                return IntPtr.Zero;
            }
        }

        #endregion

        #region - 构造函数 -

        public VSTextView(IVsTextView vsTextView)
        {
            TextView = vsTextView;
        }

        #endregion

        #region - 公开方法 -

        #region - 文本操作 -

        /// <summary>
        /// 用文字替换选中的文本值
        /// </summary>
        /// <param name="text"></param>
        public void ReplaceSelectedText(string text)
        {
            ActiveTextDocument.Selection.Insert(text);
        }

        /// <summary>
        /// 在指定位置插入文本从(0,0)开始
        /// </summary>
        /// <param name="text"></param>
        /// <param name="line"></param>
        /// <param name="col"></param>
        public void InsertText(string text, int line, int col)
        {
            EditPoint editPoint = GetEditPoint(line, col);
            if (editPoint != null)
            {
                editPoint.Insert(text);
            }
        }

        public void LineDown(int line,int lineCount)
        {
            EditPoint editPoint = GetEditPoint(line, 0);
            if (editPoint != null)
            {
                editPoint.LineDown(lineCount);
            }
        }

        /// <summary>
        /// 删除指定位置的文字，从(0,0)开始
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="startCol"></param>
        /// <param name="endLine"></param>
        /// <param name="endCol"></param>
        public void DeleteText(int startLine, int startCol, int endLine, int endCol)
        {
            EditPoint editPoint = GetEditPoint(startLine, startCol);
            if (editPoint != null)
            {
                object tp;
                TextLines.CreateTextPoint(endLine, endCol, out tp);
                editPoint.Delete(tp as TextPoint);
            }
        }

        #endregion

        #region - 注释 comment -

        public void CommentSelectedText()
        {
            TextSelection textSelection = ActiveTextSelection;
            if (textSelection != null)
            {
                //editpoint,virsualpoint 都继承于TextPoint
                EditPoint topEditPoint = textSelection.TopPoint.CreateEditPoint();
                TextPoint bottomPoint = textSelection.BottomPoint;

                UndoContext undoContext = ApplicationObject.UndoContext;
                undoContext.Open("Comment Region");
                while (topEditPoint.LessThan(bottomPoint))
                {
                    topEditPoint.Insert("//");
                    topEditPoint.LineDown();
                    topEditPoint.StartOfLine();
                }
                undoContext.Close();
            }
        }

        #endregion

        /// <summary>
        /// 获得整个TextView的TextSpan信息,(坐标从0,0开始)
        /// </summary>
        /// <returns></returns>
        public TextSpan GetWholeTextSpan()
        {
            var span = new TextSpan();
            span.iStartIndex = span.iStartLine = 0;
            if (TextLines != null)
            {
                TextLines.GetLastLineIndex(out span.iEndLine, out span.iEndIndex);
            }
            return span;
        }

        /// <summary>
        /// 获得当前活动文档的代码行数
        /// </summary>
        /// <returns></returns>
        public int GetLines()
        {
            TextSpan documentSpan = GetWholeTextSpan();
            return documentSpan.iEndLine;
        }

        /// <summary>
        /// 获得指定位置的文本（从0，0坐标开始）
        /// </summary>
        /// <param name="textSpan"></param>
        /// <returns></returns>
        public string GetText(TextSpan textSpan)
        {
            string text;
            TextView.GetTextStream(textSpan.iStartLine, textSpan.iStartIndex, textSpan.iEndLine,
                                                    textSpan.iEndIndex, out text);
            return text;
        }

        /// <summary>
        /// 获得指定位置的文本（从0,0坐标开始）
        /// </summary>
        public string GetText(int startLine, int startCol, int endLine, int endIndex)
        {
            string text;
            TextView.GetTextStream(startLine, startCol, endLine, endIndex, out text);
            return text;
        }

        /// <summary>
        /// 获得这个TextView的文本
        /// </summary>
        /// <returns></returns>
        public string GetWholeText()
        {
            return GetText(GetWholeTextSpan());
        }

        /// <summary>
        /// 获取某一行的文字，行号从0开始
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <returns></returns>
        public string GetOneLineText(int lineIndex)
        {
            return GetText(lineIndex, 0, lineIndex + 1, 0);
        }

        public int GetStartTextIndexOfCurrLine()
        {
            int line, col;
            GetCursorPositon(out line, out col);
            string text = GetOneLineText(line);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ')
                {
                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获得当前选中状态下的文本
        /// </summary>
        /// <returns></returns>
        public string GetSelectedText()
        {
            string selectedText = string.Empty;

            if (ActiveTextView != null)
            {
                ActiveTextView.GetSelectedText(out selectedText);
            }

            return selectedText;
        }

        /// <summary>
        /// 获得当前选中文本的位置信息
        /// </summary>
        /// <returns></returns>
        public TextSpan GetSelectedSpan()
        {
            TextSpan[] ts = new TextSpan[1];

            if (ActiveTextView != null)
            {
                ActiveTextView.GetSelectionSpan(ts);
            }
            return ts[0];
        }

        /// <summary>
        /// 替换指定位置的文字
        /// </summary>
        /// <param name="line">要替换的行号</param>
        /// <param name="col">要替换的列号</param>
        /// <param name="text">新的替换文字</param>
        public void ReplaceText(int line, int col, string text)
        {
            if (TextView != null)
            {
                TextView.ReplaceTextOnLine(line, col, text.Length, text, text.Length);
            }
        }

        /// <summary>
        /// 将光标移动到指定位置（从(0,0)开始）
        /// </summary>
        /// <param name="line"></param>
        /// <param name="col"></param>
        public void MoveCursorTo(int line, int col)
        {
            if (line < 0 || col < 0)
            {
                return;
            }

            if (ActiveTextView != null)
            {
                ActiveTextView.SetCaretPos(line, col);
            }
        }

        /// <summary>
        /// 获得光标当前位置  从(0,0)开始
        /// </summary>
        /// <param name="line"></param>
        /// <param name="col"></param>
        public void GetCursorPositon(out int line, out int col)
        {
            if (ActiveTextView != null)
            {
                ActiveTextView.GetCaretPos(out line, out col);
            }
            else
            {
                line = col = -1;
            }
        }

        /// <summary>
        /// 设置光标当前位置  从(0,0)开始
        /// </summary>
        /// <param name="line"></param>
        /// <param name="col"></param>
        /// <returns>VSConstants.S_OK表示成功，S_FALSE失败</returns>
        public int SetCursorPositon(int line, int col)
        {
            if (ActiveTextView != null)
            {
                return ActiveTextView.SetCaretPos(line, col);
            }

            return VSConstants.S_FALSE;
        }

        /// <summary>
        /// 选中指定段落
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void SelectText(Point start, Point end)
        {
            if (TextView != null)
            {
                TextView.SetSelection(start.Y, start.X, end.Y, end.X);
            }
        }

        /// <summary>
        /// 选中指定段落
        /// </summary>
        /// <param name="textSpan"></param>
        public void SelectText(TextSpan textSpan)
        {
            if (TextView != null)
            {
                TextView.SetSelection(textSpan.iStartLine, textSpan.iStartIndex, textSpan.iEndLine, textSpan.iEndIndex);
            }
        }

        /// <summary>
        /// 选中指定的文字段落
        /// </summary>
        public void SelectText(int lineStartNumber, int colStartNumber, int lineEndNumber, int colEndNumber)
        {
            if (ActiveTextView != null)
            {
                ActiveTextView.SetSelection(lineStartNumber, colStartNumber, lineEndNumber, colEndNumber);
            }
        }

        #endregion

        #region - 私有方法 -

        /// <summary>
        /// 获得指定位置的编辑点，从(0,0)开始
        /// </summary>
        /// <param name="line"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public EditPoint GetEditPoint(int line, int col)
        {
            if (TextLines != null)
            {
                object editPoint;
                TextLines.CreateEditPoint(line, col, out editPoint);
                if (editPoint != null)
                {
                    return editPoint as EditPoint;
                }
            }
            return null;
        }

        #endregion

    }
}