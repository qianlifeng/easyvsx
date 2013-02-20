/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 1:55:49
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using easyvsx;
using easyvsx.Intellisense;

namespace Easy.Filter
{
    /// <summary>
    /// validatetiongroup 属性过滤器
    /// </summary>
    public class ValidatetionFilter : CommandFilter
    {
        #region - 变量 -

        private static IVsTextManager textManager;
        private CompletionIntellisense completion;

        #endregion

        #region - 属性 -

        public IVsTextManager TextManager
        {
            get
            {
                if (textManager == null)
                {
                    Object obj = Package.GetGlobalService(typeof (SVsTextManager));
                    if (obj == null)
                    {
                        return null;
                    }
                    textManager = obj as IVsTextManager;
                }
                return textManager;
            }
        }

        /// <summary>
        /// 获得当前活动的Visual Studio文本编辑器
        /// </summary>
        public IVsTextView ActiveTextView
        {
            get
            {
                IVsTextView activeView = null;
                //只要TextManager不为空，则每次都获取最新的TextView对象
                if (TextManager != null)
                {
                    TextManager.GetActiveView(0, null, out activeView);
                }
                return activeView;
            }
        }

        #endregion

        #region - 构造函数 -

        public ValidatetionFilter()
        {
            var list = new List<CompletionItem>();
            for (int i = 0; i < 5; i++)
            {
                var item = new CompletionItem("item" + i, "item description" + i, "item" + i);
                list.Add(item);
            }
            completion = new CompletionIntellisense(list);
        }

        #endregion

        #region - 事件 -

        public override bool OnBeforeExecCommand(Guid pguidCmdGroup, uint nCmdid, uint nCmdExecopt, IntPtr pVain,
                                                 IntPtr pVaout)
        {
            if (pguidCmdGroup == typeof (VSConstants.VSStd2KCmdID).GUID)
            {
                switch (nCmdid)
                {
                    case (uint) VSConstants.VSStd2KCmdID.TYPECHAR:
                        object obj = Marshal.GetObjectForNativeVariant(pVain);
                        var key = (char) (ushort) obj;
                        if (key == '=')
                        {
                            int currentLineIndex, currentColIndex;
                            ActiveTextView.GetCaretPos(out currentLineIndex, out currentColIndex);
                            string lineStr;
                            ActiveTextView.GetTextStream(currentLineIndex, 0, currentLineIndex, currentColIndex,
                                                         out lineStr);
                            if (lineStr != null && GetPrevWord(lineStr) == "ValidationGroup")
                            {
                                ActiveTextView.UpdateCompletionStatus(completion,
                                                                      (uint) UpdateCompletionFlags.UCS_COMPLETEWORD);
                            }
                        }
                        break;
                }
            }

            return true;
        }

        #endregion

        #region - 方法 -

        /// <summary>
        /// 找到紧接着当前位置前的一个字符串
        /// </summary>
        /// <param name="lineStr"></param>
        /// <returns></returns>
        private string GetPrevWord(string lineStr)
        {
            //找到最后一个空格（不包括空格）到最后之间的字符串
            return lineStr.Substring(lineStr.LastIndexOf(' ') + 1);
        }

        #endregion
    }
}