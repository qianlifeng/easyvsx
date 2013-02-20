using easyVS.Menu.TopMenu.Help;
using EasyCodeGenerate.Core;
using System.Management;

namespace easyVS.Core
{
    public class EasyVSStatistics
    {
        public static void SendStart()
        {
            if (SuggestFrm.IsOnLineProxy())
            {
                string mac = GetNetCardMacAddress();
                string name = System.Environment.UserName;
                string uri = string.Format("http://www.scottqian.com/Home/AcceptSoftwareUse?Mac={0}&name={1}&software={2}", mac, name, "easyvs");
                string s = HtmlHelper.GetHtmlString(uri, null, null, null);
            }
        }

        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardMacAddress()
        {
            ManagementClass mc;
            ManagementObjectCollection moc;
            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            moc = mc.GetInstances();
            string str = "";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    str = mo["MacAddress"].ToString();
                    break;
                }
            }
            return str;
        }
    }
}
