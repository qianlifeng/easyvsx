using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using EnvDTE;
using easyvsx.VSObject;

namespace easyVS.Forms
{
    public partial class MoveToRegionForm : Form
    {
        #region - 变量 -

        public CodeElement classElement;

        #endregion

        #region - Delegate -

        public delegate void SelecteRegion(RegionElement region);

        #endregion

        #region - 构造函数 -

        public MoveToRegionForm(CodeElement classElement)
        {
            this.classElement = classElement;
            InitializeComponent();

            BindRegionToTreeView();
        }

        #endregion

        #region - 事件 -

        protected override void OnLoad(EventArgs e)
        {
            treeView1.ExpandAll();

            //获得当前鼠标位置
            Point p = new Point();
            GetCursorPos(ref p);

            Top = p.Y - 10;
            Left = p.X - 10;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region - 方法 -

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point p);

        private void BindRegionToTreeView()
        {
            treeView1.Nodes.Clear();
            VSCodeModel codeModel = new VSCodeModel();
            List<RegionElement> regions = codeModel.GetRegionsInClass(classElement);
            foreach (RegionElement item in regions)
            {
                TreeNode node = new TreeNode(item.RegionName);
                node.Tag = item;
                RecursionAddChildren(item, node);
                treeView1.Nodes.Add(node);
            }
        }

        private void RecursionAddChildren(RegionElement item, TreeNode node)
        {
            if (item.Children.Count>0)
            {
                foreach (RegionElement child in item.Children)
                {
                    TreeNode childNode = new TreeNode(child.RegionName);
                    childNode.Tag = child;
                    RecursionAddChildren(child, childNode);
                    node.Nodes.Add(childNode);

                }
            }
        }

        #endregion


        public event SelecteRegion OnSelectRegion;

        private void MoveToRegionForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RegionElement region = (RegionElement)e.Node.Tag;
            if (OnSelectRegion !=null)
            {
                OnSelectRegion(region);
            }

            Close();
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (treeView1.SelectedNode != null)
                {
                    RegionElement region = (RegionElement)treeView1.SelectedNode.Tag;
                    if (OnSelectRegion != null)
                    {
                        OnSelectRegion(region);
                    }

                    Close();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

    }
}
