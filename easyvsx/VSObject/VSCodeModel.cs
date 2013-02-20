using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using System.Collections;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Reflection;
using System.Windows.Forms;

namespace easyvsx.VSObject
{
    public class VSCodeModel : VSBase
    {
        public static Dictionary<string, List<CodeElement>> dict = new Dictionary<string, List<CodeElement>>();
        public static Dictionary<string, List<RegionElement>> regionDict = new Dictionary<string, List<RegionElement>>();

        #region - 属性 -

        public FileCodeModel2 CurrentFileCodeModel
        {
            get
            {
                if (VSDocument.ActiveDocument == null)
                {
                    return null;
                }

                FileCodeModel model = VSDocument.ActiveDocument.ProjectItem.FileCodeModel;
                return model == null ? null : model as FileCodeModel2;
            }
        }

        #endregion

        #region - 方法 -

        /// <summary>
        /// 得到一个CodeElements下所有的元素
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="allElements"></param>
        public void GetAllElements(CodeElements elements, List<CodeElement> allElements)
        {
            foreach (CodeElement element in elements)
            {
                try
                {
                    allElements.Add(element);
                    if (element.Children != null)
                    {
                        GetAllElements(element.Children, allElements);
                    }
                }
                catch
                {
                }
            }


        }

        /// <summary>
        /// 获得所有的方法（包括事件和普通方法）
        /// </summary>
        /// <param name="classElement"></param>
        /// <returns></returns>
        public List<CodeElement> GetMethodsInClass(CodeElement classElement)
        {
            return GetSpecifiedKindInClass(classElement, vsCMElement.vsCMElementFunction);
        }

        /// <summary>
        /// 获得不是事件的方法
        /// </summary>
        /// <param name="classElement"></param>
        /// <returns></returns>
        public List<CodeElement> GetNoneEventMethodsInClass(CodeElement classElement)
        {
            //不是事件和构造函数的方法
            return GetMethodsInClass(classElement)
                .Except(GetEventsInClass(classElement))
                .Except(GetConstructorInClass(classElement)).ToList();
        }

        /// <summary>
        /// 获得事件方法
        /// </summary>
        /// <param name="classElement"></param>
        /// <returns></returns>
        public List<CodeElement> GetEventsInClass(CodeElement classElement)
        {
            List<CodeElement> allMethods = GetMethodsInClass(classElement);
            List<CodeElement> eventMethods = new List<CodeElement>();

            foreach (CodeElement item in allMethods)
            {
                CodeElements parameters = ((CodeFunction)item).Parameters;
                bool isEvent = false;
                foreach (CodeElement para in parameters)
                {
                    string paraType = ((CodeParameter)para).Type.AsString.ToLower();
                    if (paraType.EndsWith("eventargs"))
                    {
                        isEvent = true;
                        break;
                    }
                }
                if (isEvent)
                {
                    eventMethods.Add(item);
                }
            }

            return eventMethods;
        }

        /// <summary>
        /// 获得公开方法（不是private）
        /// </summary>
        /// <param name="classElement"></param>
        /// <returns></returns>
        public List<CodeElement> GetPublicMethodsInClass(CodeElement classElement)
        {
            List<CodeElement> allMethods = GetMethodsInClass(classElement);
            List<CodeElement> publicMethods = new List<CodeElement>();

            foreach (CodeElement item in allMethods)
            {
                vsCMAccess access = ((CodeFunction)item).Access;
                if (access != vsCMAccess.vsCMAccessPrivate)
                {
                    publicMethods.Add(item);
                }
            }

            return publicMethods;
        }

        /// <summary>
        /// 获得私有方法（private）
        /// </summary>
        /// <param name="classElement"></param>
        /// <returns></returns>
        public List<CodeElement> GetPrivateMethodsInClass(CodeElement classElement)
        {
            List<CodeElement> allMethods = GetMethodsInClass(classElement);
            List<CodeElement> privateMethods = new List<CodeElement>();

            foreach (CodeElement item in allMethods)
            {
                vsCMAccess access = ((CodeFunction)item).Access;
                if (access == vsCMAccess.vsCMAccessPrivate)
                {
                    privateMethods.Add(item);
                }
            }

            return privateMethods;
        }

