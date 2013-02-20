/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 17:03:14
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using easyVS.Forms.Setting.UC;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using easyvsx.VSObject;

namespace easyVS.Forms
{
    public partial class SettingFrm : Form
    {
        #region - 变量 -

        /// <summary>
        /// win7配置文件放到程序数据区，否则访问权限不够VS崩溃
        /// </summary>
        public static string configDirectory = Environment.OSVersion.Version.Major < 6 ?
            AppDomain.CurrentDomain.BaseDirectory + "EasyVS" : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyVS";
        public static string configName = "EasyVSConfig.xml";

        QuickRegionSetting quickRegionControl = new QuickRegionSetting();

        LessTabSetting lessTabControl = new LessTabSetting();

        private NetworkSetting netWorkControl = new NetworkSetting();

        private AutoHeaderSetting autoHeaderControl = new AutoHeaderSetting();

        private ThemeSetting themeControl = new ThemeSetting();
        private AutoBraceSetting autoBraceControl = new AutoBraceSetting();
        private TripleClickSetting tripleClickControl = new TripleClickSetting();
        private JumpInsertSetting jumpInsertControl = new JumpInsertSetting();

        #endregion

        #region - 构造函数 -

        public SettingFrm()
        {
            InitializeComponent();
        }

        #endregion

        #region - 事件 -

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //展开所有的节点
            foreach (TreeNode item in treeView1.Nodes)
            {
                item.ExpandAll();
            }

            splitContainer1.Panel2.Controls.Add(quickRegionControl);
            splitContainer1.Panel2.Controls.Add(lessTabControl);
            splitContainer1.Panel2.Controls.Add(netWorkControl);
            splitContainer1.Panel2.Controls.Add(autoHeaderControl);
            splitContainer1.Panel2.Controls.Add(themeControl);
            splitContainer1.Panel2.Controls.Add(autoBraceControl);
            splitContainer1.Panel2.Controls.Add(tripleClickControl);
            splitContainer1.Panel2.Controls.Add(jumpInsertControl);

            ActivateOptionControl(netWorkControl);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (BaseUCSetting uc in splitContainer1.Panel2.Controls)
            {
                uc.Save();
            }

            SaveSetting(BaseUCSetting.SettingModel);

            MessageBox.Show("save successful");

            //清空缓存
            BaseUCSetting.SettingModel = null;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region - 方法 -

        /// <summary>
        /// 激活控件
        /// </summary>
        /// <param name="optionControl"></param>
        private void ActivateOptionControl(UserControl optionControl)
        {
            if (optionControl == null) throw new ArgumentNullException("optionControl");
            for (int i = 0; i < splitContainer1.Panel2.Controls.Count; i++)
            {
                splitContainer1.Panel2.Controls[i].Hide();
            }
            optionControl.Show();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "QuickRegion":
                    ActivateOptionControl(quickRegionControl);
                    break;

                case "LessTab":
                    ActivateOptionControl(lessTabControl);
                    break;

                case "Proxy Setting":
                    ActivateOptionControl(netWorkControl);
                    break;

                case "AutoHeader":
                    ActivateOptionControl(autoHeaderControl);
                    break;

                case "Theme":
                    ActivateOptionControl(themeControl);
                    break;

                case "AutoBrace":
                    ActivateOptionControl(autoBraceControl);
                    break;

                case "TripleClick":
                    ActivateOptionControl(tripleClickControl);
                    break;

                case "JumpInsert":
                    ActivateOptionControl(jumpInsertControl);
                    break;

                default:
                    break;
            }
        }

        public static void SaveSetting(SettingModel obj)
        {
            string xmlString;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingModel));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }

            StreamWriter sw = new StreamWriter(configDirectory + "\\" + configName, false);
            sw.Write(xmlString);
            sw.Close();

            //重置所有的配置（null时会重新从配置文件中读取）
            BaseUCSetting.SettingModel = null;

            if (ThemeSetting.ThemeChanged)
            {

                if (MessageBox.Show("Program has deteched that you have changed your theme, which need restart visual studio to make it work, do you want restart visual studio now?",
                    "Restart Visual Studio", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    VSBase.ReStartVS();
                }
            }
        }

        public static SettingModel ReadSetting()
        {
            SettingModel model = new SettingModel();
            //从解决方案目录下面读取配置信息
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }
            if (File.Exists(configDirectory + "\\" + configName))
            {
                using (StreamReader sr = new StreamReader(configDirectory + "\\" + configName))
                {
                    string xmlString = sr.ReadToEnd();

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingModel));
                    using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                        {
                            Object obj = xmlSerializer.Deserialize(xmlReader);
                            model = (SettingModel)obj;
                        }
                    }
                }
            }

            return model;
        }

        #endregion

    }
}
