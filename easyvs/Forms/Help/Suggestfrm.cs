using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using EasyCodeGenerate.Core;
using easyVS.Forms.Setting.UC;

namespace easyVS.Menu.TopMenu.Help
{
    public partial class SuggestFrm : Form
    {
        #region - 委托 -

        delegate void UpdateTextDelegate();

        delegate void CloseDelegate();

        #endregion

        #region - 构造函数 -

        public SuggestFrm()
        {
            InitializeComponent();
        }

        #endregion

        #region - 事件 -

        private void btsend_Click(object sender, EventArgs e)
        {
            string name = tbname.Text;
            string email = tbemail.Text;
            string advice = tbadvice.Text;
            if (name == "")
            {
                MessageBox.Show("您的称呼还没有填写。");
                return;
            }
            if (email == "")
            {
                MessageBox.Show("您的邮箱还没有填写。");
                return;
            }
            if (advice == "")
            {
                MessageBox.Show("您的建议还没有填写。");
                return;
            }

            string regexEmail = "\\w{1,}@\\w{1,}\\.\\w{1,}";
            Regex regex = new Regex(regexEmail, RegexOptions.IgnoreCase);
            MatchCollection matchCollection = regex.Matches(email);
            if (matchCollection.Count == 0)
            {
                MessageBox.Show("请填写正确的邮箱地址");
                tbemail.Focus();
                return;
            }

            const string title = "对EasyVS的建议";
            string content = "我是" + name + "。<br />" + "<br />" + advice
                + "<hr />我的邮箱是" + email;
            btsend.Enabled = false;
            tbadvice.Enabled = false;
            tbemail.Enabled = false;
            tbname.Enabled = false;
            btsend.Text = "正在发送";
            ThreadPool.QueueUserWorkItem(delegate { SendEmail(title, content); });
        }

        private void Suggestfrm_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 if (!IsOnLineProxy())
                                                 {
                                                     MessageBox.Show(@"程序检测到网络不可用，请联网后再给我发送建议!如果您需要使用代理，请到设置里面进行设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                     this.Invoke(new ThreadStart(Close));
                                                 }
                                             });
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:qianlf2008@163.com?subject='Easy使用建议'");
        }

        #endregion

        #region - 方法 -

        private void SendEmail(string title, string content)
        {
            try
            {
                //发送Internet邮件
                SmtpClient client = new SmtpClient("smtp.163.com");
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("easyvs_suggest@163.com", "easyvs");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("easyvs_suggest@163.com");
                mail.To.Add("qianlf2008@163.com");
                //mail.To.Add("抄送给谁，可以不填");
                mail.Subject = title;
                mail.BodyEncoding = System.Text.Encoding.Default;
                mail.Body = content;
                mail.IsBodyHtml = true;
                client.Send(mail);
                UpdateTextDelegate update = UpdateButtonText;
                Invoke(update);
            }
            catch
            {
                CloseDelegate close = closeSuggest;
                Invoke(close);
            }
        }

        private void UpdateButtonText()
        {
            btsend.Text = "发送成功，谢谢";
        }

        private void closeSuggest()
        {
            Close();
        }

        /// <summary>
        /// 判断当前网络是否连接
        /// </summary>
        /// <returns></returns>
        public static bool IsOnLine()
        {
            try
            {
                using (Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    soc.Connect("www.baidu.com", 80);
                }
                return true;
            }
            catch (SocketException socketException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsOnLineProxy()
        {
            string htmlString = HtmlHelper.GetHtmlString("http://www.baidu.com", null, null, Encoding.UTF8);
            if (string.IsNullOrEmpty(htmlString))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
