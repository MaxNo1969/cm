namespace TubeViewer
{
    partial class FRAllSensorsViewTV
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
            this.ucTubeView = new CM.UCTubeAllSensors();
            this.SuspendLayout();
            // 
            // ucTubeView
            // 
            this.ucTubeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTubeView.Location = new System.Drawing.Point(0, 0);
            this.ucTubeView.Name = "ucTubeView";
            this.ucTubeView.Size = new System.Drawing.Size(800, 450);
            this.ucTubeView.TabIndex = 0;
            // 
            // FRAllSensorsViewTV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ucTubeView);
            this.Name = "FRAllSensorsViewTV";
            this.Text = "Просмотр трубы по датчикам";
            this.Resize += new System.EventHandler(this.FRAllSensorsView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private CM.UCTubeAllSensors ucTubeView;
    }
}