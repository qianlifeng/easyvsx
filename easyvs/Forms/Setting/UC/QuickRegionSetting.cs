/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 17:13:48
 *
 *
 ******************************************************************************/
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
    public partial class QuickRegionSetting : BaseUCSetting
    {
        //动画图像(来自嵌入资源)   
        Image animatedImage;
        //是否正在显示动画   
        bool currentlyAnimating = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            ImageAnimator.UpdateFrames();
        }


        public QuickRegionSetting()
        {
            InitializeComponent();

            animatedImage = Image.FromStream(GetType().Assembly.GetManifestResourceStream("easyVS.FunctionGif.autoRegion.gif"));
            pictureBox1.Image = animatedImage;
            ImageAnimator.Animate(animatedImage, (o, e) => { });
            DoubleBuffered = true;
        }

        public override void Read()
        {
            tbVariable.Text = SettingModel.QuickRegionModel.Variable;
            tbProperty.Text = SettingModel.QuickRegionModel.Property;
            tbMethod.Text = SettingModel.QuickRegionModel.Method;
            tbEvent.Text = SettingModel.QuickRegionModel.Event;
            tbContructor.Text = SettingModel.QuickRegionModel.Constructor;
        }

        public override void Save()
        {
            SettingModel.QuickRegionModel.Variable = tbVariable.Text; ;
            SettingModel.QuickRegionModel.Property = tbProperty.Text;
            SettingModel.QuickRegionModel.Method = tbMethod.Text;
            SettingModel.QuickRegionModel.Event = tbEvent.Text;
            SettingModel.QuickRegionModel.Constructor = tbContructor.Text;
        }
    }

    [Serializable]
    public class QuickRegionSettingModel
    {
        [XmlElement]
        public string Variable = "- Variable -";

        [XmlElement]
        public string Property = "- Property -";

        [XmlElement]
        public string Constructor = "- Constructor -";

        [XmlElement]
        public string Event = "- Event -";

        [XmlElement]
        public string Method = "- Method -";
    }
}
