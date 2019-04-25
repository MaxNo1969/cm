using Protocol;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Форма для тестирования платы формирования тактирования АЦП
    /// </summary>
    public partial class FRModuleTADC : Form
    {
        MTADC mtadc;
        /// <summary>
        /// Конструктор
        /// </summary>
        public FRModuleTADC()
        {
            InitializeComponent();
            mtadc = Program.mtdadc;
        }


        private void FRTestADCController_FormClosed(object sender, FormClosedEventArgs e)
        {
            //mtadc.Dispose();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int n = Convert.ToInt32(input.Text);
                output.Text += string.Format("(->)!{0} (<-){1}", n, mtadc.cmd(n));
                output.Text += System.Environment.NewLine;
                input.Text = string.Empty;
            }
            catch (Exception ex)
            {
                output.Text += string.Format("(->)!{0} (Err){1}", input.Text, ex.Message);
                output.Text += System.Environment.NewLine;
                input.Text = string.Empty;
            }
        }
    }
}
