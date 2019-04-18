namespace CM
{
    partial class FRADCData
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
            this.ucGr = new UCGraph();
            this.SuspendLayout();
            // 
            // ucGr
            // 
            this.ucGr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGr.Location = new System.Drawing.Point(0, 0);
            this.ucGr.Name = "ucGr";
            this.ucGr.showGridX = false;
            this.ucGr.showGridY = false;
            this.ucGr.Size = new System.Drawing.Size(665, 366);
            this.ucGr.stepGridX = 50F;
            this.ucGr.stepGridY = 2F;
            this.ucGr.TabIndex = 0;
            this.ucGr.yOffset = 183F;
            this.ucGr.yScale = 0.2F;
            // 
            // FRADCData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 366);
            this.Controls.Add(this.ucGr);
            this.Name = "FRADCData";
            this.Text = "АЦП";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRADCData_FormClosing);
            this.Load += new System.EventHandler(this.FRADCData_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UCGraph ucGr;
    }
}