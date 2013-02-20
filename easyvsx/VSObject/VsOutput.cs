using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;

namespace easyvsx.VSObject
{
    public class VsOutput
    {
        #region - 变量 -

        private static IVsOutputWindow outputWindowObject;

        #endregion

        #region - 属性 -

        /// <summary>
        /// 获得全局的IVsOutputWindow 接口对象
        /// </summary>
        public static IVsOutputWindow OutputWindowObject
        {
            get
            {
                if (outputWindowObject == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsOutputWindow));
                    if (obj == null)
                    {
                        throw new ArgumentNullException("获得SVsOutputWindow失败");
                    }
                    outputWindowObject = obj as IVsOutputWindow;
                }
                return outputWindowObject;
            }
        }

        #endregion


   

        /// <summary>
        /// 在General输出窗体中显示信息（兼容2010 & 2008）
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowGeneralMessage(string msg)
        {
            msg += "\n";

            Guid generalPaneGuid = VSConstants.OutputWindowPaneGuid.GeneralPane_guid;
            IVsOutputWindowPane generalPane;
            OutputWindowObject.GetPane(ref generalPaneGuid, out generalPane);

            //vs2010在这里 generalPane为空
            //因为：In VS 2010, not like VS 2008, general output window pane is not added to the output window by default.
            //If you want to use it, you have to ceate the pane before getting it.
            if (generalPane == null)
            {
                OutputWindowObject.CreatePane(generalPaneGuid, "general", 1, 0);
                OutputWindowObject.GetPane(generalPaneGuid, out generalPane);

            }

            generalPane.OutputString(msg);
            generalPane.Activate();

        }

        /// <summary>
        /// 在Debug输出窗体中显示信息
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowDebugMessage(string msg)
        {
            msg += "\n";

            Guid debugPaneGuid = VSConstants.GUID_OutWindowDebugPane;
            IVsOutputWindowPane debugPane;
            OutputWindowObject.GetPane(ref debugPaneGuid, out debugPane);

            if (debugPane == null)
            {
                OutputWindowObject.CreatePane(debugPaneGuid, "debug", 1, 0);
                OutputWindowObject.GetPane(debugPaneGuid, out debugPane);

            }

            debugPane.OutputString(msg);
            debugPane.Activate(); // Brings this pane into view
        }

        /// <summary>
        /// 在自定义的输出窗体中显示信息
        /// </summary>
        /// <param name="paneName"></param>
        /// <param name="msg"></param>
        public static void ShowCustomerMessage(string paneName, string msg)
        {
            msg += "\n";

            Guid customGuid = new Guid("0F44E2D1-F5FA-4d2d-AB30-22BE123D97aa");
            IVsOutputWindowPane customPane;

            OutputWindowObject.CreatePane(ref customGuid, paneName, 1, 1);
            OutputWindowObject.GetPane(ref customGuid, out customPane);

            customPane.OutputString(msg);
            customPane.Activate();
        }
    }
}

