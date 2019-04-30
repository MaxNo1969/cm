namespace CM

{
    partial class FRTubeRawView
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.sb = new System.Windows.Forms.StatusStrip();
            this.tb = new System.Windows.Forms.ToolStrip();
            this.lblSections = new System.Windows.Forms.ToolStripLabel();
            this.txtStart = new System.Windows.Forms.ToolStripTextBox();
            this.lblCount = new System.Windows.Forms.ToolStripLabel();
            this.txtCount = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // sb
            // 
            this.sb.Location = new System.Drawing.Point(0, 401);
            this.sb.Name = "sb";
            this.sb.Size = new System.Drawing.Size(1067, 22);
            this.sb.TabIndex = 0;
            // 
            // tb
            // 
            this.tb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSections,
            this.txtStart,
            this.lblCount,
            this.txtCount,
            this.toolStripSeparator1});
            this.tb.Location = new System.Drawing.Point(0, 0);
            this.tb.Name = "tb";
            this.tb.Size = new System.Drawing.Size(1067, 25);
            this.tb.TabIndex = 1;
            // 
            // lblSections
            // 
            this.lblSections.Name = "lblSections";
            this.lblSections.Size = new System.Drawing.Size(170, 22);
            this.lblSections.Text = "Измерения: начало(0-000001)";
            // 
            // txtStart
            // 
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(100, 25);
            this.txtStart.Tag = "";
            this.txtStart.Text = "0";
            this.txtStart.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(96, 22);
            this.lblCount.Text = "количество(1-5)";
            // 
            // txtCount
            // 
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(100, 25);
            this.txtCount.Text = "1";
            this.txtCount.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // chart
            // 
            chartArea1.Name = "Area";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 25);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(1067, 376);
            this.chart.TabIndex = 2;
            // 
            // FRTubeRawView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 423);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.tb);
            this.Controls.Add(this.sb);
            this.Name = "FRTubeRawView";
            this.Text = "Данные по трубе";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRTubeRawView_FormClosing);
            this.Load += new System.EventHandler(this.FRTubeRawView_Load);
            this.tb.ResumeLayout(false);
            this.tb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip sb;
        private System.Windows.Forms.ToolStrip tb;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ToolStripLabel lblSections;
        private System.Windows.Forms.ToolStripTextBox txtStart;
        private System.Windows.Forms.ToolStripLabel lblCount;
        private System.Windows.Forms.ToolStripTextBox txtCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}