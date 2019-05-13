using FormsExtras;
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
        readonly BackgroundWorker worker;

        public FRRectifier(Rectifier _rectifier)
        {
            rectifier = _rectifier;
            InitializeComponent();
            worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 102)
            {
                double[] iu = (double[])e.UserState;
                lblI.Text = string.Format("I:{0,4:F1}В", iu[0]);
                lblU.Text = string.Format("U:{0,4:F1}А", iu[1]);
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Логирование 
            {
                string msg = "Начали контролировать блок питания";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            while (true)
            {
                if(worker.CancellationPending)
                {
                    #region Логирование 
                    {
                        string msg = "Закончили контролировать блок питания";
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    e.Cancel = true;
                    return;
                }
                if (rectifier.IsWork())
                {
                    double[] iu  = new double[] { rectifier.getVoltage(), rectifier.getAmperage() };
                    worker.ReportProgress(102, iu);
                }
                else
                {
                    double[] iu = new double[] { rectifier.getVoltage(), rectifier.getAmperage() };
                    worker.ReportProgress(102, iu);
                }
                Thread.Sleep(Program.settings.rectifierSettings.Period);
            }
        }

        private void FRRectifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    Visible = false;
                    onHide?.Invoke();
                    break;
            }
        }

        private void FRRectifier_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }
    }
}
