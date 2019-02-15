namespace CM
{
    partial class UCTube
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UCTube
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "UCTube";
            this.Size = new System.Drawing.Size(649, 426);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UCTube_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UCTube_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.UCTube_MouseDoubleClick);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.UCTube_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.UCTube_Resize);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