        public List<CodeElement> GetVariablesInClass(CodeElement classElement)
        {
            return GetSpecifiedKindInClass(classElement, vsCMElement.vsCMElementVariable);
        }

        public List<CodeElement> GetPropertyInClass(CodeElement classElement)
        {
            return GetSpecifiedKindInClass(classElement, vsCMElement.vsCMElementProperty);
        }

        public List<CodeElement> GetClassInCurrentFile()
        {
            return GetSpecifiedKindInCurrentFile(vsCMElement.vsCMElementClass);
        }

        public List<CodeElement> GetStructInCurrentFile()
        {
            return GetSpecifiedKindInCurrentFile(vsCMElement.vsCMElementStruct);
        }

        public List<CodeElement> GetConstructorInClass(CodeElement classElement)
        {
            List<CodeElement> lists = new List<CodeElement>();
            if (classElement == null)
            {
                return lists;
            }

            string className = classElement.Name;
            foreach (CodeElement item in GetMethodsInClass(classElement))
            {
                if ((item as CodeFunction).Name == className)
                {
                    lists.Add(item);
                }
            }

            return lists;
        }

        #region  not region


        public List<CodeElement> GetNotRegionMethodsInClass(CodeElement classElement)
        {
            List<CodeElement> methodsElements = GetMethodsInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(methodsElements, regions);
        }

        public List<CodeElement> GetNotRegionEventInClass(CodeElement classElement)
        {
            List<CodeElement> methodsElements = GetEventsInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(methodsElements, regions);
        }

        public List<CodeElement> GetNotRegionNoneEventMethodInClass(CodeElement classElement)
        {
            List<CodeElement> methodsElements = GetNoneEventMethodsInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(methodsElements, regions);
        }

        public List<CodeElement> GetNotRegionPublicMethodsInClass(CodeElement classElement)
        {
            List<CodeElement> methodsElements = GetPublicMethodsInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(methodsElements, regions);
        }

        public List<CodeElement> GetNotRegionPrivateMethodsInClass(CodeElement classElement)
        {
            List<CodeElement> methodsElements = GetPrivateMethodsInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(methodsElements, regions);
        }

        public List<CodeElement> GetNotRegionPropertyInClass(CodeElement classElement)
        {
            List<CodeElement> elements = GetPropertyInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(elements, regions);
        }

        public List<CodeElement> GetNotRegionVariablesInClass(CodeElement classElement)
        {
            List<CodeElement> elements = GetVariablesInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(elements, regions);
        }

        public List<CodeElement> GetNotRegionConstructorInClass(CodeElement classElement)
        {
            List<CodeElement> elements = GetConstructorInClass(classElement);
            List<RegionElement> regions = GetRegionsInClass(classElement);

            return GetNotRegionElementInClass(elements, regions);
        }

        private List<CodeElement> GetNotRegionElementInClass(List<CodeElement> elements, List<RegionElement> regions)
        {
            if (regions.Count == 0)
            {
                //类中没有region则说明所有的方法都不在region中
                return elements;
            }

            List<CodeElement> methodsNotRegion = new List<CodeElement>();

            foreach (CodeElement element in elements)
            {
                bool isInRegion = false;
                foreach (RegionElement region in regions)
                {
                    if (element.StartPoint.Line > region.StartLine && element.EndPoint.Line < region.EndLine)
                    {
                        isInRegion = true;
                        break;
                    }
                }
                if (!isInRegion)
                {
                    methodsNotRegion.Add(element);
                }
            }

            return methodsNotRegion;
        }


        #endregion

        /// <summary>
        /// 得到同级别下指定元素前面的一个元素，如果指定元素是当前级别的第一个则返回自己
        /// 如果返回null很有可能当前codeelement不再当前文档中
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public CodeElement GetPrevElementInCurrentFile(CodeElement e)
        {
            if (CurrentFileCodeModel == null)
            {
                return null;
            }

            return GetPrevElementInCurrenFile(CurrentFileCodeModel.CodeElements, e);
        }

