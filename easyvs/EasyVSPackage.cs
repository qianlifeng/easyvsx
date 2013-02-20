/******************************************************************************
 *  Author：       SQ1000
 *  CreateDate：   17/02/2012 10:44:16 AM
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Easy;
using Easy.Filter;
using EasyCodeGenerate.Menu.CodeRightClick;
using EasyCodeGenerate.Menu.TopMenu;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using easyVS.Core;
using easyVS.Forms;
using easyVS.Forms.Help;
using easyVS.Forms.Setting.UC;
using easyVS.Menu.NonMenuCommand;
using easyvsx;
using easyvsx.Markers;
using easyVS.Menu.TopMenu.Help;
using easyvsx.VSObject;
using easyVS.Menu.FileRightClick;
using System.Drawing;
using EnvDTE80;
using easyVS.Filter;

namespace EasyCodeGenerate
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidPackageTestPkgString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideService(typeof(MarkerTypeProvider))]
    [RegisterCustomerMarker]
    public sealed class EasyVSPackage : PackageBase
    {
        #region - 变量 -

        //必须要这样先拿到event变量，否则不触发
        DocumentEvents docEvents = VSBase.ApplicationObject.Events.DocumentEvents;
        WindowEvents windowEvents = VSBase.ApplicationObject.Events.WindowEvents;
        SolutionEvents solutionEvents = VSBase.ApplicationObject.Events.SolutionEvents;
        ProjectItemsEvents projectItemEvents = ((Events2)VSBase.ApplicationObject.Events).ProjectItemsEvents;

        //CommandEvents commandEvents = VSBase.ApplicationObject.Events.CommandEvents;

        #endregion

        #region - 构造函数 -

        public EasyVSPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }

        #endregion

        #region - 方法 -

        protected override void Initialize()
        {
            //这里是插件被第一次执行的时候进行的操作，只会在第一次执行的时候执行一次
            base.Initialize();

            //绑定命令
            //new Setting(this).Bind();
            //new UpdateModelFromDB(this).Bind();
            //new AddModelProject(this).Bind();
            //new QuickRegion(this).Bind();
            //new CloneCurrentLine(this).Bind();
            //new About(this).Bind();
            //new Suggestion(this).Bind();
            //new Update(this).Bind();
            //new OpenInExplore(this).Bind();
            //new RestartVS(this).Bind();

            //自动绑定所有的命令，使用反射查找继承了MenuCommand的子类
            //为了提高效率，只在MenuCommand所在的程序集中查找子类
            foreach (var type in this.GetType().Assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(MenuCommand)))
                {
                    MenuCommand command = Activator.CreateInstance(type, this) as MenuCommand;
                    if (command != null)
                    {
                        command.Bind();
                    }
                }
            }

            CheckOpenLessTab();
            CheckOpenAutoHeader();
            CheckOpenAutoBrace();
            //TextManagerEventSink.AddCommandFilter<CollapseXmlComment>();
            //TextManagerEventSink.RegisterViewEvent += new CollapseComment().TextManagerEventSink_RegisterViewEvent;
            VSBase.ApplicationObject.Events.DTEEvents.OnBeginShutdown += DTEEvents_OnBeginShutdown;

            hookVs.MouseEvent += new HookVS.MouseEventHandle(TripleClick.MouseEvent);
            hookVs.KeyUpEvent += new HookVS.KeyUpEventHandle(JumpInsert.KeyUpEvent);
            hookVs.KeyDownEvent += new HookVS.KeyUpEventHandle(JumpInsert.KeyDownEvent);

            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 if (UpdateFrm.IsNeedUpdate())
                                                 {
                                                     MessageBox.Show("detected a new version for easyvs, please use menu easyvs->help->update for update");
                                                 }
                                             });
        }

        void DTEEvents_OnBeginShutdown()
        {
            hookVs.Unhook();
        }

        /// <summary>
        /// 检查是否开启自动完成括号，引号功能
        /// </summary>
        private void CheckOpenAutoBrace()
        {
            TextManagerEventSink.AddCommandFilter<AutoBraceFilter>();
        }

        /// <summary>
        /// 检查是否开启AutoHeader功能
        /// </summary>
        private void CheckOpenAutoHeader()
        {
            docEvents.DocumentOpened += AutoHeader.DocumentOpenedEvent;
            projectItemEvents.ItemAdded += AutoHeader.projectItemEvents_ItemAdded;
        }

        /// <summary>
        /// 检查是否开启LessTab功能
        /// </summary>
        private void CheckOpenLessTab()
        {
            windowEvents.WindowActivated += LessTab.windowEvents_WindowActivated;
        }

        #endregion

    }
}