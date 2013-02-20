using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Parser;

using Microsoft.VisualStudio.TextManager.Interop;
using easyvsx.VSObject;

using System.Windows.Forms;

namespace EasyCodeGenerate.Core
{
    /// <summary>
    /// 使用NRefactory对类进行分析
    /// </summary>
    public class NRefactoryAnalyse
    {
        #region - 方法 -

        /// <summary>
        /// 获得所有事件方法的信息
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static List<MethodDeclaration> GetEventMethodDeclarations(List<INode> nodes)
        {
            List<MethodDeclaration> eventMethodDecllarations = new List<MethodDeclaration>();
            List<MethodDeclaration> allMethodDecllarations = new List<MethodDeclaration>();
            GetDeclarations(nodes, allMethodDecllarations);

            foreach (MethodDeclaration methodDeclaration in allMethodDecllarations)
            {
                foreach (ParameterDeclarationExpression parameter in methodDeclaration.Parameters)
                {
                    if (IsEventByParameter(parameter.TypeReference.Type))
                    {
                        eventMethodDecllarations.Add(methodDeclaration);
                    }
                }
            }

            return eventMethodDecllarations;
        }

        /// <summary>
        /// 获得所有事件方法的信息 ，且这些方法不在已有的region中
        /// </summary>
        public static List<MethodDeclaration> GetEventMethodDeclarationsNotInRegion(TypeDeclaration typeDeclaration, string content)
        {
            List<MethodDeclaration> eventMethodDecllarations = new List<MethodDeclaration>();
            List<MethodDeclaration> allMethodDecllarations = GetDeclarationsNotInRegion<MethodDeclaration>(typeDeclaration, content);

            foreach (MethodDeclaration methodDeclaration in allMethodDecllarations)
            {
                foreach (ParameterDeclarationExpression parameter in methodDeclaration.Parameters)
                {
                    if (IsEventByParameter(parameter.TypeReference.Type))
                    {
                        eventMethodDecllarations.Add(methodDeclaration);
                    }
                }
            }

            return eventMethodDecllarations;
        }

        #region 获得所有非事件方法的信息
        /// <summary>
        /// 获得所有非事件方法的信息
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        #endregion
        public static List<MethodDeclaration> GetNonEventMethodDeclarations(List<INode> nodes)
        {
            List<MethodDeclaration> nonEventMethodDecllarations = new List<MethodDeclaration>();
            List<MethodDeclaration> allMethodDecllarations = new List<MethodDeclaration>();
            GetDeclarations(nodes, allMethodDecllarations);

            foreach (MethodDeclaration methodDeclaration in allMethodDecllarations)
            {
                bool isEvent = false;
                foreach (ParameterDeclarationExpression parameter in methodDeclaration.Parameters)
                {
                    if (IsEventByParameter(parameter.TypeReference.Type))
                    {
                        isEvent = true;
                        break;
                    }
                }

                if (!isEvent)
                {
                    nonEventMethodDecllarations.Add(methodDeclaration);
                }
            }

            return nonEventMethodDecllarations;
        }

        /// <summary>
        /// 获得所有非事件方法的信息，且这些方法不在已有的region中
        /// </summary>
        /// <returns></returns>
        public static List<MethodDeclaration> GetNonEventMethodDeclarationsNotInRegion(TypeDeclaration typeDeclaration, string content)
        {
            List<MethodDeclaration> nonEventMethodDecllarations = new List<MethodDeclaration>();
            List<MethodDeclaration> allMethodDecllarations = GetDeclarationsNotInRegion<MethodDeclaration>(typeDeclaration, content);
            
            foreach (MethodDeclaration methodDeclaration in allMethodDecllarations)
            {
                bool isEvent = false;
                foreach (ParameterDeclarationExpression parameter in methodDeclaration.Parameters)
                {
                    if (IsEventByParameter(parameter.TypeReference.Type))
                    {
                        isEvent = true;
                        break;
                    }
                }

                if (!isEvent)
                {
                    nonEventMethodDecllarations.Add(methodDeclaration);
                }
            }

            return nonEventMethodDecllarations;
        }

