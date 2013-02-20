using System;
using System.Text;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.TextManager.Interop;
using easyVS.Forms.Setting.UC;
using easyvsx.VSObject;
using System.Collections.Generic;

namespace easyVS.Menu.NonMenuCommand
{
    public class AutoHeader
    {
        #region - Variables -

        //private static string prefix = "/******************************************************************************";

        //private static string suffix = "******************************************************************************/";

        private static List<string> newOpenDocNameList = new List<string>();

        #endregion

        #region - Methods -

        public static void DocumentSavedEvent(uint doccookie)
        {
            //VSTextView view = new VSTextView(VSTextView.ActiveTextView);
            //string headerInfo = GetHeaderInfo(view.GetWholeText());
            //if (!string.IsNullOrEmpty(headerInfo))
            //{
            //    //修改文件头信息
            //    //VSDocument.SaveActiveDocument();
            //    string pattern = BaseUCSetting.SettingModel.AutoHeaderModel.AutoHeaderTemplete.
            //                            Replace("[ModifiedTime]", "(?<ModifiedTime>.*?)");

            //    MatchCollection matchCollection = Regex.Matches(headerInfo, pattern, RegexOptions.IgnoreCase);
            //}
        }

        //private static string GetHeaderInfo(string text)
        //{
        //    if (text.StartsWith(prefix))
        //    {
        //        int last = text.IndexOf(suffix);
        //        string info = text.Substring(prefix.Length, last);
        //        return info;
        //    }
        //    return string.Empty;
        //}

        public static void DocumentOpenedEvent(Document document)
        {
            //是新打开的文件
            if (newOpenDocNameList.Contains(document.Name))
            {
                //清除新打开的记录
                newOpenDocNameList.Remove(document.Name);

                //只对.cs结尾的文件添加
                if (document.FullName.ToLower().EndsWith(".cs")
                    || document.FullName.ToLower().EndsWith(".js")
                    || document.FullName.ToLower().EndsWith(".css")
                    )
                {

                    //添加文件头信息
                    StringBuilder templete = new StringBuilder(BaseUCSetting.SettingModel.AutoHeaderModel.AutoHeaderTemplete);
                    if (!string.IsNullOrEmpty(templete.ToString()))
                    {
                        templete = templete.Replace("[CurrentTime]", DateTime.Now.ToString());
                        templete = templete.Replace("[User]", Environment.UserName);
                        //templete.Insert(0, prefix + "\n");
                        //templete.Append("\n " + suffix + "\n");

                        VSDocument.Insert(document, 1, 1, templete.ToString());
                    }
                }
            }

            // VsOutput.ShowGeneralMessage(document.Name + " 打开了");

        }


        public static void projectItemEvents_ItemAdded(ProjectItem ProjectItem)
        {
            if (BaseUCSetting.SettingModel.AutoHeaderModel.OpenAutoHeader)
            {
                //ProjectItem.Document 为空，不可用
                //记录一下这个文件是新打开的，在紧接着的document_open事件中处理
                newOpenDocNameList.Add(ProjectItem.Name);

                //  VsOutput.ShowGeneralMessage(ProjectItem.Name + "  添加了");
            }
        }

        #endregion

    }
}
