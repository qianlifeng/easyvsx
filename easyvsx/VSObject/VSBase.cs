/******************************************************************************
 *  Author：       [user]
 *  CreateDate：   17/02/2012 10:36:56 AM
 *
 *
 ******************************************************************************/
using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.IO;

namespace easyvsx.VSObject
{
    public class VSBase
    {
        #region - 变量 -

        private static DTE2 applicationObject;
        private static IVsRunningDocumentTable runningDocumentTableObject;


        #endregion

        #region - 属性 -

        /// <summary>
        /// DTE对象
        /// </summary>
        public static DTE2 ApplicationObject
        {
            get
            {
                if (applicationObject == null)
                {
                    Object dteObj = Package.GetGlobalService(typeof(DTE));
                    if (dteObj == null)
                    {
                        throw new ArgumentNullException("获得失败");
                    }
                    applicationObject = dteObj as DTE2;
                }
                return applicationObject;
            }
        }

        public static IVsRunningDocumentTable RunningDocumentTableObject
        {
            get
            {
                if (runningDocumentTableObject == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsRunningDocumentTable));
                    if (obj == null)
                    {
                        throw new ArgumentNullException("获得失败");
                    }
                    runningDocumentTableObject = obj as IVsRunningDocumentTable;
                }
                return runningDocumentTableObject;
            }
        }

        #endregion

        #region - 枚举 -

        // These are found in <drive>:\Program Files\Microsoft Visual Studio x\Common7\IDE\ProjectTemplates\
        public struct ProjectType
        {
            public static string ConsoleApplication = "ConsoleApplication.zip";
            public static string WebApplication = "WebApplication.zip";
            public static string WebService = "WebService.zip";
            public static string ClassLibrary = "ClassLibrary.zip";
            public static string EmptyProject = "EmptyProject.zip";
            public static string WindowsApplication = "WindowsApplication.zip";
            public static string WindowsService = "WindowsService.zip";
        }

        public struct LanguageType
        {
            public static string CSharp = "CSharp";
            public static string VisualBasic = "vbproj";
        }

        // These are found in <drive>:\Program Files\Microsoft Visual Studio x\Common7\IDE\ItemTemplates\
        public struct DocumentType
        {
            public static string WebClass = "WebClass.zip";
            public static string WebForm = "WebForm.zip";
            public static string WebuserControl = "WebuserControl.zip";
            public static string WebHtmlPage = "WebHtmlPage.zip";
            public static string WebConfig = "WebConfig.zip";
            public static string GlobalAsax = "GlobalAsax.zip";
            public static string Class = "Class.zip";
            public static string Interface = "Interface.zip";
        }

        #endregion

        /// <summary>
        /// 执行一些系统内置的命令
        /// </summary>
        /// <param name="cmd">通常在VSConstants.VSStd2KCmdID这个里面</param>
        public static void ExecuteCommand(uint cmd)
        {
            IVsUIShell shell = Package.GetGlobalService(typeof(IVsUIShell)) as IVsUIShell;
            if (shell != null)
            {
                Guid std2k = VSConstants.VSStd2K;
                object arg = null;
                shell.PostExecCommand(ref std2k, cmd, 0, ref arg);
            }
        }

        /// <summary>
        /// 执行未知的命令
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="cmd"></param>
        public static void ExecuteCommand(Guid guid, int cmd)
        {
            IVsUIShell shell = Package.GetGlobalService(typeof(IVsUIShell)) as IVsUIShell;
            if (shell != null)
            {
                object arg = null;
                shell.PostExecCommand(ref guid, (uint)cmd, 0, ref arg);
            } 
        }

        /// <summary>
        /// 重启VS，如果有解决方案，这重启后会重新加载此解决方案
        /// </summary>
        public static void ReStartVS()
        {
            VSSolution.SaveAllFiles();

            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = currentProcess.MainModule.FileName;
            //查看当前是否项目打开
            if (VSSolution.CurrentSolution2 != null)
            {
                if (File.Exists(VSSolution.CurrentSolution2.FullName))
                {
                    info.Arguments = "\"" + VSSolution.CurrentSolution2.FullName + "\"";
                }
            }

            System.Diagnostics.Process.Start(info);

            currentProcess.Kill();
        }
    }
}
