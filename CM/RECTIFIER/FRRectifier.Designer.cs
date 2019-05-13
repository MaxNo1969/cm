namespace CM
{
    partial class FRRectifier
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
            this.lblI = new System.Windows.Forms.Label();
            this.lblU = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblI
            // 
            this.lblI.AutoSize = true;
            this.lblI.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblI.Location = new System.Drawing.Point(14, 6);
            this.lblI.Name = "lblI";
            this.lblI.Size = new System.Drawing.Size(86, 25);
            this.lblI.TabIndex = 0;
            this.lblI.Text = "I:00.0В";
            // 
            // lblU
            // 
            this.lblU.AutoSize = true;
            this.lblU.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblU.Location = new System.Drawing.Point(4, 49);
            this.lblU.Name = "lblU";
            this.lblU.Size = new System.Drawing.Size(96, 25);
            this.lblU.TabIndex = 1;
            this.lblU.Text = "U:00.0А";
            // 
            // FRRectifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(104, 81);
            this.ControlBox = false;
            this.Controls.Add(this.lblU);
            this.Controls.Add(this.lblI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRRectifier";
            this.Text = "Блок питания";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRRectifier_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FRRectifier_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblI;
        private System.Windows.Forms.Label lblU;
    }
}