namespace TubeViewer
{
    partial class FRMain
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
            this.mm = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadDump = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAsDump = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAsCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewAllSensors = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.ts = new System.Windows.Forms.ToolStrip();
            this.mNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mm.SuspendLayout();
            this.SuspendLayout();
            // 
            // mm
            // 
            this.mm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.menuWindows});
            this.mm.Location = new System.Drawing.Point(0, 0);
            this.mm.MdiWindowListItem = this.menuWindows;
            this.mm.Name = "mm";
            this.mm.Size = new System.Drawing.Size(800, 24);
            this.mm.TabIndex = 1;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoad,
            this.menuLoadDump,
            this.menuLoadCSV,
            this.toolStripMenuItem3,
            this.mNew,
            this.toolStripMenuItem1,
            this.menuSave,
            this.menuSaveAs,
            this.toolStripMenuItem2,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(48, 20);
            this.menuFile.Text = "&Файл";
            // 
            // menuLoad
            // 
            this.menuLoad.Name = "menuLoad";
            this.menuLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuLoad.Size = new System.Drawing.Size(180, 22);
            this.menuLoad.Text = "Загрузить";
            this.menuLoad.Click += new System.EventHandler(this.Load_Click);
            // 
            // menuLoadDump
            // 
            this.menuLoadDump.Name = "menuLoadDump";
            this.menuLoadDump.Size = new System.Drawing.Size(180, 22);
            this.menuLoadDump.Text = "Загрузить Дамп";
            this.menuLoadDump.Click += new System.EventHandler(this.LoadDump_Click);
            // 
            // menuLoadCSV
            // 
            this.menuLoadCSV.Name = "menuLoadCSV";
            this.menuLoadCSV.Size = new System.Drawing.Size(180, 22);
            this.menuLoadCSV.Text = "Загрузить CSV";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // menuSave
            // 
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(180, 22);
            this.menuSave.Text = "Сохранить";
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveAsDump,
            this.menuSaveAsCsv});
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAs.Text = "Сохранить как...";
            // 
            // menuSaveAsDump
            // 
            this.menuSaveAsDump.Name = "menuSaveAsDump";
            this.menuSaveAsDump.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAsDump.Text = "Дамп";
            // 
            // menuSaveAsCsv
            // 
            this.menuSaveAsCsv.Name = "menuSaveAsCsv";
            this.menuSaveAsCsv.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAsCsv.Text = "CSV";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuExit.Size = new System.Drawing.Size(180, 22);
            this.menuExit.Text = "&Выход";
            this.menuExit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewAllSensors});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(76, 20);
            this.menuView.Text = "&Просмотр";
            // 
            // menuViewAllSensors
            // 
            this.menuViewAllSensors.Name = "menuViewAllSensors";
            this.menuViewAllSensors.Size = new System.Drawing.Size(140, 22);
            this.menuViewAllSensors.Text = "Все датчики";
            this.menuViewAllSensors.Click += new System.EventHandler(this.ViewAllSensors_Click);
            // 
            // menuWindows
            // 
            this.menuWindows.Name = "menuWindows";
            this.menuWindows.Size = new System.Drawing.Size(47, 20);
            this.menuWindows.Text = "&Окна";
            // 
            // ss
            // 
            this.ss.Location = new System.Drawing.Point(0, 428);
            this.ss.Name = "ss";
            this.ss.Size = new System.Drawing.Size(800, 22);
            this.ss.TabIndex = 2;
            this.ss.Text = "statusStrip1";
            // 
            // ts
            // 
            this.ts.Location = new System.Drawing.Point(0, 24);
            this.ts.Name = "ts";
            this.ts.Size = new System.Drawing.Size(800, 25);
            this.ts.TabIndex = 3;
            this.ts.Text = "toolStrip1";
            // 
            // mNew
            // 
            this.mNew.Name = "mNew";
            this.mNew.Size = new System.Drawing.Size(180, 22);
            this.mNew.Text = "Новая труба";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 6);
            // 
            // FRMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ts);
            this.Controls.Add(this.ss);
            this.Controls.Add(this.mm);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mm;
            this.Name = "FRMain";
            this.Text = "Просмотровщик труб";
            this.mm.ResumeLayout(false);
            this.mm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mm;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuLoad;
        private System.Windows.Forms.ToolStripMenuItem menuLoadDump;
        private System.Windows.Forms.ToolStripMenuItem menuLoadCSV;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAsDump;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAsCsv;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.StatusStrip ss;
        private System.Windows.Forms.ToolStrip ts;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuViewAllSensors;
        private System.Windows.Forms.ToolStripMenuItem menuWindows;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mNew;
    }
}

