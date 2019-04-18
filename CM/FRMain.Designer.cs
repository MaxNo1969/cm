namespace CM
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
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.workToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startstopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakToViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewTubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAllSensorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testADCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.модульТактированияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.protocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSignalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewADCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rectifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.Info = new System.Windows.Forms.ToolStripStatusLabel();
            this.Heap = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
            this.ts = new System.Windows.Forms.ToolStrip();
            this.lblTypeSize = new System.Windows.Forms.ToolStripLabel();
            this.cbTypeSize = new System.Windows.Forms.ToolStripComboBox();
            this.lblTubesCounter = new System.Windows.Forms.ToolStripLabel();
            this.rectifierTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.ts.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.workToolStripMenuItem,
            this.tubeToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.testToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.emulToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.MdiWindowListItem = this.windowsToolStripMenuItem;
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(800, 24);
            this.mainMenu.TabIndex = 1;
            // 
            // workToolStripMenuItem
            // 
            this.workToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startstopToolStripMenuItem,
            this.breakToViewToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.workToolStripMenuItem.Name = "workToolStripMenuItem";
            this.workToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.workToolStripMenuItem.Text = "&Работа";
            // 
            // startstopToolStripMenuItem
            // 
            this.startstopToolStripMenuItem.Name = "startstopToolStripMenuItem";
            this.startstopToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.startstopToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.startstopToolStripMenuItem.Text = "&Старт";
            this.startstopToolStripMenuItem.Click += new System.EventHandler(this.startstopToolStripMenuItem_Click);
            // 
            // breakToViewToolStripMenuItem
            // 
            this.breakToViewToolStripMenuItem.Name = "breakToViewToolStripMenuItem";
            this.breakToViewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.breakToViewToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.breakToViewToolStripMenuItem.Text = "&Прерывание на просмотр";
            this.breakToViewToolStripMenuItem.Click += new System.EventHandler(this.breakToViewToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(257, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::CM.Properties.Resources.exitButton;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.exitToolStripMenuItem.Text = "&Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tubeToolStripMenuItem
            // 
            this.tubeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewTubeToolStripMenuItem,
            this.viewAllSensorsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.importToolStripMenuItem});
            this.tubeToolStripMenuItem.Name = "tubeToolStripMenuItem";
            this.tubeToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.tubeToolStripMenuItem.Text = "&Труба";
            // 
            // viewTubeToolStripMenuItem
            // 
            this.viewTubeToolStripMenuItem.Name = "viewTubeToolStripMenuItem";
            this.viewTubeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.viewTubeToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.viewTubeToolStripMenuItem.Text = "&Просмотр";
            this.viewTubeToolStripMenuItem.Click += new System.EventHandler(this.viewTubeToolStripMenuItem_Click);
            // 
            // viewAllSensorsToolStripMenuItem
            // 
            this.viewAllSensorsToolStripMenuItem.Name = "viewAllSensorsToolStripMenuItem";
            this.viewAllSensorsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.viewAllSensorsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.viewAllSensorsToolStripMenuItem.Text = "Просмотр по датчикам";
            this.viewAllSensorsToolStripMenuItem.Click += new System.EventHandler(this.viewAllSensorsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(219, 6);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::CM.Properties.Resources.newButton_Image;
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.newToolStripMenuItem.Text = "&Новая";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::CM.Properties.Resources.openButton_Image;
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.openToolStripMenuItem.Text = "&Открыть";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(219, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::CM.Properties.Resources.saveButton_Image;
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.saveToolStripMenuItem.Text = "&Сохранить";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveDumpToolStripMenuItem,
            this.saveCSVToolStripMenuItem});
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.saveAsToolStripMenuItem.Text = "&Выгрузить...";
            // 
            // saveDumpToolStripMenuItem
            // 
            this.saveDumpToolStripMenuItem.Name = "saveDumpToolStripMenuItem";
            this.saveDumpToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveDumpToolStripMenuItem.Text = "Файл Дампа";
            // 
            // saveCSVToolStripMenuItem
            // 
            this.saveCSVToolStripMenuItem.Name = "saveCSVToolStripMenuItem";
            this.saveCSVToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveCSVToolStripMenuItem.Text = "Файл CSV";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importDumpToolStripMenuItem,
            this.importCSVToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.importToolStripMenuItem.Text = "&Загрузить...";
            // 
            // importDumpToolStripMenuItem
            // 
            this.importDumpToolStripMenuItem.Name = "importDumpToolStripMenuItem";
            this.importDumpToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.importDumpToolStripMenuItem.Text = "Файл дампа";
            this.importDumpToolStripMenuItem.Click += new System.EventHandler(this.importDumpToolStripMenuItem_Click);
            // 
            // importCSVToolStripMenuItem
            // 
            this.importCSVToolStripMenuItem.Name = "importCSVToolStripMenuItem";
            this.importCSVToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.importCSVToolStripMenuItem.Text = "Файл CSV";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.toolsToolStripMenuItem.Text = "&Настройки";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.optionsToolStripMenuItem.Text = "&Настройки";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testADCToolStripMenuItem,
            this.модульТактированияToolStripMenuItem,
            this.rectifierTestToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.testToolStripMenuItem.Text = "Тестирование";
            // 
            // testADCToolStripMenuItem
            // 
            this.testADCToolStripMenuItem.Name = "testADCToolStripMenuItem";
            this.testADCToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.testADCToolStripMenuItem.Text = "Тест АЦП";
            this.testADCToolStripMenuItem.Click += new System.EventHandler(this.testADCToolStripMenuItem_Click);
            // 
            // модульТактированияToolStripMenuItem
            // 
            this.модульТактированияToolStripMenuItem.Name = "модульТактированияToolStripMenuItem";
            this.модульТактированияToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.модульТактированияToolStripMenuItem.Text = "Модуль тактирования";
            this.модульТактированияToolStripMenuItem.Click += new System.EventHandler(this.модульТактированияToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.protocolToolStripMenuItem,
            this.viewSignalsToolStripMenuItem,
            this.viewADCToolStripMenuItem,
            this.rectifierToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.windowsToolStripMenuItem.Text = "&Окна";
            // 
            // protocolToolStripMenuItem
            // 
            this.protocolToolStripMenuItem.Name = "protocolToolStripMenuItem";
            this.protocolToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.protocolToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.protocolToolStripMenuItem.Text = "&Протокол";
            this.protocolToolStripMenuItem.Click += new System.EventHandler(this.protocolToolStripMenuItem_Click);
            // 
            // viewSignalsToolStripMenuItem
            // 
            this.viewSignalsToolStripMenuItem.Name = "viewSignalsToolStripMenuItem";
            this.viewSignalsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.viewSignalsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.viewSignalsToolStripMenuItem.Text = "Сигналы";
            this.viewSignalsToolStripMenuItem.Click += new System.EventHandler(this.viewSignalsToolStripMenuItem_Click);
            // 
            // viewADCToolStripMenuItem
            // 
            this.viewADCToolStripMenuItem.Name = "viewADCToolStripMenuItem";
            this.viewADCToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.viewADCToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.viewADCToolStripMenuItem.Text = "АЦП";
            this.viewADCToolStripMenuItem.Click += new System.EventHandler(this.viewADCToolStripMenuItem_Click);
            // 
            // rectifierToolStripMenuItem
            // 
            this.rectifierToolStripMenuItem.Name = "rectifierToolStripMenuItem";
            this.rectifierToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.rectifierToolStripMenuItem.Text = "Блок питания";
            this.rectifierToolStripMenuItem.Click += new System.EventHandler(this.rectifierToolStripMenuItem_Click);
            // 
            // emulToolStripMenuItem
            // 
            this.emulToolStripMenuItem.Name = "emulToolStripMenuItem";
            this.emulToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.emulToolStripMenuItem.Text = "&Эмуляция";
            this.emulToolStripMenuItem.Click += new System.EventHandler(this.emulToolStripMenuItem_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Info,
            this.Heap});
            this.ssMain.Location = new System.Drawing.Point(0, 426);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(800, 24);
            this.ssMain.TabIndex = 2;
            this.ssMain.Text = "statusStrip1";
            // 
            // Info
            // 
            this.Info.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.Info.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.Info.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(727, 19);
            this.Info.Spring = true;
            this.Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Heap
            // 
            this.Heap.AutoSize = false;
            this.Heap.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.Heap.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.Heap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Heap.Name = "Heap";
            this.Heap.Size = new System.Drawing.Size(58, 19);
            // 
            // timerUpdateUI
            // 
            this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
            // 
            // ts
            // 
            this.ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTypeSize,
            this.cbTypeSize,
            this.lblTubesCounter});
            this.ts.Location = new System.Drawing.Point(0, 24);
            this.ts.Name = "ts";
            this.ts.Size = new System.Drawing.Size(800, 25);
            this.ts.TabIndex = 4;
            this.ts.Text = "toolStrip1";
            // 
            // lblTypeSize
            // 
            this.lblTypeSize.Name = "lblTypeSize";
            this.lblTypeSize.Size = new System.Drawing.Size(75, 22);
            this.lblTypeSize.Text = "Типоразмер";
            // 
            // cbTypeSize
            // 
            this.cbTypeSize.Name = "cbTypeSize";
            this.cbTypeSize.Size = new System.Drawing.Size(121, 25);
            this.cbTypeSize.SelectedIndexChanged += new System.EventHandler(this.cbTypeSize_SelectedIndexChanged);
            // 
            // lblTubesCounter
            // 
            this.lblTubesCounter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTubesCounter.Name = "lblTubesCounter";
            this.lblTubesCounter.Size = new System.Drawing.Size(0, 22);
            // 
            // rectifierTestToolStripMenuItem
            // 
            this.rectifierTestToolStripMenuItem.Name = "rectifierTestToolStripMenuItem";
            this.rectifierTestToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.rectifierTestToolStripMenuItem.Text = "Блок питания";
            this.rectifierTestToolStripMenuItem.Click += new System.EventHandler(this.rectifierTestToolStripMenuItem_Click);
            // 
            // FRMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ts);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.mainMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "FRMain";
            this.Text = "Дефектоскоп";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FRMain_FormClosed);
            this.Load += new System.EventHandler(this.FRMain_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem tubeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel Info;
        private System.Windows.Forms.ToolStripStatusLabel Heap;
        private System.Windows.Forms.Timer timerUpdateUI;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem protocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewADCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testADCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewSignalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emulToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewTubeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAllSensorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem rectifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startstopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem breakToViewToolStripMenuItem;
        private System.Windows.Forms.ToolStrip ts;
        private System.Windows.Forms.ToolStripLabel lblTypeSize;
        private System.Windows.Forms.ToolStripComboBox cbTypeSize;
        private System.Windows.Forms.ToolStripLabel lblTubesCounter;
        private System.Windows.Forms.ToolStripMenuItem модульТактированияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectifierTestToolStripMenuItem;
    }
}

