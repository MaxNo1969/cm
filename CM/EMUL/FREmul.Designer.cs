namespace CM
{
    partial class FREmul
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.inputSignals = new System.Windows.Forms.CheckedListBox();
            this.outputSignals = new System.Windows.Forms.CheckedListBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lb = new System.Windows.Forms.ListBox();
            this.pbTube = new System.Windows.Forms.ProgressBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnTubeModel = new System.Windows.Forms.Button();
            this.lblPos = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(2, 1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(132, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(140, 1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(140, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // inputSignals
            // 
            this.inputSignals.CheckOnClick = true;
            this.inputSignals.FormattingEnabled = true;
            this.inputSignals.Location = new System.Drawing.Point(5, 30);
            this.inputSignals.Name = "inputSignals";
            this.inputSignals.Size = new System.Drawing.Size(129, 244);
            this.inputSignals.TabIndex = 2;
            this.inputSignals.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.signals_ItemCheck);
            // 
            // outputSignals
            // 
            this.outputSignals.CheckOnClick = true;
            this.outputSignals.Enabled = false;
            this.outputSignals.FormattingEnabled = true;
            this.outputSignals.Location = new System.Drawing.Point(140, 30);
            this.outputSignals.Name = "outputSignals";
            this.outputSignals.Size = new System.Drawing.Size(140, 244);
            this.outputSignals.TabIndex = 3;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lb
            // 
            this.lb.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb.FormattingEnabled = true;
            this.lb.Location = new System.Drawing.Point(286, 56);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(303, 212);
            this.lb.TabIndex = 4;
            // 
            // pbTube
            // 
            this.pbTube.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbTube.Location = new System.Drawing.Point(5, 286);
            this.pbTube.Name = "pbTube";
            this.pbTube.Size = new System.Drawing.Size(418, 23);
            this.pbTube.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbTube.TabIndex = 5;
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(286, 30);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(303, 23);
            this.lblInfo.TabIndex = 6;
            // 
            // btnTubeModel
            // 
            this.btnTubeModel.Location = new System.Drawing.Point(286, 1);
            this.btnTubeModel.Name = "btnTubeModel";
            this.btnTubeModel.Size = new System.Drawing.Size(303, 23);
            this.btnTubeModel.TabIndex = 7;
            this.btnTubeModel.Text = "Труба(модель)";
            this.btnTubeModel.UseVisualStyleBackColor = true;
            this.btnTubeModel.Click += new System.EventHandler(this.btnTubeModel_Click);
            // 
            // lblPos
            // 
            this.lblPos.Location = new System.Drawing.Point(517, 286);
            this.lblPos.Name = "lblPos";
            this.lblPos.Size = new System.Drawing.Size(72, 23);
            this.lblPos.TabIndex = 8;
            // 
            // lblTimer
            // 
            this.lblTimer.Location = new System.Drawing.Point(434, 286);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(72, 23);
            this.lblTimer.TabIndex = 9;
            // 
            // FREmul
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 321);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.lblPos);
            this.Controls.Add(this.btnTubeModel);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pbTube);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.outputSignals);
            this.Controls.Add(this.inputSignals);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FREmul";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Эмуляция";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FREmul_FormClosing);
            this.Load += new System.EventHandler(this.FREmul_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckedListBox inputSignals;
        private System.Windows.Forms.CheckedListBox outputSignals;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.ProgressBar pbTube;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnTubeModel;
        private System.Windows.Forms.Label lblPos;
        private System.Windows.Forms.Label lblTimer;
    }
}