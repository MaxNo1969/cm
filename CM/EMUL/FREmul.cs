using FormsExtras;
using Protocol;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Форма для эмуляции установки. Включает WorkerThread для эмуляции движения трубы в установке
    /// </summary>
    public partial class FREmul : MyMDIForm
    {
        /// <summary>
        /// Модель трубы  
        /// </summary>
        Tube tm;
        LCard lc;
        /// <summary>
        /// Поток для эмуляции движения трубы
        /// </summary>
        TubeMoveThread tMover;
        ///// <summary>
        ///// Поток для записи данных из виртуальной трубы в буфер эмулятора АЦП
        ///// </summary>
        //WriteDataThread wdt; 
        /// <summary>
        /// Сигналы для эмуляции PCIE1730
        /// </summary>
        SignalListDef sl;
        /// <summary>
        /// Worker - эмуляция движения трубы через установку
        /// </summary>
        BackgroundWorker worker;

        DateTime tubeStartTime;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_lcard">Карта АЦП(для эмулятора виртуальная)</param>
        /// <param name="_tube">Модель трубы</param>
        public FREmul(LCard _lcard,Tube _tube)
        {
            tm = _tube ?? throw new ArgumentException("Не задан параметр", "_tube");
            if (_lcard == null)
            {
                throw new ArgumentException("Не задан параметр", "_lcard");
            }
            if (!(_lcard is LCardVirtual))
            {
                throw new ArgumentException("Эмулятор работает только с виртуальной L502", "_lcard");
            }
            lc = _lcard;
            (lc as LCardVirtual).srcTube = tm;
            InitializeComponent();
            // Настройка ProgressBar-а
            pbTube.Minimum = 0;
            pbTube.Maximum = 100;
            tMover = new TubeMoveThread(tm.ptube);
            sl = Program.signals;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            for (int i = 0; i < sl.CountIn; i++)
            {
                SignalIn s = sl.GetSignalIn(i);
                inputSignals.Items.Add(string.Format("{0} {1}", s.Position, s.Name), s.Val);
            }
            for (int i = 0; i < sl.CountOut; i++)
            {
                SignalOut s = sl.GetSignalOut(i);
                outputSignals.Items.Add(string.Format("{0} {1}", s.Position, s.Name), s.Val);
            }
            timer.Start();
            //Подготавливаем BackgroundWorker
            worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            #region Логирование
            {
                string msg = "";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion 
            tMover.stop();
            //Снимаем все сигналы 
            sl.ClearAllInputSignals();
            pbTube.Value = 0;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                //lb.Items.Add(e.UserState as string);
            }
            else if (e.ProgressPercentage <= 100)
            {
                pbTube.Value=e.ProgressPercentage;
                lblPos.Text=string.Format("{0,5:f2} м",tm.ptube.l2px(tm.ptube.startReadX)/1000.0);
                //Проверяем открыта ли модель трубы
                if (btnTubeModel.Enabled == false)
                {
                    //frtubemodel.ucTube.winStart = tm.curPosX;
                    //frtubemodel.ucTube.Invalidate();
                }
            }
            //Добавляем в лист-бокс и обновляем метку
            else if(e.ProgressPercentage==101)
            {
                lblInfo.Text = e.UserState as string;
                lb.Items.Add(e.UserState as string);
                lb.SelectedIndex = lb.Items.Count - 1;
                lb.SelectedIndex = -1;
            }
            //Просто обновляем метку
            else if (e.ProgressPercentage == 102)
            {
                lblInfo.Text = e.UserState as string;
            }
        }

        static long startWait;
        /// <summary>
        /// Задержка в цикле ожидания сигнала
        /// </summary>
        const int signalWaitCycleTime = 1000;
        const int updateCountersPeriod = 1000;
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Логирование
            {
                string msg = "";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr,"Message");
            }
            #endregion 
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Сигнал должен быть включен всегда
            //worker.ReportProgress(101, "Включаем сигнал \"Цепи управления\"");
            //sl.set(sl.iCC, true);
            //worker.ReportProgress(101, "Включаем сигнал \"Цикл 3\"");
            //sl.set(sl.iCYC, true);
            //Тут сделаем цикл по трубам
            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                //Ждем выставления сигнала "Перекладка"
                //worker.ReportProgress(102, "Ждем выставления сигнала \"ПЕРЕКЛАДКА\"...");
                //while (sl.oGLOBRES.Val == false)
                //{
                //    //Проверяем кнопку СТОП
                //    if (worker.CancellationPending)
                //    {
                //        e.Cancel = true;
                //        return;
                //    }
                //    Thread.Sleep(signalWaitCycleTime);
                //}
                worker.ReportProgress(101, "Выставляем сигнал \"Готовность 3\"...");
                sl.set(sl.iREADY, true);
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                worker.ReportProgress(101, "Выставляем сигнал \"Соленоид\"...");
                sl.set(sl.iSOL, true);
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                //Ожидаем сигнал "Работа 3"
                //worker.ReportProgress(101, "Начинаем движение трубы...");
                ////Снимаем сигнал "Готовность 3" (начинается движение трубы)
                //worker.ReportProgress(101, "Снимаем сигнал \"ГОТОВНОСТЬ3\"...");
                //sl.set(sl.iREADY, false);
                tubeStartTime = DateTime.Now;
                //Ждем пока труба доедет до входа в модуль (~30 сек.)
                //startWait = sw.ElapsedMilliseconds;
                //while (sw.ElapsedMilliseconds - startWait < 1000)
                //{
                //    //Проверяем кнопку СТОП
                //    if (worker.CancellationPending)
                //    {
                //        e.Cancel = true;
                //        return;
                //    }
                //    Thread.Sleep(signalWaitCycleTime);
                //}
                //Запускаем движение трубы
                tMover.start();
                //Труба на входе в модуль мнк3
                worker.ReportProgress(101, "Труба на входе в модуль МНК3...");
                worker.ReportProgress(101, "Выставляем сигнал \"КОНТРОЛЬ3\"(труба на входе МНК3)...");
                sl.set(sl.iCNTR, true);
                //Ждем пока труба проедет до конца
                startWait = sw.ElapsedMilliseconds;
                while (true)
                {
                    //Проверяем кнопку СТОП
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if(sw.ElapsedMilliseconds-startWait>updateCountersPeriod)
                    {
                        worker.ReportProgress(tm.ptube.startReadX * 100 / tm.ptube.Width,0);
                        startWait = sw.ElapsedMilliseconds;
                    }
                    if (tMover.bEndOfTube)
                    {
                        #region Логирование 
                        {
                            string msg = string.Format("Окончание трубы: {0}", tm.ptube.startReadX);
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Info");
                        }
                        #endregion
                        break;
                    }
                    Thread.Sleep(signalWaitCycleTime);
                }
                worker.ReportProgress(tm.ptube.startReadX * 100 / tm.ptube.Width, 0);
                worker.ReportProgress(101, "Труба вышла из МНК3...");
                worker.ReportProgress(101, "Снимаем сигнал \"КОНТРОЛЬ3\"...");
                sl.set(sl.iCNTR, false);
                worker.ReportProgress(101, "Снимаем сигнал \"СОЛЕНОИД\"...");
                sl.set(sl.iSOL, false);
                tMover.stop();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                for (int i = 0; i < outputSignals.Items.Count; i++)
                {
                    SignalOut s = sl.GetSignalOut(i);
                    outputSignals.SetItemChecked(i, s.Val);
                }
                for (int i = 0; i < inputSignals.Items.Count; i++)
                {
                    SignalIn s = sl.GetSignalIn(i);
                    inputSignals.SetItemChecked(i, s.Val);
                }
            }
            lblTimer.Text= string.Format(@"{0:hh\:mm\:ss}", DateTime.Now - tubeStartTime);
        }

        private void signals_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            if (clb.Name == "inputSignals")
            {
                //SignalIn s = sl.GetSignalIn(e.Index);
                //s.Val = e.NewValue == CheckState.Checked;
                sl.set(sl.GetSignalIn(e.Index), e.NewValue == CheckState.Checked);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lb.Items.Clear();
            worker.RunWorkerAsync();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            if(worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        private void FREmul_Load(object sender, EventArgs e)
        {
        }
        private void FREmul_FormClosing(object sender, FormClosingEventArgs e)
        {

            tMover.stop();
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }
        //FRTubeModel frtubemodel;
        private void btnTubeModel_Click(object sender, EventArgs e)
        {
            btnTubeModel.Enabled = false;
            FRTubeView frm = new FRTubeView(tm,(FRMain)MdiParent)
            {
                Text = "Труба(модель)",
                editable = true,
            };
            frm.FormClosed += new FormClosedEventHandler((object ob, FormClosedEventArgs ea) => { btnTubeModel.Enabled = true; });
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }
    }
}
