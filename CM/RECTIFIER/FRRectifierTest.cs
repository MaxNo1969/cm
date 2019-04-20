using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public FRRectifierTest(RectifierSettings _settings)
        {
            settings = _settings;
            InitializeComponent();
            label1.Text = string.Format("Тип контроля:{0}", settings.TpIU);
            label2.Text = string.Format("Длительность работы, с:{0}", settings.Timeout);
            label3.Text = string.Format("Требуемый ток, А:{0}", settings.NominalI);
            label4.Text = string.Format("Требуемое напряжение, В:{0}", settings.NominalU);
            label5.Text = string.Format("Максимальный ток, А:{0}", settings.MaxI);
            label6.Text = string.Format("Максимальное напряжение, В:{0}", settings.MaxU);
            label7.Text = string.Format("Сопротивления перегрева, Ом:{0}", settings.MaxR);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //label8.Text = string.Format("Время:{0}",Program.rectifier.getCurrentProcessSeconds());
            //label9.Text = string.Format("Ток:{0}",Program.rectifier.getAmperage());
            //label10.Text = string.Format("Напряжение:{0}",Program.rectifier.getVoltage());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Program.rectifier.Start();
            Program.rectifier.modbus.setSingleRegister(1, 61, 1);
            Thread.Sleep(200);
            Program.rectifier.modbus.ReadInputRegisterE(1, 1, out ushort res);
            label8.Text = string.Format("{0}", res);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Program.rectifier.Stop();
            Program.rectifier.modbus.setSingleRegister(1, 61, 0);
        }
    }
}
