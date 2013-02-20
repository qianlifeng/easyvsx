using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using easyVS.Core;
using easyvsx.VSObject;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using System.Runtime.InteropServices;
using easyVS.Forms.Setting.UC;
using System.Threading;

namespace easyVS.Menu.NonMenuCommand
{
    public class LessTab
    {
        public static AlwaysNewCollection collection = new AlwaysNewCollection(BaseUCSetting.SettingModel.LessTabModel.LessTabOpenCount);
        public static bool IsSolutionInit = true;

        public static void windowEvents_WindowActivated(Window gotfocus, Window lostfocus)
        {
            if (BaseUCSetting.SettingModel.LessTabModel.OpenLessTab)
            {
                Document activeDocument = gotfocus.Document;
                if (activeDocument != null)
                {

                    //当列表中的文件个数不足已经打开的文件个数时，则为启动项目的时候
                    //此时需要将所有已经打开的加入文件列表
                    //GetAllOpenedDocument的数量为已经打开新的窗口的数量，所以collection要加1
                    //比如现在有5个窗口已经打开，当我再次打开新的窗口的时候GetAllOpenedDocument此时已经为6了
                    //而ValidCount=5，所以这边就会永远成立。这会影响到后面isNewOpen的判断。
                    if (collection.ValidCount() + 1 < VSDocument.GetAllOpenedDocument().Count)
                    {
                        VSDocument.GetAllOpenedDocument().ForEach(doc => collection.Add(doc));
                    }

                    //在加入到列表之前，先判断是否存在
                    bool isNewOpen = !collection.Contains(activeDocument);

                    //不管是新打开的还是激活的，都重新排列列表
                    collection.Add(activeDocument);

                    //只有新开了文档才调用关闭方法
                    if (isNewOpen)
                    {
                        //关闭超过数量的tab
                        CloseTabNotInCollection();
                    }
                }
            }
        }

        public static void CloseTabNotInCollection()
        {
            List<Document> openedDocument = VSDocument.GetAllOpenedDocument();
            foreach (Document item in openedDocument)
            {
                if (!collection.Contains(item) && !string.IsNullOrEmpty(item.FullName) && item.Windows.Count != 0 /*一定要有窗口才能关闭，针对打开Winform的bug*/ )
                {
                    VSDocument.CloseDocument(item.FullName);
                }
            }
        }
    }
}
