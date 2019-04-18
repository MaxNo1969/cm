using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// UserControl для отображения одного обрабатываемого сигнала с PCIE-1720
    /// </summary>
    public partial class UCSignal : UserControl
    {
        private SignalIn sIn=null;
        private SignalOut sOut=null;
        bool input=false;
        Color color_false;
        /// <summary>
        /// Конструктор. Входящий сигнал
        /// </summary>
        /// <param name="_sIn">Входящий сигнал</param>
        public UCSignal(SignalIn _sIn)
        {
            InitializeComponent();
            sIn=_sIn;
            input = true;
        }
        /// <summary>
        /// Конструктор. Исходящий сигнал
        /// </summary>
        /// <param name="_sOut">Исходящий сигнал</param>
        public UCSignal(SignalOut _sOut)
        {
            InitializeComponent();
            sOut = _sOut;
            input = false;
        }
        private void UCSignal_Load(object sender, EventArgs e)
        {
            if(input)
            {
                label1.Text = sIn.Position+" "+sIn.Name;
                TT.SetToolTip(this,sIn.Hint);
                TT.SetToolTip(label1,sIn.Hint);
            }
            else
            {
                label1.Text = sOut.Position + " " + sOut.Name;
                TT.SetToolTip(this,sOut.Hint);
                TT.SetToolTip(label1,sOut.Hint);
            }
            label1.Top = (ClientSize.Height - label1.Height) / 2;
            if (label1.Top < 0)
                label1.Top = 0;
            label1.Left = 2;
            color_false = BackColor;
            TT.Active = false;
            TT.Active = true;
        }
        /// <summary>
        /// Поменять значение сигнала
        /// </summary>
        public void Exec()
        {
            if (input)
            {
                if (sIn.Val)
                    BackColor = Color.Green;
                else
                    BackColor = color_false;
            }
            else
            {
                if (sOut.Val)
                    BackColor = Color.Red;
                else
                    BackColor = color_false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //Пока сделаем примитивную проверку на наличие файла
            bool bSce = System.IO.File.Exists("signal control enabled");
            if (!input && (bSce || Program.cmdLineArgs.ContainsKey("NOA1730")))
            {
                //bool b = sOut.Val;
                //if (b == true) sOut.Val = false;
                //if (b == false) sOut.Val = true;
                sOut.Val = !sOut.Val;
                Exec();                
            }
            TT.Active = false;
            TT.Active = true;
        }
    }
}
