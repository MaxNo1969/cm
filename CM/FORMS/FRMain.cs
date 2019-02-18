using FPS;
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
        /// <summary>
        /// Форма для отображения протокола
        /// </summary>
        private FRProt pr;

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

        Tube tube;
        PhysTube ptube;

        /// <summary>
        /// Конструктор главной формы приложения. 
        /// </summary>
        public FRMain()
        {
            Thread.CurrentThread.Name = "MainWindow";
            InitializeComponent();
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            TypeSize ts = new TypeSize("СБТ 73 01")
            {
                sensors = new SensorPars(1, 4, 8, 16),
            };
            tube = new Tube(ts,DefaultValues.tubeLen);
            ptube = new PhysTube(tube);
        }

        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            long usedMem = GC.GetTotalMemory(false);
            ssMain.Items["Heap"].Text = string.Format("{0,6}M", usedMem / (1024 * 1024));
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
            //Запускаем таймер обновления
            timerUpdateUI.Start();
        }

        private void protocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            protocolToolStripMenuItem.Checked = !protocolToolStripMenuItem.Checked;
            pr.Visible = protocolToolStripMenuItem.Checked;
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
            if (tube == null)
            {
                MessageBox.Show("Не загружена труба...");
                return;
            }
            FRAllSensorsView allSensorsView = new FRAllSensorsView(this, tube);
            allSensorsView.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "bin",
                AddExtension = true,
                Filter = "Файлы bin (*.bin)|*.bin|Все файлы (*.*)|*.*",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Tube.load(out tube, ofd.FileName))
                {
                    ptube = new PhysTube(tube);
                    viewTubeToolStripMenuItem_Click(this, null);
                }
                else
                {
                    MessageBox.Show(string.Format("Не удалось загрузить трубу из файла {0}", ofd.FileName), "Ошибка");
                }
            }
        }

        private void viewTubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tube == null)
            {
                MessageBox.Show("Не загружена труба...");
                return;
            }
            FRTubeView tubeView = new FRTubeView(ptube, this);
            tubeView.Show();
        }

        private void importDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "bin",
                AddExtension = true,
                Filter = "Файлы дампа (*.dbl)|*.dbl|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DumpReader reader = new DumpReader(ofd.FileName);
                    IDataWriter<double> writer = tube;
                    writer.Write(reader.Read());
                    ptube = new PhysTube(tube);
                    viewTubeToolStripMenuItem_Click(this, null);
                }
                catch (Exception ex)
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", ex.Message);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr, "Error");
                        MessageBox.Show(msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    #endregion
                }
            }
        }
    }
}
