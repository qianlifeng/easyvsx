using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace easyvsx.VSObject
{
    /// <summary>
    /// 提供对Document的操作支持
    /// </summary>
    public class VSDocument : VSBase
    {
        /// <summary>
        /// 当前活动的Document
        /// </summary>
        public static Document ActiveDocument
        {
            get
            {
                return ApplicationObject.ActiveDocument;
            }
        }

        public static TextPoint ActivePoint
        {
            get
            {
                TextSelection sel = (TextSelection)ActiveDocument.Selection;
                if (sel != null)
                {
                    TextPoint pnt = (TextPoint)sel.ActivePoint;
                    return pnt;
                }
                return null;
            }
        }

        /// <summary>
        /// 保存当前文档
        /// </summary>
        public static void SaveActiveDocument()
        {
            ActiveDocument.Save();
        }

        /// <summary>
        /// 获得所有已经打开的文档（不可见的不算）
        /// </summary>
        /// <returns></returns>
        public static List<Document> GetAllOpenedDocument()
        {
            List<Document> allOpen = new List<Document>();
            Documents documents = ApplicationObject.Documents;
            foreach (Document document in documents)
            {
                if (document.FullName != null && document.Windows.Count > 0)
                {
                    allOpen.Add(document);
                }
            }
            return allOpen;
        }

        /// <summary>
        /// 关闭一个已经打开的文档
        /// </summary>
        /// <param name="name">文档路径（包含文件后缀）</param>
        /// <param name="saveOption">制定如何关闭一个文档，默认为强制保存后关闭</param>
        public static void CloseDocument(string name, __VSSLNSAVEOPTIONS saveOption = __VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave)
        {
            IVsRunningDocumentTable runningTabs = RunningDocumentTableObject;
            IVsSolution solution = VSSolution.SolutionObject;

            if (runningTabs != null)
            {
                IEnumRunningDocuments runningDocuments;
                runningTabs.GetRunningDocumentsEnum(out runningDocuments);

                IntPtr documentData = IntPtr.Zero;
                uint[] docCookie = new uint[1];
                uint fetched;

                //遍历所有已经打开的文档
                while ((VSConstants.S_OK == runningDocuments.Next(1, docCookie, out fetched)) && (fetched == 1))
                {
                    uint flags;
                    uint editLocks;
                    uint readLocks;
                    string filePath;
                    IVsHierarchy docHierarchy;
                    uint docId;
                    IntPtr docData = IntPtr.Zero;

                    try
                    {
                        ErrorHandler.ThrowOnFailure(runningTabs.GetDocumentInfo(docCookie[0], out flags, out readLocks,
                                                                                out editLocks, out filePath, out docHierarchy,
                                                                                out docId, out docData));

                        if (solution != null && !filePath.EndsWith("sln") && !filePath.EndsWith("csproj"))
                        {
                            //string currentName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                            if (name == filePath)
                            {
                                solution.CloseSolutionElement((uint)saveOption, docHierarchy, docCookie[0]);
                            }
                        }
                    }
                    finally
                    {
                        if (docData != IntPtr.Zero)
                        {
                            Marshal.Release(docData);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 关闭当前文档
        /// </summary>
        public static void CloseCurrentDocument()
        {
            Document activeDocument = ApplicationObject.ActiveDocument;
            if (activeDocument != null)
            {
                CloseDocument(activeDocument.FullName);
            }
        }

        public static TextDocument GetTextDocument(Document doc)
        {
            return doc.Object("TextDocument") as TextDocument;
        }

        /// <summary>
        /// 在给定的document中插入文字
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="line">从1开始</param>
        /// <param name="column">从1开始</param>
        /// <param name="text"></param>
        public static void Insert(Document doc, int line, int column, string text)
        {
            TextDocument td = GetTextDocument(doc);
            if (td != null)
            {
                EditPoint editPoint = td.CreateEditPoint(td.StartPoint);
                editPoint.MoveToLineAndOffset(line, column);
                editPoint.Insert(text);
            }
        }
    }
}