        /// <summary>
        /// 根据参数类型判断方法是不是事件
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsEventByParameter(string typeName)
        {
            if (typeName.ToLower() == typeof(EventArgs).Name.ToLower())
            {
                return true;
            }
            if (typeName.ToLower().EndsWith("eventargs"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 找到某个type下面指定类型的注释
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">文档内容，需要用其获得所有的comment</param>
        /// <param name="type">要检索的类，接口或者枚举</param>
        /// <param name="t">要检索的类型</param>
        /// <returns></returns>
        public static TextSpan GetComment<T>(string content, TypeDeclaration type, T t) where T : AttributedNode
        {
            TextSpan textSpan = new TextSpan();
            textSpan.iStartLine = textSpan.iEndLine - 1;
            List<Comment> commentsInType = GetAllComments(content, type);
            List<Comment> hitComments = new List<Comment>();

            //找到紧邻t的上面一个类型的位置
            int index = type.Children.FindIndex(o => o == t);
            if (index == -1)  //要搜索的类型不再该type里面
            {
                return textSpan;
            }

            if (index == 0)
            {
                //搜索类下面的第一个类型
                hitComments = commentsInType.FindAll(o => o.StartPosition.Line > type.StartLocation.Line & o.StartPosition.Line < t.StartLocation.Line);
            }
            else
            {
                INode node = type.Children[index - 1];
                //找到位于t和node之间的注释
                if (node is MethodDeclaration)
                {
                    hitComments = commentsInType.FindAll(o => o.StartPosition.Line > (node as MethodDeclaration).Body.EndLocation.Line & o.StartPosition.Line < t.StartLocation.Line);
                }
                else if (node is ConstructorDeclaration)
                {
                    hitComments = commentsInType.FindAll(o => o.StartPosition.Line > (node as ConstructorDeclaration).Body.EndLocation.Line & o.StartPosition.Line < t.StartLocation.Line);
                }
                else if (node is PropertyDeclaration)
                {
                    hitComments = commentsInType.FindAll(o => o.StartPosition.Line > (node as PropertyDeclaration).BodyEnd.Line & o.StartPosition.Line < t.StartLocation.Line);
                }
                else
                {
                    hitComments = commentsInType.FindAll(o => o.StartPosition.Line > node.EndLocation.Line & o.StartPosition.Line < t.StartLocation.Line);
                }
            }

            if (hitComments.Count > 1)
            {
                textSpan.iStartLine = hitComments[0].StartPosition.Line - 1;
                textSpan.iEndLine = hitComments[hitComments.Count - 1].StartPosition.Line - 1;
            }
            else if (hitComments.Count == 1)
            {
                textSpan.iStartLine = textSpan.iEndLine = hitComments[0].StartPosition.Line - 1;
            }

            return textSpan;
        }

        /// <summary>
        ///  搜索指定行（不包括该行）上面是否有对该对象的注释，如果有则返回注释的TextSpan
        /// </summary>
        /// <param name="content"></param>
        /// <param name="line">从0开始</param>
        /// <returns></returns>
        public static TextSpan GetCommentsByLine( TypeDeclaration type,string content, int line)
        {
            TextSpan textSpan = new TextSpan();
            textSpan.iStartIndex = textSpan.iStartLine = -1;

            List<Comment> allComments = GetAllComments(content,type);
            for (int i = 0; i < allComments.Count; i++)
            {
                Comment comment = allComments[i];
                if (comment.StartPosition.Line - 1 /*因为这里的Line是从1开始的，所有要-1再算*/ == line - 1)  //查看该对象的上一行是否有comment存在
                {
                    textSpan.iEndLine = line - 1;

                    bool hitComment = true;
                    int startLoopIndex = line - 2;  //从检索到的comment的上一行开始检索
                    int commentIndex = i - 1;
                    while (hitComment && commentIndex >= 0)
                    {
                        if (allComments[commentIndex].StartPosition.Line - 1 != startLoopIndex)
                        {
                            hitComment = false;
                        }
                        textSpan.iStartLine = startLoopIndex;
                        startLoopIndex--;
                        commentIndex--;
                    }

                    break;
                }
            }

            return textSpan;
        }

        /// <summary>
        /// 获得type中的所有comment对象，注意这里的comment对象都是以行为单位的
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<Comment> GetAllComments(string content, TypeDeclaration type)
        {
            List<Comment> comments = new List<Comment>();
            using (IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(content)))
            {
                parser.ParseMethodBodies = false;
                parser.Parse();

                List<ISpecial> currentSpecials = parser.Lexer.SpecialTracker.CurrentSpecials;
                foreach (ISpecial currentSpecial in currentSpecials)
                {
                    if (currentSpecial is Comment)
                    {
                        //确保找到的comment在给定的type范围之内
                        if (currentSpecial.StartPosition.Line > type.StartLocation.Line && currentSpecial.EndPosition.Line < type.EndLocation.Line)
                        {
                            comments.Add(currentSpecial as Comment);
                        }
                    }
                }
            }
            return comments;
        }

        /// <summary>
        /// 获得所有的regions区域信息，textspan都是基于(0,0)开始的
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<TextSpan> GetAllRegions(string content)
        {
            List<TextSpan> allRegions = new List<TextSpan>();
            using (IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(content)))
            {
                parser.ParseMethodBodies = false;
                parser.Parse();

                List<ISpecial> currentSpecials = parser.Lexer.SpecialTracker.CurrentSpecials;
                TextSpan textSpan = new TextSpan();
                //使用栈来寻找可能嵌套的region
                Stack<int> stack = new Stack<int>();
                foreach (ISpecial currentSpecial in currentSpecials)
                {
                    if (currentSpecial is PreprocessingDirective)
                    {
                        PreprocessingDirective region = currentSpecial as PreprocessingDirective;
                        if (region.Cmd == "#region")
                        {
                            stack.Push(region.StartPosition.Line - 1);
                            //textSpan.iStartLine = region.StartPosition.Line - 1;
                        }
                        if (region.Cmd == "#endregion")
                        {
                            textSpan.iStartLine = stack.Pop();
                            textSpan.iEndLine = region.StartPosition.Line - 1;
                            allRegions.Add(textSpan);
                        }
                    }
                }
            }
            return allRegions;
        }

        /// <summary>
        /// 获得typeDeclaration下面的所有的regions区域信息，textspan都是基于(0,0)开始的
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<TextSpan> GetAllRegions(string content, TypeDeclaration typeDeclaration)
        {
            List<TextSpan> allRegions = new List<TextSpan>();
            List<TextSpan> list = GetAllRegions(content);
            foreach (TextSpan textSpan in list)
            {
                if (textSpan.iStartLine > typeDeclaration.StartLocation.Line && textSpan.iEndLine < typeDeclaration.EndLocation.Line)
                {
                    allRegions.Add(textSpan);
                }
            }
            return allRegions;
        }

        public static List<TextSpan> GetEmptyRegions(VSTextView textView)
        {
            List<TextSpan> allRegions = new List<TextSpan>();
            List<TextSpan> textSpans = GetAllRegions(textView.GetWholeText());
            for (int i = 0; i < textSpans.Count; i++)
            {
                TextSpan region = textSpans[i];
                bool hasContent = false;
                //检测start和end之间是否都是空行
                for (int j = region.iStartLine + 1; j <= region.iEndLine - 1; j++)
                {
                    string s = textView.GetOneLineText(j);
                    if (!string.IsNullOrEmpty(s.Trim()))
                    {
                        //region之间有内容
                        hasContent = true;
                        break;
                    }
                }

                if (!hasContent)
                {
                    allRegions.Add(region);
                }

            }
            return allRegions;
        }

        public static List<int> GetBlankLiens(VSTextView textView)
        {
            List<int> lines = new List<int>();
            using (IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(textView.GetWholeText())))
            {
                parser.ParseMethodBodies = false;
                parser.Parse();

                List<ISpecial> currentSpecials = parser.Lexer.SpecialTracker.CurrentSpecials;
                TextSpan textSpan = new TextSpan();
                foreach (ISpecial currentSpecial in currentSpecials)
                {
                    if (currentSpecial is BlankLine)
                    {
                        BlankLine region = currentSpecial as BlankLine;
                        lines.Add(region.StartPosition.Line - 1);
                    }
                }
            }
            return lines;
        }