        public CodeElement GetParentElementInCurrentFile(CodeElement e)
        {
            if (CurrentFileCodeModel == null)
            {
                return null;
            }
            CodeElement result = null;
            GetParentElementInCurrentFile(CurrentFileCodeModel.CodeElements, e, ref result);
            return result;
        }

        /// <summary>
        /// 寻找所有的region，包含层级关系
        /// </summary>
        /// <param name="classElement"></param>
        /// <returns></returns>
        public List<RegionElement> GetRegionsInClass(CodeElement classElement)
        {
            ////缓存操作
            //string key = classElement.GetHashCode() + "_GetRegionsInClass";
            //if (!IsCurrenFileDirty() && regionDict.ContainsKey(key))
            //{
            //    return regionDict[key];
            //}

            List<RegionElement> allRegions = GetAllRegionsInClass(classElement);
            if (allRegions.Count == 0)
            {
                return new List<RegionElement>();
            }

            int maxLevel = allRegions.Max(o => o.Level);
            List<RegionElement> regions = allRegions.Where(o => o.Level == 1).ToList();
            List<RegionElement> currentLevel = regions;
            int currentIndex = 1;
            while (currentIndex <= maxLevel)
            {
                List<RegionElement> nextLevelElements = new List<RegionElement>();
                foreach (RegionElement item in currentLevel)
                {
                    List<RegionElement> nextLevelElement = allRegions.Where(o => o.Level == currentIndex + 1
                        && o.StartLine > item.StartLine
                        && o.EndLine < item.EndLine).ToList();
                    item.Children = nextLevelElement;
                    nextLevelElements.AddRange(item.Children);
                }
                currentIndex++;
                currentLevel = nextLevelElements; //regions.Where(o => o.Level == currentIndex).ToList();
            }


            ////重复添加会报错
            //if (regionDict.ContainsKey(key))
            //{
            //    regionDict.Remove(key);
            //}
            //regionDict.Add(key, regions);
            return regions;
        }

        /// <summary>
        /// 寻找所有的region，不包含层级关系
        /// </summary>
        /// <param name="classElement"></param>
        /// <returns></returns>
        public List<RegionElement> GetAllRegionsInClass(CodeElement classElement)
        {
            List<RegionElement> allRegions = new List<RegionElement>();
            int currentLine = classElement.StartPoint.Line - 1;
            VSTextView textView = new VSTextView(VSTextView.ActiveTextView);

            //使用栈来寻找可能嵌套的region
            Stack<int> stack = new Stack<int>();
            while (currentLine <= classElement.EndPoint.Line)
            {
                string region = textView.GetOneLineText(currentLine);
                if (region != null)
                {
                    if (region.TrimStart().StartsWith("#region"))
                    {
                        stack.Push(currentLine);
                    }
                    if (region.TrimStart().StartsWith("#endregion"))
                    {
                        RegionElement textSpan = new RegionElement();
                        if (stack.Count != 0)
                        {

                            textSpan.StartLine = stack.Pop();
                            textSpan.EndLine = currentLine;
                            textSpan.Level = stack.Count + 1;
                            textSpan.RegionName = textView.GetOneLineText(textSpan.StartLine).Trim()
                                .Replace("#region ", "").Replace("\r\n", "");
                            allRegions.Add(textSpan);
                        }
                    }
                }
                currentLine++;
            }

            return allRegions;
        }

        private CodeElement GetParentElementInCurrentFile(CodeElements waitCheckElements, CodeElement e, ref CodeElement result)
        {
            if (waitCheckElements.Count == 0)
            {
                return null;
            }

            IEnumerator enumerator = waitCheckElements.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CodeElement current = enumerator.Current as CodeElement;
                if (current == e)
                {
                    return current;
                }

                //递归找，如果找到则直接返回
                CodeElement c = GetParentElementInCurrentFile(current.Children, e, ref result);
                if (c != null)
                {
                    //这里不能纯粹返回current父元素
                    //在第一次找个匹配元素的时候就要继续下其父元素，否则随着地递归的返回调用这个父元素
                    //会被往上层变
                    if (result == null)
                    {
                        //一旦result被复制就说明已经找到父元素，不会再重复赋值
                        result = current;
                    }
                    return current;
                }
            }

