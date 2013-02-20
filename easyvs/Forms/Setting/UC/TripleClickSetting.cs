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
using System.Reflection;

namespace easyVS.Forms.Setting.UC
{
    public partial class TripleClickSetting : BaseUCSetting
    {
        //动画图像(来自嵌入资源)   
        Image animatedImage;
        //是否正在显示动画   
        bool currentlyAnimating = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            //得到动画的下一帧准备渲染   
            ImageAnimator.UpdateFrames();
        }   

        public TripleClickSetting()
        {
            InitializeComponent();

            animatedImage = Image.FromStream(GetType().Assembly.GetManifestResourceStream("easyVS.FunctionGif.tripleClick.gif"));
            pictureBox1.Image = animatedImage;
            ImageAnimator.Animate(animatedImage, (o,e) => { });
            DoubleBuffered = true;
        }

        public override void Read()
        {
            if (SettingModel.TripleClickModel.OpenTripleClick)
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
                SettingModel.TripleClickModel.OpenTripleClick = true;
            }
            else
            {
                SettingModel.TripleClickModel.OpenTripleClick = false;
            }
        }
    }

    [Serializable]
    public class TripleClickSettingModel
    {
        /// <summary>
        /// tripleclick暂时默认不开启，因为还有一些问题 
        /// </summary>
        [XmlElement]
        public bool OpenTripleClick = false;
    }
}