        /// <summary>
        /// 获得文档中所有的空白区域块
        /// </summary>
        /// <param name="textView"></param>
        /// <param name="atLeastEmptyLines">连续几个空行才算空白区域</param>
        public static List<TextSpan> GetBlankLines(VSTextView textView, int atLeastEmptyLines)
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

        /// <summary>
        /// 得到文档中的using区域信息
        /// </summary>
        /// <param name="textView"></param>
        /// <returns></returns>
        public static List<TextSpan> GetUsingDeclaration(VSTextView textView)
        {
            //List<int> lines = new List<int>();
            ////using (IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(textView.GetWholeText())))
            ////{
            ////    parser.ParseMethodBodies = false;
            ////    parser.Parse();

            ////    List<ISpecial> currentSpecials = parser.Lexer.SpecialTracker.CurrentSpecials;
            ////    TextSpan textSpan = new TextSpan();
            ////    foreach (ISpecial currentSpecial in currentSpecials)
            ////    {
            ////        if (currentSpecial is BlankLine)
            ////        {
            ////            BlankLine region = currentSpecial as BlankLine;
            ////            lines.Add(region.StartPosition.Line - 1);
            ////        }
            ////    }
            ////}
            //return lines;
            return null;
        }

        #endregion

        #region - GetDeclarations -

