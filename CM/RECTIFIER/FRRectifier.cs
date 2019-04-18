using FormsExtras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    public partial class FRRectifier : MyMDIForm
    {
        /// <summary>
        /// Делегат при скрытии формы
        /// </summary>
        public delegate void OnHideForm();

        /// <summary>
        /// Действия при скрытии формы
        /// </summary>
        public OnHideForm onHide = null;

        readonly Rectifier rectifier;

        public FRRectifier(Rectifier _rectifier)
        {
            rectifier = _rectifier;
            InitializeComponent();
        }

        private void FRRectifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    Visible = false;
                    //if (onHide != null) onHide();
                    onHide?.Invoke();
                    break;
            }
        }
    }
}
