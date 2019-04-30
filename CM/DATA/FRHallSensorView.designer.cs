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
            this.lblStartMeas = new System.Windows.Forms.ToolStripLabel();
            this.txtStartMeas = new System.Windows.Forms.ToolStripTextBox();
            this.lblCntMeas = new System.Windows.Forms.ToolStripLabel();
            this.txtCountMeas = new System.Windows.Forms.ToolStripTextBox();
            this.ch = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            this.lblStartMeas,
            this.txtStartMeas,
            this.lblCntMeas,
            this.txtCountMeas});
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
            // lblStartMeas
            // 
            this.lblStartMeas.Name = "lblStartMeas";
            this.lblStartMeas.Size = new System.Drawing.Size(134, 22);
            this.lblStartMeas.Text = "Начальное измерение:";
            // 
            // txtStartMeas
            // 
            this.txtStartMeas.AutoSize = false;
            this.txtStartMeas.Name = "txtStartMeas";
            this.txtStartMeas.Size = new System.Drawing.Size(50, 25);
            this.txtStartMeas.TextChanged += new System.EventHandler(this.selectedIndexChanged);
            // 
            // lblCntMeas
            // 
            this.lblCntMeas.Name = "lblCntMeas";
            this.lblCntMeas.Size = new System.Drawing.Size(75, 22);
            this.lblCntMeas.Text = "Количество:";
            // 
            // txtCountMeas
            // 
            this.txtCountMeas.AutoSize = false;
            this.txtCountMeas.Name = "txtCountMeas";
            this.txtCountMeas.Size = new System.Drawing.Size(50, 25);
            this.txtCountMeas.TextChanged += new System.EventHandler(this.selectedIndexChanged);
            // 
            // ch
            // 
            this.ch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ch.Location = new System.Drawing.Point(0, 25);
            this.ch.Name = "ch";
            this.ch.Size = new System.Drawing.Size(1065, 403);
            this.ch.TabIndex = 2;
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
        private System.Windows.Forms.ToolStripLabel lblStartMeas;
        private System.Windows.Forms.ToolStripLabel lblCntMeas;
        private System.Windows.Forms.ToolStripTextBox txtStartMeas;
        private System.Windows.Forms.ToolStripTextBox txtCountMeas;
    }
}