        /// <summary>
        /// 获得一个AttributedNode节点下面（NRefacotry下面的类）所有的指定类型节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">这里的nodes，一般为class类型的children</param>
        /// <param name="declarations"></param>
        private static void GetDeclarations<T>(List<INode> nodes, List<T> declarations) where T : AttributedNode
        {
            foreach (INode node in nodes)
            {
                if (node.GetType() == typeof(T))
                {
                    declarations.Add(node as T);
                }
                GetDeclarations(node.Children, declarations);
            }
        }

        /// <summary>
        /// 获得一个AttributedNode节点下面（NRefacotry下面的类）所有的指定类型节点，且这些节点不在已有的region中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">这里的nodes，一般为class类型的children</param>
        /// <param name="declarations"></param>
        private static void GetDeclarationsNotInRegion<T>(List<INode> nodes, List<T> declarations, List<TextSpan> regionSpans) where T : AttributedNode
        {
            foreach (INode node in nodes)
            {
                TextSpan textSpan = regionSpans.Find(o => o.iStartLine <= node.StartLocation.Line && o.iEndLine >= node.EndLocation.Line);
                if (node.GetType() == typeof(T) && (textSpan.iStartLine == 0 && textSpan.iEndLine == 0))  //没有找到其外围有region包围
                {
                    declarations.Add(node as T);
                }

                GetDeclarationsNotInRegion(node.Children, declarations, regionSpans);
            }
        }

        /// <summary>
        /// 获得字符串中的所有的指定类型节点（不管有几个类）
        /// </summary>
        public static List<T> GetDeclarations<T>(string textContent) where T : AttributedNode
        {
            List<T> list = new List<T>();
            using (IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(textContent)))
            {
                parser.ParseMethodBodies = false;
                parser.Parse();
                GetDeclarations<T>(parser.CompilationUnit.Children, list);
            }

            return list;
        }

        /// <summary>
        /// 获得一个TypeDeclaration节点下面所有的指定类型节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> GetDeclarations<T>(TypeDeclaration type) where T : AttributedNode
        {
            List<T> list = new List<T>();
            GetDeclarations(type.Children, list);
            return list;
        }

        /// <summary>
        /// 获得一个TypeDeclaration节点下面所有的指定类型节点且这些节点不在已有的region中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<T> GetDeclarationsNotInRegion<T>(TypeDeclaration type, string content) where T : AttributedNode
        {
            List<T> list = new List<T>();
            GetDeclarationsNotInRegion(type.Children, list, GetAllRegions(content));
            return list;
        }

        /// <summary>
        /// 获得给定文件中的TypeDeclaration类型的对象（一般为类对象，接口对象，枚举对象等等）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<TypeDeclaration> GetTypeDeclarations(string content)
        {
            return GetDeclarations<TypeDeclaration>(content);
        }

        #endregion

    }
}
