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
    public partial class AutoHeaderSetting : BaseUCSetting
    {
        public AutoHeaderSetting()
        {
            InitializeComponent();
        }

        public override void Read()
        {
            if (SettingModel.AutoHeaderModel.OpenAutoHeader)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }

            richTextBox1.Text = SettingModel.AutoHeaderModel.AutoHeaderTemplete.Replace("\n", "\r\n");
        }

        public override void Save()
        {
            if (radioButton1.Checked && !string.IsNullOrEmpty(richTextBox1.Text))
            {
                //填写了内容才开启，否则关闭
                SettingModel.AutoHeaderModel.OpenAutoHeader = true;
            }
            else
            {
                SettingModel.AutoHeaderModel.OpenAutoHeader = false;
            }

            SettingModel.AutoHeaderModel.AutoHeaderTemplete = richTextBox1.Text;
        }

        private void btnResetTemplete_Click(object sender, EventArgs e)
        {
            richTextBox1.Text =
@"/**************************************************************** 
 *  作者：       [User]
 *  创建时间：   [CurrentTime]
 *
 ***************************************************************/
";
        }

    }

    [Serializable]
    public class AutoHeaderSettingModel
    {
        [XmlElement]
        public bool OpenAutoHeader = false;

        [XmlElement]
        public string AutoHeaderTemplete =
@"/**************************************************************** 
 *  作者：       [User]
 *  创建时间：   [CurrentTime]
 *
 ***************************************************************/
";
    }
}
