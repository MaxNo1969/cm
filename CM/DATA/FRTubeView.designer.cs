namespace CM
{
    partial class FRTubeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRTubeView));
            this.sb = new System.Windows.Forms.StatusStrip();
            this.Info = new System.Windows.Forms.ToolStripStatusLabel();
            this.Zone = new System.Windows.Forms.ToolStripStatusLabel();
            this.PositionX = new System.Windows.Forms.ToolStripStatusLabel();
            this.PositionY = new System.Windows.Forms.ToolStripStatusLabel();
            this.Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.tbOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbReset = new System.Windows.Forms.ToolStripButton();
            this.tbFill = new System.Windows.Forms.ToolStripButton();
            this.ucTube = new UCTube();
            this.sb.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sb
            // 
            this.sb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Info,
            this.Zone,
            this.PositionX,
            this.PositionY,
            this.Value});
            this.sb.Location = new System.Drawing.Point(0, 439);
            this.sb.Name = "sb";
            this.sb.Size = new System.Drawing.Size(762, 24);
            this.sb.TabIndex = 3;
            this.sb.Text = "statusStrip1";
            // 
            // Info
            // 
            this.Info.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.Info.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(442, 19);
            this.Info.Spring = true;
            this.Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Zone
            // 
            this.Zone.AutoSize = false;
            this.Zone.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.Zone.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.Zone.Name = "Zone";
            this.Zone.Size = new System.Drawing.Size(132, 19);
            // 
            // PositionX
            // 
            this.PositionX.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.PositionX.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.PositionX.Name = "PositionX";
            this.PositionX.Size = new System.Drawing.Size(58, 19);
            this.PositionX.Text = "00.000 М";
            // 
            // PositionY
            // 
            this.PositionY.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.PositionY.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.PositionY.Name = "PositionY";
            this.PositionY.Size = new System.Drawing.Size(50, 19);
            this.PositionY.Text = "000 мм";
            // 
            // Value
            // 
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(34, 19);
            this.Value.Text = "0.000";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSave,
            this.tbOpen,
            this.toolStripSeparator1,
            this.tbReset,
            this.tbFill});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(762, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbSave
            // 
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbSave.Image")));
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(69, 22);
            this.tbSave.Text = "&Сохранить";
            this.tbSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // tbOpen
            // 
            this.tbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tbOpen.Image")));
            this.tbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbOpen.Name = "tbOpen";
            this.tbOpen.Size = new System.Drawing.Size(65, 22);
            this.tbOpen.Text = "&Загрузить";
            this.tbOpen.Click += new System.EventHandler(this.miLoad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbReset
            // 
            this.tbReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbReset.Image = ((System.Drawing.Image)(resources.GetObject("tbReset.Image")));
            this.tbReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbReset.Name = "tbReset";
            this.tbReset.Size = new System.Drawing.Size(63, 22);
            this.tbReset.Text = "&Очистить";
            this.tbReset.Click += new System.EventHandler(this.miReset_Click);
            // 
            // tbFill
            // 
            this.tbFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbFill.Image = ((System.Drawing.Image)(resources.GetObject("tbFill.Image")));
            this.tbFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbFill.Name = "tbFill";
            this.tbFill.Size = new System.Drawing.Size(70, 22);
            this.tbFill.Text = "&Заполнить";
            this.tbFill.Click += new System.EventHandler(this.miFill_Click);
            // 
            // ucTube
            // 
            this.ucTube.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTube.Location = new System.Drawing.Point(0, 25);
            this.ucTube.Name = "ucTube";
            this.ucTube.Size = new System.Drawing.Size(762, 414);
            this.ucTube.TabIndex = 6;
            // 
            // FRTubeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 463);
            this.Controls.Add(this.ucTube);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.sb);
            this.Name = "FRTubeView";
            this.Text = "Труба";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRTubeView_FormClosing);
            this.Load += new System.EventHandler(this.FRTubeModel_Load);
            this.sb.ResumeLayout(false);
            this.sb.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip sb;
        private System.Windows.Forms.ToolStripStatusLabel Info;
        private System.Windows.Forms.ToolStripStatusLabel Zone;
        private System.Windows.Forms.ToolStripStatusLabel PositionX;
        private System.Windows.Forms.ToolStripStatusLabel PositionY;
        private System.Windows.Forms.ToolStripStatusLabel Value;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.ToolStripButton tbOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbReset;
        private System.Windows.Forms.ToolStripButton tbFill;
        private UCTube ucTube;
    }
}