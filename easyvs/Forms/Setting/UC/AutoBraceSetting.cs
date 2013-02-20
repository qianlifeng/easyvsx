using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using easyvsx.EventSink;
using Easy.Filter;
using System.Xml.Serialization;
using EasyCodeGenerate;

namespace easyVS.Forms.Setting.UC
{
    public partial class AutoBraceSetting : BaseUCSetting
    {
        //动画图像(来自嵌入资源)   
        Image animatedImage;
        //是否正在显示动画   
        bool currentlyAnimating = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            ImageAnimator.UpdateFrames();
        }

        public AutoBraceSetting()
        {
            InitializeComponent();

            animatedImage = Image.FromStream(GetType().Assembly.GetManifestResourceStream("easyVS.FunctionGif.autoBrace.gif"));
            pictureBox1.Image = animatedImage;
            ImageAnimator.Animate(animatedImage, (o, e) => { });
            DoubleBuffered = true;
        }

        public override void Read()
        {
            if (SettingModel.AutoBraceModel.OpenAutoBrace)
            {
                rbtTrue.Checked = true;
            }
            else
            {
                rbtFalse.Checked = true;
            }
        }

        public override void Save()
        {
            if (rbtTrue.Checked)
            {
                SettingModel.AutoBraceModel.OpenAutoBrace = true;
            }
            else
            {
                SettingModel.AutoBraceModel.OpenAutoBrace = false;
            }
        }
    }

    [Serializable]
    public class AutoBraceSettingModel
    {
        [XmlElement]
        public bool OpenAutoBrace = false;
    }
}
