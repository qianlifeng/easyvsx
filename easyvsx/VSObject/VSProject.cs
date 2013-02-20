/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 13:54:20
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace easyvsx.VSObject
{
    public class VSProject : VSBase
    {
        #region - 属性 -

        public Project Project { get; set; }

        /// <summary>
        /// 获得解决方案下当前活动的项目
        /// </summary>
        public static Project ActiveProject
        {
            get
            {
                Array projects = (Array)ApplicationObject.ActiveSolutionProjects;
                if (projects != null && projects.Length > 0)
                {
                    return projects.GetValue(0) as Project;
                }


                projects = (Array)ApplicationObject.Solution.SolutionBuild.StartupProjects;
                if (projects != null && projects.Length > 0)
                {
                    return projects.GetValue(0) as Project;
                }

                return null;
            }
        }

        #endregion

        public VSProject(Project project)
        {
            Project = project;
        }

        public VSProject()
        {
        }

        #region - 公开方法 -

        #region - Get -

        /// <summary>
        /// 获得一个document的文件名
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string GetDocumentName(Document document)
        {
            if (document == null)
            {
                return string.Empty;
            }

            return document.FullName.Substring(document.FullName.LastIndexOf('\\') + 1);
        }



        #endregion

        #region - 操作 -

    


        /// <summary>
        ///创建基于模板的文档
        /// </summary>
        /// <param name="projectItems">某个项目下面的ProjectItems</param>
        /// <param name="documentType">DocumentType枚举</param>
        /// <param name="documentName">文档名字（不包含后缀）</param>
        public void CreateDocumentFromTemplete(ProjectItems projectItems, string documentType, string documentName)
        {
            if (VSSolution.CurrentSolution2 != null)
            {
                string projectTemplate = VSSolution.CurrentSolution2.GetProjectItemTemplate(documentType, "CSharp");
                if (!string.IsNullOrEmpty(projectTemplate))
                {
                    projectItems.AddFromTemplate(projectTemplate, documentName);
                }
            }
        }

        /// <summary>
        /// 将一个已经存在的文件加入项目
        /// </summary>
        /// <param name="projectItems">某个项目下面的ProjectItems</param>
        /// <param name="path">要加入的文件的路径</param>
        public void CreateDocumentFromCopy(ProjectItems projectItems, string path)
        {
            if (VSSolution.CurrentSolution2 != null)
            {
                if (!File.Exists(path))
                {
                    throw new NullReferenceException("要加载的文件不存在");
                }
                string givingName = path.Substring(path.LastIndexOf('\\') + 1, path.LastIndexOf('.') - path.LastIndexOf('\\') - 1);

                //检测是否已经存在该文件 

                List<ProjectItem> allFileProjectItem = GetAllFileProjectItem(projectItems);
                ProjectItem projectItem = allFileProjectItem.Find(i => i.Name == givingName);
                if (projectItem != null)
                {
                    MessageBox.Show("已经存在给定的文件");
                }
                else
                {
                    try
                    {
                        projectItems.AddFromFileCopy(path);
                    }
                    catch (COMException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 将指定字符串加入到一个新建的文件中去
        /// </summary>
        /// <param name="projectItems">某个项目下面的ProjectItems</param>
        /// <param name="code">文件内容</param>
        /// <param name="name">文件名（包含后缀）</param>
        public void CreateDocumentFromString(ProjectItems projectItems, string code, string name)
        {
            using (TempFolder tempFolder = new TempFolder())
            {
                string path = tempFolder.CreateTempFolder();

                using (FileStream fileStream = File.Create(path + "\\" + name))
                {
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.Write(code);
                    sw.Flush();
                    sw.Close();
                }

                CreateDocumentFromCopy(projectItems, path + "\\" + name);
            }
        }

        /// <summary>
        /// 删除一个ProjectItems下面的文件
        /// </summary>
        /// <param name="projectItems"></param>
        /// <param name="fileName">文件名字</param>
        public void RemoveDocument(ProjectItems projectItems, string fileName)
        {
            if (projectItems == null)
            {
                return;
            }

            foreach (ProjectItem item in projectItems)
            {
                if (fileName == item.Name)
                {
                    item.Delete();
                }
            }
        }

        /// <summary>
        /// 递归获得一个ProjectItems下面所有的file类型的ProjectItem
        /// </summary>
        public List<ProjectItem> GetAllFileProjectItemRecursion(ProjectItems items)
        {
            List<ProjectItem> list = new List<ProjectItem>();
            foreach (ProjectItem item in items)
            {
                switch (item.Kind)
                {
                    case VSConstants.ItemTypeGuid.PhysicalFile_string:
                        list.Add(item);
                        break;

                    case VSConstants.ItemTypeGuid.PhysicalFolder_string:
                        list.AddRange(GetAllFileProjectItemRecursion(item.ProjectItems));
                        break;
                }
            }
            return list;
        }

        /// <summary>
        /// 获得当前层级的ProjectItems下面所有的file类型的ProjectItem，不对子文件夹进行查找
        /// </summary>
        public List<ProjectItem> GetAllFileProjectItem(ProjectItems items)
        {
            List<ProjectItem> list = new List<ProjectItem>();
            foreach (ProjectItem item in items)
            {
                switch (item.Kind)
                {
                    case VSConstants.ItemTypeGuid.PhysicalFile_string:
                        list.Add(item);
                        if (item.ProjectItems != null)  //这种情况一般是针对aspx下面的aspx.cs进行的
                        {
                            list.AddRange(GetAllFileProjectItem(item.ProjectItems));
                        }
                        break;
                }
            }
            return list;
        }

        #endregion

        #endregion
    }
}
