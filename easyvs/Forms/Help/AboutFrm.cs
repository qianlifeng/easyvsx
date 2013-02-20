using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace easyVS.Forms.Help
{
    public partial class AboutFrm : Form
    {
        public AboutFrm()
        {
            InitializeComponent();

            textBox1.Text = @"
1. LessTab      自动关闭多余的tab
2. AutoHead     自动添加文件头信息
3. 右击任何文件增加“用windows explorer打开”选项
4. QuickRegion  快速整理当前文档的region，快捷键 Ctrl + E, Ctrl + R
5. 快速复制选中文字（不选中这复杂当前行）到下一行，快捷键 Ctrl + E, Ctrol + E
6. 主题更换
7. 快速重启
";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cnblogs.com/qianlifeng");
        }
    }
}
