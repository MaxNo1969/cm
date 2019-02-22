using System;
using System.Windows.Forms;

namespace FPS
{
    public class FormSp : Form
    {
        public FormSp()
        {
            Load += SPForm_Load;
            FormClosing += SPForm_FormClosing;
        }

        private void SPForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DesignMode)
                FormPosSaver.save(this);
        }

        private void SPForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                FormPosSaver.load(this);
        }
    }
}
