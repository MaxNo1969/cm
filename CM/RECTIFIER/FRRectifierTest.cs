using Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    public partial class FRRectifierTest : Form
    {
        RectifierSettings settings;
        bool isStarted = false;
        public FRRectifierTest(RectifierSettings _settings)
        {
            settings = _settings;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //timer.Start();
            //WaitHelper.Wait(1000);
            Program.rectifier.Start();
            Program.rectifier.modbus.ReadHoldingRegisterE(1, 61, out ushort _res);
            isStarted = (_res == 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.rectifier.Stop();
            Program.rectifier.modbus.ReadHoldingRegisterE(1, 61, out ushort _res);
            //WaitHelper.Wait(1000);
            //timer.Stop();
            isStarted = (_res == 1);
        }

        private void FRRectifierTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(isStarted)
                Program.rectifier.Stop();
        }
    }
}
