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
        LCardVirtual lc;
        /// <summary>
        /// Поток для эмуляции движения трубы
        /// </summary>
        //TubeMoveThread tMover;
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

        FRMain fRMain;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_lcard">Карта АЦП(для эмулятора виртуальная)</param>
        /// <param name="_tube">Модель трубы</param>
        public FREmul(LCardVirtual _lcard,Tube _tube, FRMain _frMain):base(_frMain)
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
            fRMain = _frMain;
            InitializeComponent();
            lc = _lcard;
            lc.srcTube = tm;
            sl = Program.signals;
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
            //Снимаем все сигналы 
            //sl.ClearAllInputSignals();
            sl.set(sl.iREADY, false);
            sl.set(sl.iCNTR, false);
            sl.set(sl.iSTRB, false);
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
                lblTimer.Text = string.Format("{0:hh\\:mm\\:ss}", DateTime.Now - tubeStartTime);
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
            }
            //Просто обновляем метку
            else if (e.ProgressPercentage == 102)
            {
                lblInfo.Text = e.UserState as string;
            }
        }

        DateTime startWait;
        /// <summary>
        /// Задержка в цикле ожидания сигнала
        /// </summary>
        const int signalWaitCycleTime = 200;
        const int updateCountersPeriod = 200;
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
            //Тут сделаем цикл по трубам
            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                worker.ReportProgress(101, "Выставляем сигнал \"ГОТОВНОСТЬ\"...");
                sl.set(sl.iREADY, true);
                worker.ReportProgress(101, "Выставляем сигнал \"СОЛЕНОИД\"...");
                sl.set(sl.iSOL, true);
                worker.ReportProgress(101, "Выставляем сигнал \"ЦИКЛ\"...");
                sl.set(sl.iCYC, true);
                //Ожидаем сигнал "РАБОТА"
                //startWait = DateTime.Now;
                while (!sl.get(sl.oWRK))
                {
                    //Проверяем кнопку СТОП
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    Thread.Sleep(signalWaitCycleTime);
                }
                worker.ReportProgress(101, "Получен сигнал \"РАБОТА\"...");
                tubeStartTime = DateTime.Now;
                //Труба на входе в модуль мнк3
                worker.ReportProgress(101, "Труба на входе в модуль МНК3...");
                worker.ReportProgress(101, "Выставляем сигнал \"КОНТРОЛЬ\"(труба на входе МНК3)...");
                sl.set(sl.iCNTR, true);
                //Ждем пока труба проедет до конца
                startWait = DateTime.Now;
                while (true)
                {
                    //Проверяем кнопку СТОП
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if((DateTime.Now-startWait).TotalMilliseconds>updateCountersPeriod)
                    {
                        worker.ReportProgress(tm.ptube.startReadX * 100 / tm.ptube.Width,0);
                        startWait = DateTime.Now;
                    }
                    ////Закончились данные для эмуляции
                    //if(lc.index>tm.rawDataSize)
                    //{
                    //    #region Логирование 
                    //    {
                    //        string msg = "Закончились данные для эмуляции";
                    //        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    //        Log.add(logstr, LogRecord.LogReason.info);
                    //        Debug.WriteLine(logstr, "Message");
                    //    }
                    //    #endregion
                    //    break;
                    //}
                    if (!lc.IsRunning)
                    {
                        #region Логирование 
                        {
                            string msg = string.Format("Закончились данные в модели. Считано {0}",lc.srcTube.rawDataSize  );
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
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
                Thread.Sleep(signalWaitCycleTime);
                if(fRMain.breakToView)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
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
            // Настройка ProgressBar-а
            pbTube.Minimum = 0;
            pbTube.Maximum = 100;
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
            //Подготавливаем BackgroundWorker
            worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            worker.ReportProgress(101, "Выставляем сигнал \"ЦИКЛ\"...");
            sl.set(sl.iCYC, true);
            worker.ReportProgress(101, "Выставляем сигнал \"СОЛЕНОИД\"...");
            sl.set(sl.iSOL, true);
        }

        private void FREmul_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }
        //FRTubeModel frtubemodel;
        private void btnTubeModel_Click(object sender, EventArgs e)
        {
            //btnTubeModel.Enabled = false;
            //FRTubeView frm = new FRTubeView(tm,(FRMain)MdiParent)
            //{
            //    Text = "Труба(модель)",
            //    editable = true,
            //};
            //frm.FormClosed += new FormClosedEventHandler((object ob, FormClosedEventArgs ea) => { btnTubeModel.Enabled = true; });
            //frm.MdiParent = this.MdiParent;
            //frm.Show();
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "dbl",
                AddExtension = true,
                Filter = "Файлы дампа (*.dbl)|*.dbl|Все файлы (*.*)|*.*",
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DumpReader reader = new DumpReader(ofd.FileName);
                    IDataWriter<double> writer = tm;
                    tm.reset();
                    writer.Write(reader.Read());
                    Program.tube.raw2phys(0, Program.tube.sections, 0, Program.tube.ptube.Width / Program.tube.ptube.logZoneSize);
                    FRAllSensorsView frm = new FRAllSensorsView(fRMain, tm)
                    {
                        Text = string.Format("Труба - модель (Дамп:{0})", ofd.FileName),
                    };
                    frm.Show();
                }
                catch (Exception ex)
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", ex.Message);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr, "Error");
                        MessageBox.Show(msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    #endregion
                }
            }

        }
    }
}