            return null;
        }

        /// <summary>
        /// 得到同级别下指定元素前面的一个元素，如果指定元素是当前级别的第一个则返回自己
        /// 如果返回null很有可能当前codeelement不再当前文档中
        /// 
        /// 1
        /// --2
        /// --3
        /// ----4
        /// ----5
        /// 6
        /// 7
        /// 例如，我们要找3的前面一个元素为2，2的前一个元素不存在
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private CodeElement GetPrevElementInCurrenFile(CodeElements waitCheckElements, CodeElement element)
        {
            if (waitCheckElements.Count == 0)
            {
                return null;
            }

            IEnumerator enumerator = waitCheckElements.GetEnumerator();
            enumerator.MoveNext();
            //同一级别下的第一个元素
            CodeElement firstElementInLevel = enumerator.Current as CodeElement;
            if (firstElementInLevel == element)
            {
                //如果要找元素是第一个元素，则返回自己
                return firstElementInLevel;
            }
            else
            {
                //递归找第一个元素的子元素
                CodeElement c = GetPrevElementInCurrenFile(firstElementInLevel.Children, element);
                //如果返回值不为空，则说明找到了
                if (c != null)
                {
                    return c;
                }
            }


            CodeElement prevElement = firstElementInLevel;
            while (enumerator.MoveNext())
            {
                CodeElement e = enumerator.Current as CodeElement;
                if (e == element)
                {
                    return prevElement;
                }
                prevElement = e;

                //递归找，如果找到则直接返回
                CodeElement c = GetPrevElementInCurrenFile(e.Children, element);
                if (c != null)
                {
                    return c;
                }
            }

            //没有前一项则返回空，很有可能当前codeelement不再当前文档中
            return null;
        }

        /// <summary>
        /// 获得当前元素下面（包括此元素）所有指定类型的元素
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public List<CodeElement> GetSpecifiedKindUnderElement(CodeElement currentElement, vsCMElement elementType)
        {
            ////缓存操作
            //string key = currentElement.GetHashCode() + "_GetSpecifiedKindUnderElement_" + elementType.ToString();
            //if (!IsCurrenFileDirty() && dict.ContainsKey(key))
            //{
            //    return dict[key];
            //}

            List<CodeElement> reslists = new List<CodeElement>();
            if (CurrentFileCodeModel == null)
            {
                return reslists;
            }

            //如果当前类是要寻找的类型，则先加入队列。然后找其子元素
            if (currentElement.Kind == elementType)
            {
                reslists.Add(currentElement);
            }

            //对于enum类型来说，不递归其子元素。因为其子元素会被认为是varaible变量
            //这个过滤放在这边，可以让筛选enum本身得以成功
            if (currentElement.Kind == vsCMElement.vsCMElementEnum)
            {
                return reslists;
            }

            IEnumerator enumerator = currentElement.Children.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CodeElement e = enumerator.Current as CodeElement;
                if (e.Kind == elementType)
                {
                    reslists.Add(e);
                }

                //递归在元素的所有子元素
                //正常查找到时候可能差到namesapce就停止了，如果要查找class元素在namesapce的子元素下

                if (e.Children.Count > 0)
                {
                    foreach (CodeElement item in e.Children)
                    {
                        reslists.AddRange(GetSpecifiedKindUnderElement(item, elementType));
                    }
                }


            }

            ////重复添加会报错
            //if (dict.ContainsKey(key))
            //{
            //    dict.Remove(key);
            //}
            //dict.Add(key, reslists);

