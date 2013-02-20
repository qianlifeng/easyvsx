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
    public partial class NetworkSetting : BaseUCSetting
    {
        public NetworkSetting()
        {
            InitializeComponent();
        }

        public override void Read()
        {

            tbAddrss.Text = SettingModel.NetWorkModel.Address;
            tbPort.Text = SettingModel.NetWorkModel.Port.ToString();
            tbUser.Text = SettingModel.NetWorkModel.User;
            tbPwd.Text = SettingModel.NetWorkModel.Pwd;

        }

        public override void Save()
        {
            SettingModel.NetWorkModel.Address = tbAddrss.Text;
            SettingModel.NetWorkModel.Port = Convert.ToInt32(tbPort.Text);
            SettingModel.NetWorkModel.User = tbUser.Text;
            SettingModel.NetWorkModel.Pwd = tbPwd.Text;
        }
    }


    [Serializable]
    public class NetWorkSettingModel
    {
        [XmlElement]
        public string Address = string.Empty;

        [XmlElement]
        public int Port = 80;

        [XmlElement]
        public string User = string.Empty;

        [XmlElement]
        public string Pwd = string.Empty;
    }
}
