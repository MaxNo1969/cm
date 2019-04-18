namespace CM
{
    partial class FRTestL502
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRTestL502));
            this.tb = new System.Windows.Forms.ToolStrip();
            this.btnStartL502 = new System.Windows.Forms.ToolStripButton();
            this.tbView = new System.Windows.Forms.ToolStripButton();
            this.tbSaveData = new System.Windows.Forms.ToolStripButton();
            this.sb = new System.Windows.Forms.StatusStrip();
            this.Info = new System.Windows.Forms.ToolStripStatusLabel();
            this.DataSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.Time = new System.Windows.Forms.ToolStripStatusLabel();
            this.ucGr = new UCGraph();
            this.tb.SuspendLayout();
            this.sb.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb
            // 
            this.tb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartL502,
            this.tbView,
            this.tbSaveData});
            this.tb.Location = new System.Drawing.Point(0, 0);
            this.tb.Name = "tb";
            this.tb.Size = new System.Drawing.Size(758, 25);
            this.tb.TabIndex = 0;
            this.tb.Text = "";
            // 
            // btnStartL502
            // 
            this.btnStartL502.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStartL502.Image = ((System.Drawing.Image)(resources.GetObject("btnStartL502.Image")));
            this.btnStartL502.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartL502.Name = "btnStartL502";
            this.btnStartL502.Size = new System.Drawing.Size(42, 22);
            this.btnStartL502.Text = "Старт";
            this.btnStartL502.ToolTipText = "Запуск АЦП";
            this.btnStartL502.Click += new System.EventHandler(this.btnStartL502_Click);
            // 
            // tbView
            // 
            this.tbView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbView.Image = ((System.Drawing.Image)(resources.GetObject("tbView.Image")));
            this.tbView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbView.Name = "tbView";
            this.tbView.Size = new System.Drawing.Size(68, 22);
            this.tbView.Text = "Просмотр";
            this.tbView.Click += new System.EventHandler(this.tbView_Click);
            // 
            // tbSaveData
            // 
            this.tbSaveData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbSaveData.Enabled = false;
            this.tbSaveData.Image = ((System.Drawing.Image)(resources.GetObject("tbSaveData.Image")));
            this.tbSaveData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSaveData.Name = "tbSaveData";
            this.tbSaveData.Size = new System.Drawing.Size(91, 22);
            this.tbSaveData.Text = "Запись в файл";
            this.tbSaveData.Click += new System.EventHandler(this.tbSaveLcard_Click);
            // 
            // sb
            // 
            this.sb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Info,
            this.DataSize,
            this.Time});
            this.sb.Location = new System.Drawing.Point(0, 362);
            this.sb.Name = "sb";
            this.sb.Size = new System.Drawing.Size(758, 22);
            this.sb.TabIndex = 1;
            this.sb.Text = "";
            // 
            // Info
            // 
            this.Info.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.Info.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(639, 17);
            this.Info.Spring = true;
            this.Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DataSize
            // 
            this.DataSize.AutoSize = false;
            this.DataSize.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.DataSize.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.DataSize.Name = "DataSize";
            this.DataSize.Size = new System.Drawing.Size(55, 17);
            // 
            // Time
            // 
            this.Time.AutoSize = false;
            this.Time.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.Time.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(49, 17);
            // 
            // ucGr
            // 
            this.ucGr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGr.Location = new System.Drawing.Point(0, 25);
            this.ucGr.maxX = 0F;
            this.ucGr.maxY = 0F;
            this.ucGr.minY = 0F;
            this.ucGr.Name = "ucGr";
            this.ucGr.showGridX = false;
            this.ucGr.showGridY = false;
            this.ucGr.Size = new System.Drawing.Size(758, 337);
            this.ucGr.stepGridX = 50F;
            this.ucGr.stepGridY = 2F;
            this.ucGr.TabIndex = 2;
            this.ucGr.xScale = 50F;
            this.ucGr.yOffset = 168.5F;
            this.ucGr.yScale = 0.2F;
            // 
            // FRTestL502
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 384);
            this.Controls.Add(this.ucGr);
            this.Controls.Add(this.sb);
            this.Controls.Add(this.tb);
            this.Name = "FRTestL502";
            this.Text = "Тест LCard502";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRTestL502_FormClosing);
            this.Load += new System.EventHandler(this.FRTestL502_Load);
            this.tb.ResumeLayout(false);
            this.tb.PerformLayout();
            this.sb.ResumeLayout(false);
            this.sb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tb;
        private System.Windows.Forms.StatusStrip sb;
        private UCGraph ucGr;
        private System.Windows.Forms.ToolStripButton btnStartL502;
        private System.Windows.Forms.ToolStripStatusLabel Info;
        private System.Windows.Forms.ToolStripStatusLabel DataSize;
        private System.Windows.Forms.ToolStripStatusLabel Time;
        private System.Windows.Forms.ToolStripButton tbSaveData;
        private System.Windows.Forms.ToolStripButton tbView;
    }
}