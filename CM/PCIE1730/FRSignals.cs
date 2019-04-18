using FormsExtras;
using System;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Форма отображения сигналов платы цифрового ввода/вывода
    /// </summary>
    public partial class FRSignals : MyMDIForm 
    {
        SignalListDef SL;
        /// <summary>
        /// Делегат при скрытии формы
        /// </summary>
        public delegate void OnHideForm();
        /// <summary>
        /// Действия при скрытии формы
        /// </summary>
        public OnHideForm onHide = null;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_SL">Список обрабатываемых сигналов</param>
        public FRSignals(SignalListDef _SL)
        {
            InitializeComponent();
            SL = _SL;
        }

        private void FSignals_Load(object sender, EventArgs e)
        {
            timer1.Interval = 100;/*DBSPar.Int("FSignals Timer Period", 100);*/
            int space = 4;
            int ltop = 0;
            int rleft = 0;
            for (int i = 0; i < SL.CountIn; i++)
            {
                UCSignal p = new UCSignal(SL.GetSignalIn(i));
                Controls.Add(p);
                p.Left = space;
                p.Top = ltop + space;
                ltop += p.Height + space;
                rleft = p.Left + p.Width + space;
            }
            ltop = 0;
            for (int i = 0; i < SL.CountOut; i++)
            {
                UCSignal p = new UCSignal(SL.GetSignalOut(i));
                Controls.Add(p);
                p.Left = rleft;
                p.Top = ltop + space;
                ltop += p.Height + space;
            }
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach(Control p in Controls)
            {
                if (p is UCSignal)
                    ((UCSignal)p).Exec();
            }
        }

        private void FRSignals_FormClosing(object sender, FormClosingEventArgs e)
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
