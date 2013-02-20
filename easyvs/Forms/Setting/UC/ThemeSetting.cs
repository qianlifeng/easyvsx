/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 16:59:36
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
using easyvsx.VSObject;
using System.IO;

namespace easyVS.Forms.Setting.UC
{
    public partial class ThemeSetting : BaseUCSetting
    {

        public static bool ThemeChanged = false;

        public ThemeSetting()
        {
            InitializeComponent();

        }

        public override void Read()
        {
            //加载所有主题名字
            foreach (string item in Enum.GetNames(typeof(ThemeNames)))
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedItem = SettingModel.ThemeModel.ThemeName;
        }

        public override void Save()
        {

            if (comboBox1.Text == ThemeNames.Default.ToString())
            {
                Default();
            }
            if (comboBox1.Text == ThemeNames.SonOfObsidian.ToString())
            {
                SonOfObsidian();
            }
            if (comboBox1.Text == ThemeNames.CodingHorror.ToString())
            {
                CodingHorror();
            }
            if (comboBox1.Text == ThemeNames.Nina.ToString())
            {
                Nina();
            }
            if (comboBox1.Text == ThemeNames.WekeRoad.ToString())
            {
                WekeRoad();
            }
            if (comboBox1.Text == ThemeNames.Green.ToString())
            {
                Green();
            }
            if (comboBox1.Text == ThemeNames.LightForEye.ToString())
            {
                LightForEye();
            }

            VSFontColor.RefreshTextEditCache();
            if (SettingModel.ThemeModel.ThemeName != comboBox1.Text)
            {
                ThemeChanged = true;
            }
            SettingModel.ThemeModel.ThemeName = comboBox1.Text;
        }

        #region 主题设置

