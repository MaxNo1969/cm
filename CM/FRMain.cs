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
    public interface ISBUpdateAvailable
    {
        /// <summary>
        /// Обновление статусбара из других потоков
        /// </summary>
        /// <param name="_sbItem">Имя поля в статусбаре</param>
        /// <param name="_sbText">Выводимая строка</param>
        void setSb(string _sbItem, string _sbText);
    }
    public partial class FRMain : Form,ISBUpdateAvailable
    {
        ///Подчиненные формы
        /// <summary>
        /// Форма для отображения сигналов с 1730
        /// </summary>
        private FRSignals fSignals;

        /// <summary>
        /// Форма для отображения протокола
        /// </summary>
        private FRProt pr;

        /// <summary>
        /// Форма для отображения выпрямителя
        /// </summary>
        private FRRectifier fRectifier;

        private FRADCData frADC = null;

        private FRTubeView tubeView = null;

        BackgroundWorker mainWorkCicle;

        #region Доступ к статусу из других потоков
        /// <summary>
        /// Вызов Invoke для обновления статусбара из другого потока
        /// </summary>
        /// <param name="_sbItem">Имя поля в статусбаре</param>
        /// <param name="_sbText">Выводимая строка</param>
        public void setSb(string _sbItem, string _sbText)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => { ssMain.Items[_sbItem].Text = _sbText; }));
            }
            else
            {
                ssMain.Items[_sbItem].Text = _sbText;
            }
        }
        #endregion

        /// <summary>
        /// Конструктор главной формы приложения. 
        /// </summary>
        public FRMain()
        {
            #region Логирование 
            {
                string msg = string.Format("{0}", @"Конструктор");
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            Thread.CurrentThread.Name = "MainWindow";
            InitializeComponent();
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            mainWorkCicle = new MainWorkCycle(Program.tube, this);
        }

        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            long usedMem = GC.GetTotalMemory(false);
            setSb("Heap", string.Format("{0,6}M", usedMem / (1024 * 1024)));
        }

        private void FRMain_Load(object sender, EventArgs e)
        {
            //Настраиваем протокол
            // Окно протокола создаем сразу оно будет существовать 
            // всё время работы программы
            pr = new FRProt()
            {
                MdiParent = this,
                Dock = DockStyle.Bottom,
                saveMethod = FRProt.SaveMethod._tofile,
            };
            //Тут можно вставить обработчик закрытия формы
            pr.onHide += new FRProt.OnHideForm(() => { protocolToolStripMenuItem.Checked = false; });
            pr.Visible = FormPosSaver.visible(pr);
            protocolToolStripMenuItem.Checked = pr.Visible;

            //Настраиваем окно сигналов
            // Окно создаем сразу оно будет существовать 
            // всё время работы программы
            fSignals = new FRSignals(Program.signals)
            {
                MdiParent = this,
            };
            fSignals.onHide += new FRSignals.OnHideForm(() => { viewSignalsToolStripMenuItem.Checked = false; });
            fSignals.Visible = FormPosSaver.visible(fSignals);
            viewSignalsToolStripMenuItem.Checked = fSignals.Visible;

            //Настраиваем окно сигналов
            // Окно создаем сразу оно будет существовать 
            // всё время работы программы
            fRectifier = new FRRectifier(Program.rectifier)
            {
                MdiParent = this,
            };
            fRectifier.onHide += new FRRectifier.OnHideForm(() => { rectifierToolStripMenuItem.Checked = false; });
            fRectifier.Visible = FormPosSaver.visible(fRectifier);
            rectifierToolStripMenuItem.Checked = fRectifier.Visible;

            foreach (TypeSize ts in Program.settings.TypeSizes)
            {
                cbTypeSize.Items.Add(ts);
                if (ts.Name == Program.settings.Current.Name) cbTypeSize.SelectedItem = ts;
            }


            if (Program.cmdLineArgs.ContainsKey("NOA1730"))
            {
                emulToolStripMenuItem.Visible = true;
                Program.signals.set(Program.signals.iCC, true);
            }
            else
            {
                emulToolStripMenuItem.Visible = false;
            }

            //Запускаем таймер обновления
            timerUpdateUI.Start();
        }
        #region Показ/скрытие окон
        /// <summary>
        /// Протокол
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void protocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            protocolToolStripMenuItem.Checked = !protocolToolStripMenuItem.Checked;
            pr.Visible = protocolToolStripMenuItem.Checked;
        }
        /// <summary>
        /// Сигналы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewSignalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewSignalsToolStripMenuItem.Checked = !viewSignalsToolStripMenuItem.Checked;
            fSignals.Visible = viewSignalsToolStripMenuItem.Checked;
        }
        /// <summary>
        /// Блок питания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rectifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rectifierToolStripMenuItem.Checked = !rectifierToolStripMenuItem.Checked;
            fRectifier.Visible = rectifierToolStripMenuItem.Checked;
        }
        #endregion Показ/скрытие окон
        /// <summary>
        /// АЦП
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewADCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frADC = new FRADCData(Program.lCard)
            {
                MdiParent = this,
                parentMenu = viewADCToolStripMenuItem,
            };
            frADC.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FRMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            pr.Dispose();
        }

        private void viewAllSensorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.tube == null)
            {
                MessageBox.Show("Не загружена труба...");
                return;
            }
            FRAllSensorsView allSensorsView = new FRAllSensorsView(this, Program.tube);
            allSensorsView.Show();
        }

        private bool loadTube(ref Tube _tube)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "bin",
                AddExtension = true,
                Filter = "Трубы (*.bin)|*.bin|Все файлы (*.*)|*.*",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Tube.load(ref _tube, ofd.FileName))
                {
                    #region Логирование 
                    {
                        string msg = string.Format("Загружена труба из файла {0}", ofd.FileName);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    //_tube.ptube = new PhysTube(Program.tube);
                    return true;
                }
                else
                {
                    #region Логирование 
                    {
                        string msg = string.Format("Не удалось загрузить трубу из файла {0}", ofd.FileName);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr, "Error");
                    }
                    #endregion
                    _tube = null;
                    return false;
                }
            }
            _tube = null;
            return false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadTube(ref Program.tube))
            {
                if (Program.tube.rtube.ts.Name != Program.settings.Current.Name)
                {
                    MessageBox.Show(string.Format("Изменился типоразмер {0} => {1}",
                        Program.settings.Current.Name, Program.tube.rtube.ts.Name));
                    Program.settings.TypeSizes.Current = Program.tube.rtube.ts;
                    Program.settings.Current = Program.tube.rtube.ts;
                }
                viewTubeToolStripMenuItem_Click(this, null);
            }
            else
            {
                MessageBox.Show(string.Format("Не удалось загрузить трубу."), "Ошибка");
            }
        }

        private void viewTubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewTube();
        }

        public void viewTube()
        {
            if (Program.tube == null)
            {
                MessageBox.Show("Не загружена труба...");
                return;
            }
            tubeView = new FRTubeView(Program.tube, this)
            {
                MdiParent = this,
                parentMenu = viewTubeToolStripMenuItem,
            };
            tubeView.Show();

        }

        private void importDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "dbl",
                AddExtension = true,
                Filter = "Файлы дампа (*.dbl)|*.dbl|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DumpReader reader = new DumpReader(ofd.FileName);
                    IDataWriter<double> writer = Program.tube;
                    writer.Write(reader.Read());
                    Program.tube.raw2phys(0, Program.tube.sections, 0, Program.tube.ptube.Width / Program.tube.ptube.logZoneSize);
                    //viewTubeToolStripMenuItem_Click(this, null);
                    viewAllSensorsToolStripMenuItem_Click(this, null);
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

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRSettings frSettings = new FRSettings(ref Program.settings)
            {
                MdiParent = this,
                parentMenu = optionsToolStripMenuItem,
            };
            frSettings.Show();
        }

        private void testADCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.lCard is LCardVirtual)
            {
                Tube emulTube = new Tube(Program.settings.Current, Program.settings.TubeLen);
                if (loadTube(ref emulTube))
                {
                    LCardVirtual lCardVirtual = (LCardVirtual)Program.lCard;
                    lCardVirtual.srcTube = emulTube;
                }
                else
                    return;
            }
            FRTestL502 frm = new FRTestL502(this)
            {
                MdiParent = this,
                parentMenu = testADCToolStripMenuItem,
            };
            frm.Show();
        }

        private void emulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FREmul frm = new FREmul(Program.lCard as LCardVirtual, new Tube(Program.settings.Current, Program.settings.TubeLen),this)
            {
                parentMenu = emulToolStripMenuItem,
                MdiParent = this,
            };
            frm.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "bin",
                AddExtension = true,
                Filter = "Трубы (*.bin)|*.bin|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if(Tube.save(Program.tube,sfd.FileName))
                    {
                        #region Логирование 
                        {
                            string msg = string.Format("Записан файл {0}", sfd.FileName);
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
                        }
                        #endregion
                    }
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

        private void startstopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Логирование 
            {
                string msg = string.Format("{0}", startstopToolStripMenuItem.Text );
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            if (startstopToolStripMenuItem.Text == "&Старт")
            {
                startstopToolStripMenuItem.Text = "&Стоп";
                mainWorkCicle.RunWorkerAsync();
            }
            else
            {
                startstopToolStripMenuItem.Text = "&Старт";
                mainWorkCicle.CancelAsync();
            }
        }

        public bool breakToView { get { return breakToViewToolStripMenuItem.Checked; } }
        private void breakToViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(breakToViewToolStripMenuItem.Checked)
            {
                breakToViewToolStripMenuItem.Checked = false;
            }
            else
            {
                breakToViewToolStripMenuItem.Checked = true;
            }
        }

        private void cbTypeSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.settings.Current = (TypeSize)cbTypeSize.SelectedItem;
            //ToDo - изменения настроек
            Program.settings.onChangeSettings?.Invoke(new object[] { "Typesize", cbTypeSize.SelectedItem });
            Program.settings.changed = true;
        }

        private void модульТактированияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRModuleTADC frm = new FRModuleTADC();
            frm.Show();
        }

        private void rectifierTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRRectifierTest frm = new FRRectifierTest(Program.settings.Current.rectifier);
            frm.Show();
        }

        private void adcRawDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.tube == null)
            {
                MessageBox.Show("Не загружена труба...");
                return;
            }
            FRTubeRawView frm = new FRTubeRawView(Program.tube,this, adcRawDataToolStripMenuItem);
            frm.Show();
        }
    }
}
