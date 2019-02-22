using System.Windows.Forms;

namespace CM
{
    partial class FRListEditor
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LB = new System.Windows.Forms.ListBox();
            this.pGrid = new PropertyGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BDelete = new System.Windows.Forms.Button();
            this.BAdd = new System.Windows.Forms.Button();
            this.BCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(11, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pGrid);
            this.splitContainer1.Size = new System.Drawing.Size(345, 307);
            this.splitContainer1.SplitterDistance = 140;
            this.splitContainer1.TabIndex = 15;
            // 
            // LB
            // 
            this.LB.BackColor = System.Drawing.SystemColors.Window;
            this.LB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB.FormattingEnabled = true;
            this.LB.Location = new System.Drawing.Point(0, 0);
            this.LB.Name = "LB";
            this.LB.Size = new System.Drawing.Size(140, 307);
            this.LB.TabIndex = 8;
            this.LB.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.LB.DragDrop += new System.Windows.Forms.DragEventHandler(this.LB_DragDrop);
            this.LB.DragOver += new System.Windows.Forms.DragEventHandler(this.listBox1_DragOver);
            this.LB.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.LB_QueryContinueDrag);
            this.LB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            this.LB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseMove);
            this.LB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LB_MouseUp);
            // 
            // pdView1
            // 
            this.pGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGrid.Location = new System.Drawing.Point(0, 0);
            this.pGrid.Name = "pGrid";
            this.pGrid.Size = new System.Drawing.Size(201, 307);
            this.pGrid.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(36, 4);
            // 
            // BDelete
            // 
            this.BDelete.Location = new System.Drawing.Point(177, 325);
            this.BDelete.Name = "BDelete";
            this.BDelete.Size = new System.Drawing.Size(77, 23);
            this.BDelete.TabIndex = 14;
            this.BDelete.Text = "Удалить";
            this.BDelete.UseVisualStyleBackColor = true;
            this.BDelete.Click += new System.EventHandler(this.BDelete_Click);
            // 
            // BAdd
            // 
            this.BAdd.Location = new System.Drawing.Point(11, 325);
            this.BAdd.Name = "BAdd";
            this.BAdd.Size = new System.Drawing.Size(77, 23);
            this.BAdd.TabIndex = 13;
            this.BAdd.Text = "Добавить";
            this.BAdd.UseVisualStyleBackColor = true;
            this.BAdd.Click += new System.EventHandler(this.BAdd_Click);
            // 
            // BCopy
            // 
            this.BCopy.Location = new System.Drawing.Point(94, 325);
            this.BCopy.Name = "BCopy";
            this.BCopy.Size = new System.Drawing.Size(77, 23);
            this.BCopy.TabIndex = 16;
            this.BCopy.Text = "Копировать";
            this.BCopy.UseVisualStyleBackColor = true;
            this.BCopy.Click += new System.EventHandler(this.BCopy_Click);
            // 
            // FLBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 455);
            this.Controls.Add(this.BCopy);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.BDelete);
            this.Controls.Add(this.BAdd);
            this.Name = "FLBase";
            this.Text = "FLBaseT";
            this.Activated += new System.EventHandler(this.FLBase_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FLBaseT_FormClosed);
            this.Load += new System.EventHandler(this.FLBaseT_Load);
            this.Resize += new System.EventHandler(this.FLBaseT_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        protected System.Windows.Forms.ListBox LB;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.Button BDelete;
        public System.Windows.Forms.Button BAdd;
        public System.Windows.Forms.Button BCopy;
        private PropertyGrid pGrid;

    }
}