            return reslists;
        }

        /// <summary>
        /// 获得当前文件中所有指定类型的元素
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public List<CodeElement> GetSpecifiedKindInCurrentFile(vsCMElement kind)
        {
            //这里不能应用缓存，因为当前文件会不断变化

            List<CodeElement> resLists = new List<CodeElement>();
            if (CurrentFileCodeModel == null)
            {
                return resLists;
            }

            IEnumerator enumerator = CurrentFileCodeModel.CodeElements.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CodeElement e = enumerator.Current as CodeElement;
                //这里必须要递归寻找，不能单纯的比较当前类型和要查找的类型。
                //因为要查找的类型可能在当前类型之下
                resLists.AddRange(GetSpecifiedKindUnderElement(e, kind));
            }

            return resLists;
        }

        /// <summary>
        /// 获得一个class元素下所有指定的元素
        /// </summary>
        /// <param name="classElement"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public List<CodeElement> GetSpecifiedKindInClass(CodeElement classElement, vsCMElement kind)
        {
            ////缓存操作
            //string key = classElement.GetHashCode() + "_GetSpecifiedKindInClass_" + kind.ToString();
            //if (!IsCurrenFileDirty() && dict.ContainsKey(key))
            //{
            //    return dict[key];
            //}

            List<CodeElement> resLists = new List<CodeElement>();

            IEnumerator iterator = classElement.Children.GetEnumerator();
            while (iterator.MoveNext())
            {
                CodeElement e = iterator.Current as CodeElement;
                resLists.AddRange(GetSpecifiedKindUnderElement(e, kind));
            }

            ////重复添加会报错
            //if (dict.ContainsKey(key))
            //{
            //    dict.Remove(key);
            //}
            //dict.Add(key, resLists);
            return resLists;
        }

        /// <summary>
        /// 当前文件是否被修改
        /// </summary>
        /// <returns></returns>
        public bool IsCurrenFileDirty()
        {
            IEnumerator enumerator = CurrentFileCodeModel.CodeElements.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ProjectItem pi = ((CodeElement)enumerator.Current).ProjectItem;
                return pi.IsDirty;
            }

            return true;
        }

        public  List<int> GetBlankLiens(VSTextView textView)
        {
            List<int> lines = new List<int>();
            int allLines = textView.GetLines();
            for (int i = 0; i < allLines; i++)
            {
                if (string.IsNullOrEmpty(textView.GetOneLineText(i).Trim()))
                {
                    lines.Add(i); 
                }
            }
            return lines;
        }

        /// <summary>
        /// 获得文档中所有的空白区域块
        /// </summary>
        /// <param name="textView"></param>
        /// <param name="atLeastEmptyLines">连续几个空行才算空白区域</param>
        public  List<TextSpan> GetBlankLines(VSTextView textView, int atLeastEmptyLines)
        {
            List<TextSpan> blankLineRegion = new List<TextSpan>();
            List<int> blankLiens = GetBlankLiens(textView);
            for (int i = 0; i < blankLiens.Count; i++)
            {
                TextSpan region = new TextSpan();
                int startBlank = blankLiens[i];
                region.iStartLine = startBlank;

                bool hasSequentialBlank = true;
                while (hasSequentialBlank)
                {
                    if (i == blankLiens.Count - 1)  //循环到了最后一行
                    {
                        region.iEndLine = blankLiens[i];
                        if (region.iEndLine - region.iStartLine >= atLeastEmptyLines - 1)
                        {
                            blankLineRegion.Add(region);
                        }
                        break;
                    }

                    if (blankLiens[i + 1] != blankLiens[i] + 1) //检查下一个空行的行号是否是上一个空行行号+1
                    {
                        hasSequentialBlank = false;
                        region.iEndLine = blankLiens[i];
                        if (region.iEndLine - region.iStartLine >= atLeastEmptyLines - 1)
                        {
                            blankLineRegion.Add(region);
                        }
                        i--; //把下面i++多的减掉
                    }

                    i++;
                }
            }

            return blankLineRegion;
        }

        #endregion
    }

    public class RegionElement
    {
        #region - 属性 -

        public int StartLine { get; set; }

        public int EndLine { get; set; }

        /// <summary>
        /// 当前region在所有region中的层级
        /// </summary>
        public int Level { get; set; }

        public string RegionName { get; set; }

        public List<RegionElement> Children { get; set; }

        #endregion
    }
}