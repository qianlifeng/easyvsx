using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using easyVS.Menu.TopMenu.Help;
using EasyCodeGenerate.Core;
using System.Reflection;
using System.Diagnostics;

namespace easyVS.Forms.Help
{
    public partial class UpdateFrm : Form
    {
        public static double currentVersion = 0.41;

        public UpdateFrm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            lblCurrentVersion.Text = currentVersion.ToString();
            linkLabel1.Visible = false;

            ThreadPool.QueueUserWorkItem(delegate
            {
                if (!SuggestFrm.IsOnLineProxy())
                {
                    MessageBox.Show(@"network is not available, if you use proxy, please configure it with menu easyvs->setting->proxy settting", "tips", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Invoke(new ThreadStart(Close));
                }
                else
                {
                    StartUpdate();
                }
            });
            base.OnLoad(e);
        }

        private void StartUpdate()
        {
            string html = HtmlHelper.GetHtmlString("http://www.cnblogs.com/qianlifeng/archive/2012/01/09/2317625.html", null, null, Encoding.UTF8);
            List<string> listStr = HtmlHelper.GetStrings("current version:", "</div>", html);
            if (listStr.Count > 0)
            {
                double newVersion = Convert.ToDouble(listStr[0]);
                if (newVersion > currentVersion)
                {
                    List<string> des = HtmlHelper.GetStrings("current version feature:", "</div>", html);
                    List<string> updateLinkStr = HtmlHelper.GetStrings("current version update site:", "</div>", html);
                    
                    if (des.Count > 0 && updateLinkStr.Count > 0)
                    {
                        Invoke(new ThreadStart(() => {
                            textBox1.Text = "";
                            foreach (string item in des[0].Split(';'))
                            {
                                textBox1.Text += item + "\r\n"; 
                            } 
                            
                            lblNewVersion.ForeColor = Color.Red;
                            linkLabel1.Visible = true;
                            linkLabel1.LinkClicked += (sender, e) => { Process.Start(updateLinkStr[0]); };
                            lblNewVersion.Text = newVersion.ToString();
                        }));
                    }
                }
                else
                {
                    Invoke(new ThreadStart(() =>
                    {
                        lblNewVersion.Text = newVersion.ToString();
                        textBox1.Text = "";
                    }));
                    MessageBox.Show("current version is latest");
                }
            }
        }

        /// <summary>
        /// 检查是否需要更新
        /// </summary>
        /// <returns></returns>
        public static bool IsNeedUpdate()
        {
            string html = HtmlHelper.GetHtmlString("http://www.cnblogs.com/qianlifeng/archive/2012/01/09/2317625.html",
                                                   null, null, Encoding.UTF8);
            List<string> listStr = HtmlHelper.GetStrings("current version:", "</div>", html);
            if (listStr.Count > 0)
            {
                double newVersion = Convert.ToDouble(listStr[0]);
                if (newVersion > currentVersion)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
