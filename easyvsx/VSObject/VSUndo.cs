using System;
using EnvDTE;

namespace easyvsx.VSObject
{
    public class VSUndo : VSBase, IDisposable
    {
        public static UndoContext undoContext;

        //这里返回VSUndo是为了使用using调用结束的时候，自动调用dispose方法来关闭undocontext
        public static VSUndo StartUndo()
        {
            undoContext = ApplicationObject.UndoContext;
            undoContext.Open(Guid.NewGuid().ToString());
            return new VSUndo();
        }

        public void Dispose()
        {
            undoContext.Close();
        }

    }
}
