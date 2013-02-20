namespace easyVS.Forms
{
    partial class MoveToRegionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Node0");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Node8");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Node9");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Node10");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Node4", new System.Windows.Forms.TreeNode[] {
            treeNode16,
            treeNode17,
            treeNode18});
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Node5");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Node6");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22});
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Node11");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Node12");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Node13");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Node2", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25,
            treeNode26});
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Node3");
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode15.Name = "Node0";
            treeNode15.Text = "Node0";
            treeNode16.Name = "Node8";
            treeNode16.Text = "Node8";
            treeNode17.Name = "Node9";
            treeNode17.Text = "Node9";
            treeNode18.Name = "Node10";
            treeNode18.Text = "Node10";
            treeNode19.Name = "Node4";
            treeNode19.Text = "Node4";
            treeNode20.Name = "Node5";
            treeNode20.Text = "Node5";
            treeNode21.Name = "Node6";
            treeNode21.Text = "Node6";
            treeNode22.Name = "Node7";
            treeNode22.Text = "Node7";
            treeNode23.Name = "Node1";
            treeNode23.Text = "Node1";
            treeNode24.Name = "Node11";
            treeNode24.Text = "Node11";
            treeNode25.Name = "Node12";
            treeNode25.Text = "Node12";
            treeNode26.Name = "Node13";
            treeNode26.Text = "Node13";
            treeNode27.Name = "Node2";
            treeNode27.Text = "Node2";
            treeNode28.Name = "Node3";
            treeNode28.Text = "Node3";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode15,
            treeNode23,
            treeNode27,
            treeNode28});
            this.treeView1.Size = new System.Drawing.Size(148, 262);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 236);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(148, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Maroon;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 21);
            this.toolStripStatusLabel1.Text = "Close";
            this.toolStripStatusLabel1.ToolTipText = "HotKey: Ese";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // MoveToRegionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(148, 262);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MoveToRegionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MoveToRegionForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MoveToRegionForm_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;


    }
}