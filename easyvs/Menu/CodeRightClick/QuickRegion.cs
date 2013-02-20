using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using Easy;
using EasyCodeGenerate.Core;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;
using easyvsx;
using easyvsx.VSObject;
using easyVS.Forms;
using easyVS.Forms.Setting.UC;
using System.Collections;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace EasyCodeGenerate.Menu.CodeRightClick
{
    [CommandID(GuidList.CodeRightClick_CmdSetString, PkgCmdIDList.CodeRIghtClick_QuickRegion)]
    public class QuickRegion : MenuCommand
    {
        #region - 构造函数 -

        public QuickRegion(PackageBase package)
            : base(package)
        {
        }

        #endregion

        #region - 方法 -

        protected override void OnQueryStatus(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            ////只有在.cs文件上单击才显示菜单
            //string fullPath = VSDocument.ActiveDocument.FullName;
            //string fileName = fullPath.Substring(fullPath.LastIndexOf('\\') + 1);
            //string suffix = fileName.Substring(fileName.IndexOf('.') + 1);
            //if (suffix != "cs")
            //{
            //    command.Visible = false;
            //    command.Enabled = false;
            //}
            //else
            //{
            //    command.Visible = true;
            //    command.Enabled = true;
            //}
        }

        protected override void OnExecute(Microsoft.VisualStudio.Shell.OleMenuCommand command)
        {
            if (VSTextView.ActiveTextView == null)
            {
                return;
            }

            long getElement = 0;
            long regionTime = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //开始之前先格式化
            VSBase.ExecuteCommand((uint)VSConstants.VSStd2KCmdID.FORMATDOCUMENT);
            VSStatusBar.SetText("quick region......");
            using (VSUndo.StartUndo())
            {
                SettingModel model = SettingFrm.ReadSetting();
                if (model != null)
                {
                    QuickRegionSettingModel quickRegionModel = model.QuickRegionModel;
                    VSCodeModel codeModel = new VSCodeModel();
                    List<CodeElement> classLists = GetClassAndStructInFile(codeModel);

                    for (int i = 0; i < classLists.Count; i++)
                    {
                        
                        sw.Stop();
                        getElement = sw.ElapsedMilliseconds;
                        sw.Restart();

                        List<CodeElement> noneEventElements = codeModel.GetNotRegionNoneEventMethodInClass(classLists[i]);
                        if (noneEventElements.Count != 0)
                        {
                            RegionElement(noneEventElements, i, model.QuickRegionModel.Method);
                        }

                        List<CodeElement> eventElements = codeModel.GetNotRegionEventInClass(classLists[i]);
                        if (eventElements.Count != 0)
                        {
                            RegionElement(eventElements, i, model.QuickRegionModel.Event);
                        }

                        List<CodeElement> constructorElements = codeModel.GetNotRegionConstructorInClass(classLists[i]);
                        if (constructorElements.Count != 0)
                        {
                            RegionElement(constructorElements, i, model.QuickRegionModel.Constructor);
                        }

                        List<CodeElement> propertyElements = codeModel.GetNotRegionPropertyInClass(classLists[i]);
                        if (propertyElements.Count != 0)
                        {
                            RegionElement(propertyElements, i, model.QuickRegionModel.Property);
                        }

                        List<CodeElement> variablesElements = codeModel.GetNotRegionVariablesInClass(classLists[i]);
                        if (variablesElements.Count != 0)
                        {
                            RegionElement(variablesElements, i, model.QuickRegionModel.Variable);                            
                        }

                        sw.Stop();
                        regionTime = sw.ElapsedMilliseconds;
                        
                    }

                    //QuickRegionpNonEventMethod(textView, quickRegionModel.Method);
                    //QuickRegionpEventMethod(textView, quickRegionModel.Event);
                    //QuickRegionConstructor(textView, quickRegionModel.Constructor);
                    //QuickRegionpProperty(textView, quickRegionModel.Property);
                    //QuickRegionDelegates(textView, "- Delegate -");
                    //QuickRegionVariables(textView, quickRegionModel.Variable);
                    //CleanEmptyRegion(textView);
                    CleanBlankLine(codeModel);

                    VSDocument.SaveActiveDocument();
                    VSBase.ExecuteCommand((uint)VSConstants.VSStd2KCmdID.FORMATDOCUMENT);
                    VSBase.ExecuteCommand((uint)VSConstants.VSStd2KCmdID.OUTLN_COLLAPSE_TO_DEF);
                }
            }
            VsOutput.ShowDebugMessage("region complete, get element time total: " + getElement + " milliseconds\r\n"+" region time: "+regionTime);
        }

        private static List<CodeElement> GetClassAndStructInFile(VSCodeModel codeModel)
        {
            List<CodeElement> classLists = codeModel.GetClassInCurrentFile();
            classLists.AddRange(codeModel.GetStructInCurrentFile());
            return classLists;
        }

        public void RegionElement(List<CodeElement> needRegionElements, int classIndex, string regionName)
        {
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            VSCodeModel codeModel = new VSCodeModel();

            List<TextSpan> methodsBody = new List<TextSpan>();
            StringBuilder sb = new StringBuilder();
            foreach (CodeElement item in needRegionElements)
            {
                //在这里获得的位置是最新的修改后的位置。
                CodeElement ce = codeModel.GetPrevElementInCurrentFile(item);
                if (ce != null)
                {
                    TextSpan es = new TextSpan();
                    //这里的行数全部以(0,0)开始
                    es.iEndLine = item.EndPoint.Line - 1;
                    //当前元素的开始定位其上一个元素的结尾
                    es.iStartLine = ce.EndPoint.Line;

                    //当前元素是第一个元素，所以他的开始应该是紧接着他父元素
                    if (ce == item)
                    {
                        CodeElement parentElement = codeModel.GetParentElementInCurrentFile(item);
                        int parentElementStartLine = GetElementBodyStartLine(textView, parentElement);
                        es.iStartLine = parentElementStartLine;
                    }


                    //检查es.iStartLine到item.StartLine之间有没有#region或者#endregion，这些
                    //东西不能为算在里面，否则粘贴后会破坏现有的region
                    es.iStartLine = CutRegionFlag(textView, es.iStartLine, item.StartPoint.Line);

                    methodsBody.Add(es);
                    sb.Append(textView.GetText(es.iStartLine, 0, es.iEndLine + 1, 0));
                }
            }

            //开始删除原来的节点
            int filedCount = methodsBody.Count;
            for (int i = filedCount - 1; i >= 0; i--)
            {
                textView.DeleteText(methodsBody[i].iStartLine, 0, methodsBody[i].iEndLine + 1, 0);
            }

            //插入新的region包围的内容
            //首先检查当前是否已经存在给定的regionName，如果存在则不用新建

            if (filedCount > 0)
            {
                //需要重新获得删除节点后最新的class位置信息
                List<CodeElement> classLists = GetClassAndStructInFile(codeModel);
                int line = GetElementBodyStartLine(textView, classLists[classIndex]);
                
                //检查有没有已经同名的region
                int r = CheckExistRegionName(regionName, classLists[classIndex]);
                if (r != -1)
                {
                    line = r;
                }
                else
                {
                    sb.Insert(0, "\t\t#region " + regionName + "\r\n\r\n");
                    sb.Append("\t\n\t\t#endregion\r\n\r\n");
                }

                textView.InsertText(sb.ToString(), line, 0);
            }


        }

        private int CutRegionFlag(VSTextView textView, int topStart, int methodStart)
        {
            //这里要从下面往上找，不能从上往下找否则找到的第一个后面可能还会存在#region
            int current = methodStart;
            while (current > topStart)
            {
                string lineStr = textView.GetOneLineText(current).Trim();
                if (lineStr.StartsWith("#region") || lineStr.StartsWith("#endregion"))
                {
                    //返回#region 或者 #endregion的下一行作为分界线
                    return ++current;
                }

                current--;
            }

            return topStart;
        }

        /// <summary>
        /// 获得类体开始的行数
        /// </summary>
        /// <param name="textView"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private int GetElementBodyStartLine(VSTextView textView, CodeElement element)
        {
            int line = element.StartPoint.Line;
            int start = line;
            while (start < textView.GetLines())
            {
                if (textView.GetOneLineText(start).TrimStart().StartsWith("{"))
                {
                    line = ++start;
                    break;
                }
                start++;
            }
            return line;
        }

        /// <summary>
        /// 检查当前文件中是否存在相同的region名字，如果不存在则返回-1，否则返回该region的末尾位置
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="classElement"></param>
        /// <returns></returns>
        private int CheckExistRegionName(string regionName, CodeElement classElement)
        {
            VSCodeModel codeModel = new VSCodeModel();
            List<RegionElement> regions = codeModel.GetAllRegionsInClass(classElement);
            RegionElement re = regions.Where(o => o.RegionName == regionName).FirstOrDefault();
            if (re == null)
            {
                return -1;
            }
            else
            {
                return re.EndLine - 1;
            }
        }

        private void CleanBlankLine(VSCodeModel codeModel)
        {
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);
            List<TextSpan> blankLines = codeModel.GetBlankLines(textView, 3);  //超过3行空白的一律合并
            for (int i = blankLines.Count - 1; i >= 0; i--)
            {
                TextSpan emptyRegion = blankLines[i];

                textView.DeleteText(emptyRegion.iStartLine, 0, emptyRegion.iEndLine, 0);
            }
        }
        
        
//private void QuickRegionVariables(VSTextView textView, string name)
        //{
        //    //对于一个文件的整理，应该以class为组来进行
        //    List<TypeDeclaration> classDeclarations = NRefactoryAnalyse.GetDeclarations<TypeDeclaration>(textView.GetWholeText());

        //    //处理的时候采用从下至上的方法，从而避免影响后面的代码
        //    for (int index = classDeclarations.Count - 1; index >= 0; index--)
        //    {
        //        if (classDeclarations[index].Type == ClassType.Enum)
        //        {
        //            //不处理enum类型
        //            continue;
        //        }
        //        //List<FieldDeclaration> fieldDeclarations = NRefactoryAnalyse.GetDeclarations<FieldDeclaration>(classDeclarations[index]);
        //        List<FieldDeclaration> fieldDeclarations = NRefactoryAnalyse.GetDeclarationsNotInRegion<FieldDeclaration>(classDeclarations[index], textView.GetWholeText());

        //        BaseRegion(textView, classDeclarations[index], fieldDeclarations, name);
        //    }
        //}

        //private void QuickRegionDelegates(VSTextView textView, string name)
        //{
        //    //对于一个文件的整理，应该以class为组来进行
        //    List<TypeDeclaration> classDeclarations = NRefactoryAnalyse.GetDeclarations<TypeDeclaration>(textView.GetWholeText());

        //    //处理的时候采用从下至上的方法，从而避免影响后面的代码
        //    for (int index = classDeclarations.Count - 1; index >= 0; index--)
        //    {
        //        if (classDeclarations[index].Type == ClassType.Enum)
        //        {
        //            //不处理enum类型
        //            continue;
        //        }
        //        List<DelegateDeclaration> fieldDeclarations = NRefactoryAnalyse.GetDeclarationsNotInRegion<DelegateDeclaration>(classDeclarations[index], textView.GetWholeText());
        //        BaseRegion<DelegateDeclaration>(textView, classDeclarations[index], fieldDeclarations, name);
        //    }
        //}

        //public void QuickRegionpProperty(VSTextView textView, string name)
        //{
        //    //对于一个文件的整理，应该以class为组来进行
        //    List<TypeDeclaration> classDeclarations = NRefactoryAnalyse.GetDeclarations<TypeDeclaration>(textView.GetWholeText());

        //    //处理的时候采用从下至上的方法，从而避免影响后面的代码
        //    for (int index = classDeclarations.Count - 1; index >= 0; index--)
        //    {
        //        if (classDeclarations[index].Type == ClassType.Enum)
        //        {
        //            //不处理enum类型
        //            continue;
        //        }
        //        List<PropertyDeclaration> propertyDeclarations = NRefactoryAnalyse.GetDeclarationsNotInRegion<PropertyDeclaration>(classDeclarations[index], textView.GetWholeText());
        //        BaseRegion<PropertyDeclaration>(textView, classDeclarations[index], propertyDeclarations, name);
        //    }
        //}

        //public void QuickRegionConstructor(VSTextView textView, string name)
        //{
        //    //对于一个文件的整理，应该以class为组来进行
        //    List<TypeDeclaration> classDeclarations = NRefactoryAnalyse.GetDeclarations<TypeDeclaration>(textView.GetWholeText());
        //    //同样这里处理的时候采用从下至上的方法，从而避免影响后面的代码
        //    for (int index = classDeclarations.Count - 1; index >= 0; index--)
        //    {
        //        if (classDeclarations[index].Type == ClassType.Enum)
        //        {
        //            //不处理enum类型
        //            continue;
        //        }
        //        List<ConstructorDeclaration> methodDeclarations = NRefactoryAnalyse.GetDeclarationsNotInRegion<ConstructorDeclaration>(classDeclarations[index], textView.GetWholeText());
        //        BaseRegion<ConstructorDeclaration>(textView, classDeclarations[index], methodDeclarations, name);
        //    }
        //}

        //public void QuickRegionpEventMethod(VSTextView textView, string name)
        //{
        //    //对于一个文件的整理，应该以class为组来进行
        //    List<TypeDeclaration> classDeclarations = NRefactoryAnalyse.GetDeclarations<TypeDeclaration>(textView.GetWholeText());
        //    //同样这里处理的时候采用从下至上的方法，从而避免影响后面的代码
        //    for (int index = classDeclarations.Count - 1; index >= 0; index--)
        //    {
        //        if (classDeclarations[index].Type == ClassType.Enum)
        //        {
        //            //不处理enum类型
        //            continue;
        //        }
        //        List<MethodDeclaration> methodDeclarations = NRefactoryAnalyse.GetEventMethodDeclarationsNotInRegion(classDeclarations[index], textView.GetWholeText());
        //        BaseRegion<MethodDeclaration>(textView, classDeclarations[index], methodDeclarations, name);
        //    }
        //}

        //public void QuickRegionpNonEventMethod(VSTextView textView, string name)
        //{
        //    //对于一个文件的整理，应该以class为组来进行
        //    List<TypeDeclaration> classDeclarations = NRefactoryAnalyse.GetDeclarations<TypeDeclaration>(textView.GetWholeText());
        //    //同样这里处理的时候采用从下至上的方法，从而避免影响后面的代码
        //    for (int index = classDeclarations.Count - 1; index >= 0; index--)
        //    {
        //        if (classDeclarations[index].Type == ClassType.Enum)
        //        {
        //            //不处理enum类型
        //            continue;
        //        }
        //        List<MethodDeclaration> methodDeclarations = NRefactoryAnalyse.GetNonEventMethodDeclarationsNotInRegion(classDeclarations[index], textView.GetWholeText());
        //        BaseRegion<MethodDeclaration>(textView, classDeclarations[index], methodDeclarations, name);
        //    }

        //}



        //private void CleanEmptyRegion(VSTextView textView)
        //{
        //    List<TextSpan> emptyRegions = NRefactoryAnalyse.GetEmptyRegions(textView);
        //    for (int i = emptyRegions.Count - 1; i >= 0; i--)
        //    {
        //        TextSpan emptyRegion = emptyRegions[i];
        //        textView.DeleteText(emptyRegion.iStartLine, 0, emptyRegion.iEndLine + 1, 0);  //删除空行的时候，不全部删除。为了美观，多留出一行
        //    }
        //}

        //private Dictionary<T, TextSpan> GetDeclarationAboveRegion<T>(string content, TypeDeclaration typedeclaration) where T : AttributedNode
        //{
        //    Dictionary<T, TextSpan> resDictionary = new Dictionary<T, TextSpan>();
        //    List<T> propertyDeclarations = NRefactoryAnalyse.GetDeclarations<T>(typedeclaration);
        //    foreach (T t in propertyDeclarations)
        //    {
        //        //搜索该位置上面的注释部分：
        //        TextSpan bodySpan = NRefactoryAnalyse.GetComment<T>(content, typedeclaration, t);
        //        bodySpan.iEndLine = t.EndLocation.Line - 1;
        //        //剩下的就是确定对象开始的line，需要同时考虑comment和attribute的情况
        //        string text;

        //        //搜索该属性上面是否有attribute
        //        if (t.Attributes.Count > 0)
        //        {
        //            //存在attribute
        //            //找到attribute中的第一个的位置
        //            int firstAttrStartLine = t.Attributes[0].StartLocation.Line - 1;

        //            //同时又搜索到了注释
        //            if (bodySpan.iStartLine != -1)
        //            {
        //                //attribute在注释之上
        //                if (firstAttrStartLine < bodySpan.iStartLine)
        //                {
        //                    bodySpan.iStartLine = firstAttrStartLine;
        //                }
        //            }
        //            else
        //            {
        //                //只有attribute
        //                bodySpan.iStartLine = firstAttrStartLine;
        //            }
        //        }
        //        else if (bodySpan.iStartLine == -1)
        //        {
        //            //既没有attribute，也米有注释
        //            bodySpan.iStartLine = t.StartLocation.Line - 1;
        //        }

        //        resDictionary.Add(t, bodySpan);
        //    }

        //    return resDictionary;
        //}

        ///// <summary>
        ///// 提供基础的快速region功能，目的是将typeDeclaration类型中的filterList集合进行region
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="textView"></param>
        ///// <param name="typeDeclaration"></param>
        ///// <param name="filterList"></param>
        //private void BaseRegion<T>(VSTextView textView, TypeDeclaration typeDeclaration, List<T> filterList, string regionName) where T : AttributedNode
        //{
        //    Dictionary<T, TextSpan> aboveRegion = GetDeclarationAboveRegion<T>(textView.GetWholeText(), typeDeclaration);
        //    StringBuilder totalField = new StringBuilder();
        //    foreach (T t in filterList)
        //    {
        //        if (aboveRegion.ContainsKey(t))
        //        {
        //            int endLocation = t.EndLocation.Line;
        //            if (t is PropertyDeclaration)
        //            {
        //                endLocation = (t as PropertyDeclaration).BodyEnd.Line;
        //            }
        //            else if (t is MethodDeclaration)
        //            {
        //                endLocation = (t as MethodDeclaration).Body.EndLocation.Line;
        //                if (endLocation == 0)
        //                {
        //                    //方法可能只有声明没有方法体的情况（DLLImport那种方法），此情况下结尾为方法名的结尾
        //                    endLocation = t.EndLocation.Line;
        //                }
        //            }
        //            else if (t is ConstructorDeclaration)
        //            {
        //                endLocation = (t as ConstructorDeclaration).Body.EndLocation.Line;
        //                if (endLocation == 0)
        //                {
        //                    //方法可能只有声明没有方法体的情况（DLLImport那种方法），此情况下结尾为方法名的结尾
        //                    endLocation = t.EndLocation.Line;
        //                }
        //            }
        //            string text = textView.GetText(aboveRegion[t].iStartLine, 0, endLocation, 0);
        //            totalField.Append("\r\n" + text);
        //        }
        //    }


        //    //开始删除原来的节点
        //    int filedCount = filterList.Count;
        //    for (int i = filedCount - 1; i >= 0; i--)
        //    {
        //        //这里需要从后往前删除，因为如果从前面往后删除的时候，删除前面一个会影响其后面的所有其他位置
        //        T t = filterList[i];
        //        int endLocation = t.EndLocation.Line;
        //        if (t is PropertyDeclaration)
        //        {
        //            endLocation = (t as PropertyDeclaration).BodyEnd.Line;
        //        }
        //        else if (t is MethodDeclaration)
        //        {
        //            endLocation = (t as MethodDeclaration).Body.EndLocation.Line;
        //            if (endLocation == 0)
        //            {
        //                //方法可能只有声明没有方法体的情况（DLLImport那种方法），此情况下结尾为方法名的结尾
        //                endLocation = t.EndLocation.Line;
        //            }
        //        }
        //        else if (t is ConstructorDeclaration)
        //        {
        //            endLocation = (t as ConstructorDeclaration).Body.EndLocation.Line;
        //            if (endLocation == 0)
        //            {
        //                //方法可能只有声明没有方法体的情况（DLLImport那种方法），此情况下结尾为方法名的结尾
        //                endLocation = t.EndLocation.Line;
        //            }
        //        }
        //        textView.DeleteText(aboveRegion[t].iStartLine, 0, endLocation, 0);
        //    }

        //    //插入新的region包围的内容
        //    //首先检查当前是否已经存在给定的regionName，如果存在则不用新建
        //    if (filedCount > 0)
        //    {
        //        int line;
        //        if (!CheckExistRegionName(textView, typeDeclaration, regionName, out line))
        //        {
        //            //没有已经同名的region
        //            line = typeDeclaration.BodyStartLocation.Line + 1;
        //            totalField.Insert(0, "\t\t#region " + regionName + "\r\n");
        //            totalField.Append("\t\n\t\t#endregion\r\n\r\n");
        //        }
        //        textView.InsertText(totalField.ToString(), line, 0);
        //    }
        //}

        ///// <summary>
        ///// 检查当前
        ///// </summary>
        ///// <param name="regionName"></param>
        ///// <param name="line"></param>
        ///// <returns></returns>
        //private bool CheckExistRegionName(VSTextView textView, TypeDeclaration typeDeclaration, string regionName, out int line)
        //{
        //    List<TextSpan> allRegions = NRefactoryAnalyse.GetAllRegions(textView.GetWholeText(), typeDeclaration);
        //    line = 0;
        //    foreach (TextSpan allRegion in allRegions)
        //    {
        //        string name =
        //            textView.GetText(allRegion.iStartLine, 0, allRegion.iStartLine + 1, 0).Replace("#region", "").Trim();
        //        if (name == regionName)
        //        {
        //            line = allRegion.iEndLine - 1;  //实际的返回位置应该是#endregion的上面一行
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        #endregion
    }
}