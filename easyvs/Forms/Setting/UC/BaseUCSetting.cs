using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace easyVS.Forms.Setting.UC
{
    public class BaseUCSetting : UserControl
    {
        #region - 变量 -

        private static SettingModel settingModel;

        /// <summary>
        /// 用于所有控件共同访问的全局配置模型
        /// </summary>
        public static SettingModel SettingModel
        {
            get { return settingModel ?? (settingModel = SettingFrm.ReadSetting()); }
            set { settingModel = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Read();
        }

        #endregion

        public virtual void Save() { }

        /// <summary>
        /// 用于负责每个控件读取各自的配置信息
        /// </summary>
        public virtual void Read() { }

    }

    [Serializable]
    public class SettingModel
    {
        #region - 变量 -

        [XmlElement]
        public QuickRegionSettingModel QuickRegionModel = new QuickRegionSettingModel();


        [XmlElement]
        public LessTabSettingModel LessTabModel = new LessTabSettingModel();


        [XmlElement]
        public NetWorkSettingModel NetWorkModel = new NetWorkSettingModel();


        [XmlElement]
        public AutoHeaderSettingModel AutoHeaderModel = new AutoHeaderSettingModel();

        [XmlElement]
        public AutoBraceSettingModel AutoBraceModel = new AutoBraceSettingModel();

        [XmlElement]
        public JumpInsertSettingModel JumpInsertModel = new JumpInsertSettingModel();

        [XmlElement]
        public TripleClickSettingModel TripleClickModel = new TripleClickSettingModel();

        [XmlElement]
        public ThemeSettingModel ThemeModel = new ThemeSettingModel();

        #endregion
    }
}
