using System;
using System.Windows.Forms;

namespace FormsExtras
{
    /// <summary>
    /// Форма от которой должны наследоваться все дочерние формы в приложении
    /// </summary>
    public partial class MyMDIForm : FormSp
    {
        /// <summary>
        /// Признак того, что может быть открыто только одно такое окно
        /// </summary>
        public bool single = true;
        /// <summary>
        /// Меню для вызова этой формы
        /// </summary>
        public ToolStripMenuItem parentMenu = null;
        /// <summary>
        /// Кнопка для вызова этой формы
        /// </summary>
        public ToolStripButton parentButton = null;
        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public MyMDIForm()
        {
            Load += MNMDIForm_Load;
            FormClosing += MNMDIForm_FormClosing;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_mdi"></param>
        /// <param name="_menu"></param>
        /// <param name="_btn"></param>
        public MyMDIForm(Form _mdi, ToolStripMenuItem _menu = null, ToolStripButton _btn = null)
        {
            MdiParent = _mdi;
            parentMenu = _menu;
            parentButton = _btn;
            Load += MNMDIForm_Load;
            FormClosing += MNMDIForm_FormClosing;
        }

        private void MNMDIForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (single)
            {
                if (parentMenu != null) parentMenu.Enabled = true;
                if (parentButton != null) parentButton.Enabled = true;
            }
        }

        private void MNMDIForm_Load(object sender, EventArgs e)
        {
            if (single)
            {
                if (parentMenu != null) parentMenu.Enabled = false;
                if (parentButton != null) parentButton.Enabled = false;
            }
        }
    }
}
