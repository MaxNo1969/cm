namespace CM
{
    partial class FRHallSensorView
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
            this.sb = new System.Windows.Forms.StatusStrip();
            this.tb = new System.Windows.Forms.ToolStrip();
            this.lblSensorNum = new System.Windows.Forms.ToolStripLabel();
            this.cbSensorNum = new System.Windows.Forms.ToolStripComboBox();
            this.lblRow = new System.Windows.Forms.ToolStripLabel();
            this.cbRow = new System.Windows.Forms.ToolStripComboBox();
            this.lblStart = new System.Windows.Forms.ToolStripLabel();
            this.txtStart = new System.Windows.Forms.ToolStripTextBox();
            this.lblEnd = new System.Windows.Forms.ToolStripLabel();
            this.txtEnd = new System.Windows.Forms.ToolStripTextBox();
            this.ch = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ch)).BeginInit();
            this.SuspendLayout();
            // 
            // sb
            // 
            this.sb.Location = new System.Drawing.Point(0, 428);
            this.sb.Name = "sb";
            this.sb.Size = new System.Drawing.Size(1065, 22);
            this.sb.TabIndex = 0;
            this.sb.Text = "statusStrip1";
            // 
            // tb
            // 
            this.tb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSensorNum,
            this.cbSensorNum,
            this.lblRow,
            this.cbRow,
            this.lblStart,
            this.txtStart,
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.lblEnd,
            this.txtEnd,
            this.toolStripLabel2});
            this.tb.Location = new System.Drawing.Point(0, 0);
            this.tb.Name = "tb";
            this.tb.Size = new System.Drawing.Size(1065, 25);
            this.tb.TabIndex = 1;
            this.tb.Text = "toolStrip1";
            // 
            // lblSensorNum
            // 
            this.lblSensorNum.Name = "lblSensorNum";
            this.lblSensorNum.Size = new System.Drawing.Size(94, 22);
            this.lblSensorNum.Text = "Номер датчика:";
            // 
            // cbSensorNum
            // 
            this.cbSensorNum.AutoSize = false;
            this.cbSensorNum.Name = "cbSensorNum";
            this.cbSensorNum.Size = new System.Drawing.Size(50, 23);
            this.cbSensorNum.SelectedIndexChanged += new System.EventHandler(this.selectedIndexChanged);
            // 
            // lblRow
            // 
            this.lblRow.Name = "lblRow";
            this.lblRow.Size = new System.Drawing.Size(86, 22);
            this.lblRow.Text = "Датчик Холла:";
            // 
            // cbRow
            // 
            this.cbRow.AutoSize = false;
            this.cbRow.Name = "cbRow";
            this.cbRow.Size = new System.Drawing.Size(50, 23);
            this.cbRow.SelectedIndexChanged += new System.EventHandler(this.selectedIndexChanged);
            // 
            // lblStart
            // 
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(52, 22);
            this.lblStart.Text = "Начало:";
            // 
            // txtStart
            // 
            this.txtStart.AutoSize = false;
            this.txtStart.Name = "txtStart";
            this.txtStart.ReadOnly = true;
            this.txtStart.Size = new System.Drawing.Size(50, 25);
            this.txtStart.TextChanged += new System.EventHandler(this.selectedIndexChanged);
            // 
            // lblEnd
            // 
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(44, 22);
            this.lblEnd.Text = "Конец:";
            // 
            // txtEnd
            // 
            this.txtEnd.AutoSize = false;
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.ReadOnly = true;
            this.txtEnd.Size = new System.Drawing.Size(50, 25);
            this.txtEnd.TextChanged += new System.EventHandler(this.selectedIndexChanged);
            // 
            // ch
            // 
            this.ch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ch.Location = new System.Drawing.Point(0, 25);
            this.ch.Name = "ch";
            this.ch.Size = new System.Drawing.Size(1065, 403);
            this.ch.TabIndex = 2;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(25, 22);
            this.toolStripLabel1.Text = "мм";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(25, 22);
            this.toolStripLabel2.Text = "мм";
            // 
            // FRHallSensorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 450);
            this.Controls.Add(this.ch);
            this.Controls.Add(this.tb);
            this.Controls.Add(this.sb);
            this.Name = "FRHallSensorView";
            this.Text = "FRHallSensorView";
            this.tb.ResumeLayout(false);
            this.tb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip sb;
        private System.Windows.Forms.ToolStrip tb;
        private System.Windows.Forms.ToolStripLabel lblSensorNum;
        private System.Windows.Forms.ToolStripComboBox cbSensorNum;
        private System.Windows.Forms.ToolStripLabel lblRow;
        private System.Windows.Forms.ToolStripComboBox cbRow;
        private System.Windows.Forms.DataVisualization.Charting.Chart ch;
        private System.Windows.Forms.ToolStripLabel lblStart;
        private System.Windows.Forms.ToolStripLabel lblEnd;
        private System.Windows.Forms.ToolStripTextBox txtStart;
        private System.Windows.Forms.ToolStripTextBox txtEnd;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    }
}