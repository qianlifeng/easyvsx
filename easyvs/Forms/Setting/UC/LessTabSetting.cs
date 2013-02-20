using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace easyVS.Forms.Setting.UC
{
    public partial class LessTabSetting : BaseUCSetting
    {
        public LessTabSetting()
        {
            InitializeComponent();

            tbOpenCount.LostFocus += tbOpenCount_LostFocus;
        }

        void tbOpenCount_LostFocus(object sender, EventArgs e)
        {
            int s;
            if (!int.TryParse(tbOpenCount.Text,out s))
            {
                MessageBox.Show("请填写正确的数量");
                tbOpenCount.Focus();
            }
        }

        public override void Read()
        {
            if (SettingModel.LessTabModel.OpenLessTab)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }

            tbOpenCount.Text = SettingModel.LessTabModel.LessTabOpenCount.ToString();

        }

        public override void Save()
        {
            if (radioButton1.Checked)
            {
                SettingModel.LessTabModel.OpenLessTab = true;
            }
            else
            {
                SettingModel.LessTabModel.OpenLessTab = false;
            }

            SettingModel.LessTabModel.LessTabOpenCount = Convert.ToInt32(tbOpenCount.Text);
        }
    
    }

    [Serializable]
    public class LessTabSettingModel
    {
        [XmlElement]
        public bool OpenLessTab = true;

        [XmlElement]
        public int LessTabOpenCount = 8;
    }
}