        private void SonOfObsidian()
        {
            VSFontColor.SetTextEditColor("Comment", 0x007B7466, 0x02000000);
            VSFontColor.SetTextEditColor("Plain Text", 0x00F3F2F1, 0x002A2822);
            VSFontColor.SetTextEditColor("Selected Text", 0x00FFFFFF, 0x0064614F);
            VSFontColor.SetTextEditColor("Brace Matching (Rectangle)", 0x02000000, 0x00505050);
            VSFontColor.SetTextEditColor("Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Number", 0x0022CDFF, 0x02000000);
            VSFontColor.SetTextEditColor("Operator", 0x00B7E2E8, 0x02000000);
            VSFontColor.SetTextEditColor("String", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("String(C# @ Verbatim)", 0x0010C2EF, 0x02000000);
            VSFontColor.SetTextEditColor("urlformat", 0x00F9DA99, 0x02000000);
            VSFontColor.SetTextEditColor("User Types", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Enums)", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Interfaces)", 0x00B48C8C, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Delegates)", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Value types)", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("Indicator Margin", 0x02000000, 0x00F0F0F0);
            VSFontColor.SetTextEditColor("Line Numbers", 0x00494E3F, 0x00343129);
            VSFontColor.SetTextEditColor("Preprocessor Keyword", 0x00BD82A0, 0x02000000);
            VSFontColor.SetTextEditColor("Keyword", 0x0063C793, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Comment", 0x008AA399, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Tag", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Comment", 0x007B7466, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Keyword", 0x0022CDFF, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Name", 0x00B48C8C, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Value", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Selector", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("CSS String Value", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute Value", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Comment", 0x007B7466, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Element Name", 0x0063C793, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Entity", 0x00BD82A0, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Operator", 0x00E4E2E0, 0x00343129);
            VSFontColor.SetTextEditColor("HTML Server-Side Script", 0x00B48C8C, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Tag Delimiter", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Razor Code", 0x02000000, 0x003E3C32);
            VSFontColor.SetTextEditColor("Script Comment", 0x007B7466, 0x02000000);
            VSFontColor.SetTextEditColor("Script Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Keyword", 0x0063C793, 0x02000000);
            VSFontColor.SetTextEditColor("Script Number", 0x0022CDFF, 0x02000000);
            VSFontColor.SetTextEditColor("Script Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script String", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Value", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("XML CData Section", 0x008AA399, 0x02000000);
            VSFontColor.SetTextEditColor("XML Comment", 0x007B7466, 0x02000000);
            VSFontColor.SetTextEditColor("XML Delimiter", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Name", 0x0063C793, 0x02000000);
            VSFontColor.SetTextEditColor("XML Text", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Value", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("XAML CData Section", 0x008AA399, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Comment", 0x007B7466, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Delimiter", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Class", 0x00B48C8C, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Name", 0x00B18C67, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Value", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Name", 0x0063C793, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Inactive Selected Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.square", 0x00555555, 0x00E2E2E2);
            VSFontColor.SetTextEditColor("outlining.verticalrule", 0x006F5C25, 0x02000000);
            VSFontColor.SetTextEditColor("Syntax Error", 0x000076EC, 0x02000000);
            VSFontColor.SetTextEditColor("Compiler Error", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("Warning", 0x0022CDFF, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.collapsehintadornment", 0x00514E3E, 0x02000000);
            VSFontColor.SetTextEditColor("Collapsible Text", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Excluded Code", 0x00616161, 0x02000000);
            VSFontColor.SetTextEditColor("MarkerFormatDefinition/HighlightedReference", 0x02000000, 0x00808080);
            VSFontColor.SetTextEditColor("urlformat", 0x00F9DA99, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x00E4E2E0, 0x00343129);
        }

        private void CodingHorror()
        {
            //以下代码有python自动生成
            VSFontColor.SetTextEditColor("Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("Plain Text", 0x02000000, 0x00F8F8F8);
            VSFontColor.SetTextEditColor("Selected Text", 0x02000000, 0x0033FF9E);
            VSFontColor.SetTextEditColor("Brace Matching (Rectangle)", 0x02000000, 0x00CCE0DB);
            VSFontColor.SetTextEditColor("Identifier", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("Number", 0x02000000, 0x00FFFFE6);
            VSFontColor.SetTextEditColor("Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("String", 0x00000000, 0x00E6FFFF);
            VSFontColor.SetTextEditColor("String(C# @ Verbatim)", 0x00000000, 0x00E6FFFF);
            VSFontColor.SetTextEditColor("urlformat", 0x00000000, 0x02000000);
            VSFontColor.SetTextEditColor("User Types", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Enums)", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Interfaces)", 0x00006BD6, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Delegates)", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Value types)", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("Indicator Margin", 0x02000000, 0x00F0F0F0);
            VSFontColor.SetTextEditColor("Line Numbers", 0x00FFFFFF, 0x00808080);
            VSFontColor.SetTextEditColor("Preprocessor Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Keyword", 0x00800000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Tag", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Name", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Selector", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS String Value", 0x00000000, 0x00E6FFFF);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute Value", 0x00800000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Element Name", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Entity", 0x00000000, 0x00FFFFE6);
            VSFontColor.SetTextEditColor("HTML Operator", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Server-Side Script", 0x02000000, 0x0052EFCD);
            VSFontColor.SetTextEditColor("HTML Tag Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Razor Code", 0x02000000, 0x00F2EFE7);
            VSFontColor.SetTextEditColor("Script Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("Script Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script String", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Value", 0x00800000, 0x02000000);
            VSFontColor.SetTextEditColor("XML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Value", 0x00800000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Class", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Name", 0x000053A6, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Value", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Inactive Selected Text", 0x02000000, 0x00DBCDBF);
            VSFontColor.SetTextEditColor("outlining.square", 0x00555555, 0x00E2E2E2);
            VSFontColor.SetTextEditColor("outlining.verticalrule", 0x00A5A5A5, 0x02000000);
            VSFontColor.SetTextEditColor("Syntax Error", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("Compiler Error", 0x00922626, 0x02000000);
            VSFontColor.SetTextEditColor("Warning", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.collapsehintadornment", 0x00A3786C, 0x00ECE2DF);
            VSFontColor.SetTextEditColor("Collapsible Text", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Excluded Code", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("MarkerFormatDefinition/HighlightedReference", 0x02000000, 0x00CCE0DB);
        }

        private void Nina()
        {
            //以下代码有python自动生成
            VSFontColor.SetTextEditColor("Comment", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("Plain Text", 0x00ECEEEE, 0x002E1E10);
            VSFontColor.SetTextEditColor("Selected Text", 0x02000000, 0x0019450D);
            VSFontColor.SetTextEditColor("Brace Matching (Rectangle)", 0x02000000, 0x00CCE0DB);
            VSFontColor.SetTextEditColor("Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("String", 0x00A87FAD, 0x02000000);
            VSFontColor.SetTextEditColor("String(C# @ Verbatim)", 0x00A87FAD, 0x02000000);
            VSFontColor.SetTextEditColor("urlformat", 0x00A87FAD, 0x02000000);
            VSFontColor.SetTextEditColor("User Types", 0x0034E28A, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Enums)", 0x0000D4ED, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Interfaces)", 0x0000D4ED, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Delegates)", 0x0000D4ED, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Value types)", 0x0000D4ED, 0x02000000);
            VSFontColor.SetTextEditColor("Indicator Margin", 0x02000000, 0x00F0F0F0);
            VSFontColor.SetTextEditColor("Line Numbers", 0x00AF912B, 0x00FFFFFF);
            VSFontColor.SetTextEditColor("Preprocessor Keyword", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("Keyword", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Comment", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Tag", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Comment", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Keyword", 0x0000D4ED, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Name", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Value", 0x0034E28A, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Selector", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("CSS String Value", 0x0034E28A, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute Value", 0x0034E28A, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Comment", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Element Name", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Entity", 0x00A87FAD, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Server-Side Script", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Tag Delimiter", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Razor Code", 0x02000000, 0x0019450D);
            VSFontColor.SetTextEditColor("Script Comment", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("Script Identifier", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("Script Keyword", 0x0000D4ED, 0x02000000);
            VSFontColor.SetTextEditColor("Script Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script String", 0x00A87FAD, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Quotes", 0x0034E28A, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Value", 0x0034E28A, 0x02000000);
            VSFontColor.SetTextEditColor("XML CData Section", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("XML Comment", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("XML Delimiter", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Name", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("XML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Quotes", 0x00000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Value", 0x0034E28A, 0x02000000);
            VSFontColor.SetTextEditColor("XAML CData Section", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Comment", 0x00858A88, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Delimiter", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Class", 0x0000D4ED, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Name", 0x00A87FAD, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Value", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Name", 0x00CF9F72, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Inactive Selected Text", 0x02000000, 0x0019450D);
            VSFontColor.SetTextEditColor("outlining.square", 0x00555555, 0x00E2E2E2);
            VSFontColor.SetTextEditColor("outlining.verticalrule", 0x00A5A5A5, 0x02000000);
            VSFontColor.SetTextEditColor("Syntax Error", 0x00FF00FF, 0x02000000);
            VSFontColor.SetTextEditColor("Compiler Error", 0x00FFED00, 0x02000000);
            VSFontColor.SetTextEditColor("Warning", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.collapsehintadornment", 0x00E8DDD7, 0x002E1E10);
            VSFontColor.SetTextEditColor("Collapsible Text", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Excluded Code", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("MarkerFormatDefinition/HighlightedReference", 0x02000000, 0x0019450D);
            VSFontColor.SetTextEditColor("Breakpoint (Enabled)", 0x00FFFFFF, 0x00463A96);
            VSFontColor.SetTextEditColor("Current Statement", 0x00000000, 0x0062EEFF);
        }

        private void WekeRoad()
        {
            //以下代码有python自动生成
            VSFontColor.SetTextEditColor("Comment", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Plain Text", 0x00FFFFFF, 0x00000000);
            VSFontColor.SetTextEditColor("Selected Text", 0x02000000, 0x00800000);
            VSFontColor.SetTextEditColor("Brace Matching (Rectangle)", 0x02000000, 0x00588479);
            VSFontColor.SetTextEditColor("Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("String", 0x005CC2A5, 0x02000000);
            VSFontColor.SetTextEditColor("String(C# @ Verbatim)", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("urlformat", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("User Types", 0x006DC6FF, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Enums)", 0x006DC6FF, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Interfaces)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Delegates)", 0x006DC6FF, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Value types)", 0x006DC6FF, 0x02000000);
            VSFontColor.SetTextEditColor("Indicator Margin", 0x02000000, 0x00F0F0F0);
            VSFontColor.SetTextEditColor("Line Numbers", 0x00AF912B, 0x00000000);
            VSFontColor.SetTextEditColor("Preprocessor Keyword", 0x003248DA, 0x02000000);
            VSFontColor.SetTextEditColor("Keyword", 0x003278CC, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Tag", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Comment", 0x00FFFF00, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Keyword", 0x003278CC, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Name", 0x00BEF4FC, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Value", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Selector", 0x00BEF4FC, 0x02000000);
            VSFontColor.SetTextEditColor("CSS String Value", 0x00FFFF00, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute Value", 0x005CC2A5, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Element Name", 0x006DC6FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Entity", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Operator", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Server-Side Script", 0x00BB9768, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Tag Delimiter", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("Razor Code", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Comment", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Script Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Keyword", 0x003278CC, 0x02000000);
            VSFontColor.SetTextEditColor("Script Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script String", 0x0050C2A5, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Value", 0x0050C2A5, 0x02000000);
            VSFontColor.SetTextEditColor("XML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Comment", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Delimiter", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("XML Name", 0x003278CC, 0x02000000);
            VSFontColor.SetTextEditColor("XML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Value", 0x0050C2A5, 0x02000000);
            VSFontColor.SetTextEditColor("XAML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Comment", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Delimiter", 0x00FFFFFF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Class", 0x006DC6FF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Name", 0x003278CC, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Value", 0x00BEF4FC, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Name", 0x003278CC, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Inactive Selected Text", 0x02000000, 0x00695744);
            VSFontColor.SetTextEditColor("outlining.square", 0x00555555, 0x00E2E2E2);
            VSFontColor.SetTextEditColor("outlining.verticalrule", 0x00A5A5A5, 0x02000000);
            VSFontColor.SetTextEditColor("Syntax Error", 0x0028289A, 0x02000000);
            VSFontColor.SetTextEditColor("Compiler Error", 0x00C95959, 0x02000000);
            VSFontColor.SetTextEditColor("Warning", 0x00195C19, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.collapsehintadornment", 0x000B578E, 0x000D161C);
            VSFontColor.SetTextEditColor("Collapsible Text", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Excluded Code", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("MarkerFormatDefinition/HighlightedReference", 0x02000000, 0x0021362D);
        }

        private void Default()
        {
            //以下代码有python自动生成
            VSFontColor.SetTextEditColor("Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("Plain Text", 0x02000000, 0x00FFFFFF);
            VSFontColor.SetTextEditColor("Selected Text", 0x02000000, 0x00FF9933);
            VSFontColor.SetTextEditColor("Brace Matching (Rectangle)", 0x02000000, 0x00CCE0DB);
            VSFontColor.SetTextEditColor("Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("String", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("String(C# @ Verbatim)", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("urlformat", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("User Types", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Enums)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Interfaces)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Delegates)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Value types)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("Indicator Margin", 0x02000000, 0x00F0F0F0);
            VSFontColor.SetTextEditColor("Line Numbers", 0x00AF912B, 0x00FFFFFF);
            VSFontColor.SetTextEditColor("Preprocessor Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Tag", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Name", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Selector", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS String Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Element Name", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Entity", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Operator", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Server-Side Script", 0x02000000, 0x0000FFFF);
            VSFontColor.SetTextEditColor("HTML Tag Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Razor Code", 0x02000000, 0x00F2EFE7);
            VSFontColor.SetTextEditColor("Script Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("Script Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script String", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Quotes", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Class", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Name", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Inactive Selected Text", 0x02000000, 0x00DBCDBF);
            VSFontColor.SetTextEditColor("outlining.square", 0x00555555, 0x00E2E2E2);
            VSFontColor.SetTextEditColor("outlining.verticalrule", 0x00A5A5A5, 0x02000000);
            VSFontColor.SetTextEditColor("Syntax Error", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("Compiler Error", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Warning", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.collapsehintadornment", 0x00E8DDD7, 0x00FAF7F6);
            VSFontColor.SetTextEditColor("Collapsible Text", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Excluded Code", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("MarkerFormatDefinition/HighlightedReference", 0x02000000, 0x00CCE0DB);
        }

        private void Green()
        {
            //以下代码有python自动生成
            VSFontColor.SetTextEditColor("Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("Plain Text", 0x00000000, 0x00B8D2B7);
            VSFontColor.SetTextEditColor("Selected Text", 0x02000000, 0x00C988E7);
            VSFontColor.SetTextEditColor("Brace Matching (Rectangle)", 0x02000000, 0x00CCE0DB);
            VSFontColor.SetTextEditColor("Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("String", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("String(C# @ Verbatim)", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("urlformat", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("User Types", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Enums)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Interfaces)", 0x00B08A0C, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Delegates)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Value types)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("Indicator Margin", 0x02000000, 0x00F0F0F0);
            VSFontColor.SetTextEditColor("Line Numbers", 0x00D5AC20, 0x00DFE0C2);
            VSFontColor.SetTextEditColor("Preprocessor Keyword", 0x00F81212, 0x02000000);
            VSFontColor.SetTextEditColor("Keyword", 0x00F13B3B, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Tag", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Name", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Selector", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS String Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Element Name", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Entity", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Operator", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Server-Side Script", 0x02000000, 0x0099F4F4);
            VSFontColor.SetTextEditColor("HTML Tag Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Razor Code", 0x02000000, 0x00F2EFE7);
            VSFontColor.SetTextEditColor("Script Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("Script Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script String", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Quotes", 0x00000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Quotes", 0x00000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Class", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Name", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Inactive Selected Text", 0x02000000, 0x00DBCDBF);
            VSFontColor.SetTextEditColor("outlining.square", 0x00555555, 0x00E2E2E2);
            VSFontColor.SetTextEditColor("outlining.verticalrule", 0x00A5A5A5, 0x02000000);
            VSFontColor.SetTextEditColor("Syntax Error", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("Compiler Error", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Warning", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.collapsehintadornment", 0x00D2BDB2, 0x00D7BBB2);
            VSFontColor.SetTextEditColor("Collapsible Text", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Excluded Code", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("MarkerFormatDefinition/HighlightedReference", 0x02000000, 0x00CCE0DB);
            VSFontColor.SetTextEditColor("Breakpoint (Enabled)", 0x00FFFFFF, 0x00463A96);
            VSFontColor.SetTextEditColor("Current Statement", 0x00000000, 0x0062EEFF);
        }

        private void LightForEye()
        {
            //以下代码有python自动生成
            VSFontColor.SetTextEditColor("Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("Plain Text", 0x00000000, 0x00D6D6D6);
            VSFontColor.SetTextEditColor("Selected Text", 0x02000000, 0x00FF9933);
            VSFontColor.SetTextEditColor("Brace Matching (Rectangle)", 0x02000000, 0x00CCE0DB);
            VSFontColor.SetTextEditColor("Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("String", 0x001515A3, 0x00D6F4FF);
            VSFontColor.SetTextEditColor("String(C# @ Verbatim)", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("urlformat", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("User Types", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Enums)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Interfaces)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Delegates)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("User Types(Value types)", 0x00AF912B, 0x02000000);
            VSFontColor.SetTextEditColor("Indicator Margin", 0x02000000, 0x00F0F0F0);
            VSFontColor.SetTextEditColor("Line Numbers", 0x00AF912B, 0x00FFFFFF);
            VSFontColor.SetTextEditColor("Preprocessor Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Comment", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Doc Tag", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Name", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Property Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("CSS Selector", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("CSS String Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Element Name", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Entity", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Operator", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("HTML Server-Side Script", 0x02000000, 0x0000FFFF);
            VSFontColor.SetTextEditColor("HTML Tag Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Razor Code", 0x02000000, 0x00F2EFE7);
            VSFontColor.SetTextEditColor("Script Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("Script Identifier", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Keyword", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Number", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script Operator", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Script String", 0x00000080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Quotes", 0x00000000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML CData Section", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("XML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Quotes", 0x00000000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Attribute Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML CData Section", 0x00808080, 0x00FFFFFF);
            VSFontColor.SetTextEditColor("XAML Comment", 0x00006400, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Delimiter", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Class", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Name", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Markup Extension Parameter Value", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Name", 0x001515A3, 0x02000000);
            VSFontColor.SetTextEditColor("XAML Text", 0x02000000, 0x02000000);
            VSFontColor.SetTextEditColor("Inactive Selected Text", 0x02000000, 0x00DBCDBF);
            VSFontColor.SetTextEditColor("outlining.square", 0x00555555, 0x00E2E2E2);
            VSFontColor.SetTextEditColor("outlining.verticalrule", 0x00A5A5A5, 0x02000000);
            VSFontColor.SetTextEditColor("Syntax Error", 0x000000FF, 0x02000000);
            VSFontColor.SetTextEditColor("Compiler Error", 0x00FF0000, 0x02000000);
            VSFontColor.SetTextEditColor("Warning", 0x00008000, 0x02000000);
            VSFontColor.SetTextEditColor("outlining.collapsehintadornment", 0x00E8DDD7, 0x00FAF7F6);
            VSFontColor.SetTextEditColor("Collapsible Text", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("Excluded Code", 0x00808080, 0x02000000);
            VSFontColor.SetTextEditColor("MarkerFormatDefinition/HighlightedReference", 0x02000000, 0x00CCE0DB);
            VSFontColor.SetTextEditColor("Breakpoint (Enabled)", 0x00FFFFFF, 0x00463A96);
            VSFontColor.SetTextEditColor("Current Statement", 0x00000000, 0x0062EEFF);
        }

        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == ThemeNames.Default.ToString())
            {
                Stream strm = GetType().Assembly.GetManifestResourceStream("easyVS.ThemePreview.default.png");
                pictureBox1.Image = new Bitmap(Image.FromStream(strm));
                strm.Close();
            }
            if (comboBox1.Text == ThemeNames.SonOfObsidian.ToString())
            {
                Stream strm = GetType().Assembly.GetManifestResourceStream("easyVS.ThemePreview.son-of-obsidian.png");
                pictureBox1.Image = new Bitmap(Image.FromStream(strm));
                strm.Close();
            }
            if (comboBox1.Text == ThemeNames.CodingHorror.ToString())
            {
                Stream strm = GetType().Assembly.GetManifestResourceStream("easyVS.ThemePreview.codinghorror.png");
                pictureBox1.Image = new Bitmap(Image.FromStream(strm));
                strm.Close();
            }
            if (comboBox1.Text == ThemeNames.Nina.ToString())
            {
                Stream strm = GetType().Assembly.GetManifestResourceStream("easyVS.ThemePreview.nina.png");
                pictureBox1.Image = new Bitmap(Image.FromStream(strm));
                strm.Close();
            }
            if (comboBox1.Text == ThemeNames.WekeRoad.ToString())
            {
                Stream strm = GetType().Assembly.GetManifestResourceStream("easyVS.ThemePreview.WekeRoad.png");
                pictureBox1.Image = new Bitmap(Image.FromStream(strm));
                strm.Close();
            }
            if (comboBox1.Text == ThemeNames.Green.ToString())
            {
                Stream strm = GetType().Assembly.GetManifestResourceStream("easyVS.ThemePreview.green.png");
                pictureBox1.Image = new Bitmap(Image.FromStream(strm));
                strm.Close();
            }
            if (comboBox1.Text == ThemeNames.LightForEye.ToString())
            {
                Stream strm = GetType().Assembly.GetManifestResourceStream("easyVS.ThemePreview.lightForEye.png");
                pictureBox1.Image = new Bitmap(Image.FromStream(strm));
                strm.Close();
            }
        }

    }

    [Serializable]
    public class ThemeSettingModel
    {
        [XmlElement]
        public string ThemeName = ThemeNames.Default.ToString();
    }

    /// <summary>
    /// 所有主题的集合
    /// </summary>
    public enum ThemeNames
    {
        Default,
        SonOfObsidian,
        CodingHorror,
        Nina,
        WekeRoad,
        Green,
        LightForEye
    }
}
