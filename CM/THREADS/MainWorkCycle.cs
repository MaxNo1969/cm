using Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CM
{
    class MainWorkCycle : BackgroundWorker
    {
        readonly FRMain frMain;
        readonly Tube tube;
        WorkThread workThread = null;
        public MainWorkCycle(Tube _tube, FRMain _frMain):base()
        {
            #region Логирование 
            {
                string msg = string.Format("{0}", this.ToString() );
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            tube = _tube;
            frMain = _frMain;
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            ProgressChanged += MainWorkCycle_ProgressChanged;
            RunWorkerCompleted += MainWorkCycle_RunWorkerCompleted;
            DoWork += MainWorkCycle_DoWork;
        }

        private void MainWorkCycle_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1} - Enter", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            while(true)
            {
                //В цикле сперва ждем готовность и соленоид

                //А при появлении ждем контроль
                //Выставляем сигнал "Перекладка" - установка готова к следующему рабочему циклу
                Program.signals.oGLOBRES.Val = true;
                if (CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                workThread = new WorkThread(tube, frMain);
                workThread.start();
                while (workThread.isRunning) Thread.Sleep(100);
                ReportProgress(10);
                break;
            }
            workThread.stop();
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1} - Exit", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
        }

        private void MainWorkCycle_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
        }

        private void MainWorkCycle_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
        }
    }
}
