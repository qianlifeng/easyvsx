using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Microsoft.VisualStudio.Shell;

namespace easyvsx
{
    public abstract class MenuCommand
    {
        #region  - 变量 -

        private readonly Package package;
        private readonly CommandID commandId;
        private OleMenuCommand oleMenuCommand;

        #endregion

        #region - 属性 -

        public CommandID CommandId
        {
            get { return commandId; }
        }

        protected Package Package
        {
            get { return package; }
        }

        protected IServiceProvider ServiceProvider
        {
            get { return package; }
        }

        protected OleMenuCommand OleMenuCommand
        {
            get { return oleMenuCommand; }
        }

        #endregion

        #region - 构造函数 -

        protected internal MenuCommand(PackageBase package)
        {
            this.package = package;
            foreach (object attr in GetType().GetCustomAttributes(false))
            {
                CommandIDAttribute idAttr = attr as CommandIDAttribute;
                if (idAttr != null)
                {
                    commandId = new CommandID(idAttr.Guid, (int)idAttr.Command);
                }
            }
        }

        #endregion

        #region - 需要被重写的方法 -

        protected virtual void OnExecute(OleMenuCommand command)
        {
        }

        protected virtual void OnQueryStatus(OleMenuCommand command)
        {
        }

        protected virtual void OnChange(OleMenuCommand command)
        {
        }

        #endregion

        #region - 方法 -



        public void Bind()
        {
            if (package == null)
            {
                return;
            }

            OleMenuCommandService mcs = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (mcs == null)
            {
                return;
            }

            oleMenuCommand = new OleMenuCommand(
              ExecuteMenuCommandCallback,
              ChangeCallback,
              BeforeStatusQueryCallback,
              commandId);
            mcs.AddCommand(oleMenuCommand);
        }

        private void ExecuteMenuCommandCallback(object sender, EventArgs e)
        {
            OleMenuCommand command = sender as OleMenuCommand;
            if (command != null) OnExecute(command);
        }

        private void ChangeCallback(object sender, EventArgs e)
        {
            OleMenuCommand command = sender as OleMenuCommand;
            if (command != null) OnChange(command);
        }

        private void BeforeStatusQueryCallback(object sender, EventArgs e)
        {
            OleMenuCommand command = sender as OleMenuCommand;
            if (command != null) OnQueryStatus(command);
        }

        #endregion
    }
}
