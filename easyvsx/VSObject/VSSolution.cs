/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 13:54:26
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

namespace easyvsx.VSObject
{
    /// <summary>
    /// 解决方案类，具有操作project的能力
    /// </summary>
    public class VSSolution : VSBase
    {
        #region - 变量 -

        private static IVsSolution solutionObject;
        private static Solution2 solution2;

        #endregion

        #region - 属性 -

        /// <summary>
        /// 获得全局的Solution接口对象
        /// </summary>
        public static IVsSolution SolutionObject
        {
            get
            {
                if (solutionObject == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsSolution));
                    if (obj == null)
                    {
                        throw new ArgumentNullException("获得失败");
                    }
                    solutionObject = obj as IVsSolution;
                }
                return solutionObject;
            }
        }

        public static Solution2 CurrentSolution2
        {
            get
            {
                if (ApplicationObject != null && ApplicationObject.Solution != null)
                {
                    return ApplicationObject.Solution as Solution2;
                }
                return null;
            }
        }

        protected static Solution CurrentSolution
        {
            get
            {
                return ApplicationObject.Solution;
            }
        }

        /// <summary>
        /// 获得当前解决方案的目录信息
        /// </summary>
        public DirectoryInfo SolutionDirectory
        {
            get
            {
                try
                {
                    FileInfo fi = new FileInfo(CurrentSolution.FullName);
                    return fi.Directory;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }



        #endregion

        #region - 公开方法 -

        /// <summary>
        /// 等同于保存全部按钮
        /// </summary>
        public static void SaveAllFiles()
        {
            VSBase.ExecuteCommand(new Guid("5EFC7975-14BC-11CF-9B2B-00AA00573819"), 224);
        }

        /// <summary>
        /// 根据项目名字寻找项目
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public Project GetProjectByName(string projectName)
        {
            foreach (Project project in CurrentSolution2.Projects)
            {
                if (project.Name == projectName)
                {
                    return project;
                }   
            }

            return null;
        }

        /// <summary>
        /// 在当前解决方案下创建一个项目
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="projectName"></param>
        public void CreateProject(string projectType, string projectName)
        {
            CreateProject(projectType, GetCurrentSolutionFolder(), projectName);
        }

        /// <summary>
        /// 在当前解决方案下创建一个项目
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="projectFolderPath">项目路径</param>
        /// <param name="projectName"></param>
        public void CreateProject(string projectType, string projectFolderPath, string projectName)
        {
            if (CurrentSolution2 != null && !string.IsNullOrEmpty(projectFolderPath))
            {
                CreateProject(CurrentSolution2, projectType, LanguageType.CSharp, projectFolderPath + @"\" + projectName);
            }
            else
            {
                if (string.IsNullOrEmpty(projectFolderPath))
                {
                    MessageBox.Show(@"解决方案不存在");
                }
            }
        }

        public void ChangeProjectName(Project project,string name)
        {

        }

        /// <summary>
        /// 返回当前solution所在的目录（\\结尾）
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentSolutionFolder()
        {
            if (CurrentSolution2 != null)
            {
                if (!String.IsNullOrEmpty(CurrentSolution2.FullName))
                {
                    return CurrentSolution2.FullName.Substring(0, CurrentSolution2.FullName.LastIndexOf('\\')+1);
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 获得解决方案中所有的项目集合
        /// </summary>
        /// <returns></returns>
        public static Projects GetProjectsInSolution()
        {
            if (CurrentSolution2 == null)
            {
                return null;
            }
            return CurrentSolution2.Projects;
        }

        #endregion

        #region - 私有方法 -

        private static void CreateProject(Solution2 solution, string projType, string projLanguage, string projPath)
        {
            try
            {
                string projectName = projPath.Substring(projPath.LastIndexOf('\\') + 1);
                string templatePath = solution.GetProjectTemplate(projType, projLanguage);
                solution.AddFromTemplate(templatePath, projPath, projectName);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        #endregion
    }
}
