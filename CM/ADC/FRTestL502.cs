using FormsExtras;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Форма для запуска тестового сбора с АЦП
    /// </summary>
    public partial class FRTestL502 : MyMDIForm, ISBUpdateAvailable
    {

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
                BeginInvoke(new Action(() => { sb.Items[_sbItem].Text = _sbText; }));
            }
            else
            {
                sb.Items[_sbItem].Text = _sbText;
            }
        }
        #endregion

        /// <summary>
        /// Таймер для отсчета времени
        /// </summary>
        Stopwatch sw;
        /// <summary>
        /// Указатель на главную форму приложения
        /// </summary>
        FRMain frMain;
        /// <summary>
        /// Блокировка
        /// </summary>
        private object block = new object();
        /// <summary>
        /// Массивы для передачи данных в картинку с графиком
        /// x  - индекс в массиве
        /// y - значение (нормвлизовано)
        /// </summary>
        double[] x;
        double[] y;
        Dt sens = null;
        //Dt corrSens = null;
        //сколько данных за раз показываем в окне
        int size = 2048;
        ReadDataThread readDataThread;
        //ZoneWriterThread zoneWriter;

        /// <summary>
        /// Обработчик события записи очередной порции данных в трубу (вызывается в потоке readDataThread для класса Tube.LogTube.RawTube)
        /// </summary>
        /// <param name="_data">Данные для вывода в график</param>
        public void lCardDataRead(IEnumerable<double> _data)
        {
            //lock (block)
            if(_data!=null)
            {
                IEnumerator<double> enumerator = _data.GetEnumerator();
                enumerator.Reset();
                int cnt = 0;
                for (int i = 0; i < size && enumerator.MoveNext(); i++)
                {
                    y[i] = enumerator.Current;
                    cnt++;
                }
                //Array.Copy(_data, y, size);
            }
            ucGr.Invalidate();
            setSb("DataSize", string.Format("{0}", Program.tube.rawDataSize));
            setSb("Time",string.Format(@"{0:mm\:ss\.ff}", sw.Elapsed));
            if (sw.ElapsedMilliseconds > 20000) Stop();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_frm">Главная форма приложения</param>
        public FRTestL502(FRMain _frm)
        {
            sw = new Stopwatch();
            frMain = _frm;
            isReadThStarted = false;
            InitializeComponent();
            tbSaveData.Enabled = (Program.tube.rawDataSize > 0);
            ucGr.showGridX = true;
            ucGr.showGridY = true;
            ucGr.minX = 0;
            ucGr.maxX = size;
            //ucGr.stepGridY = 0.01f;
            //ucGr.minY = -0.1f;
            //ucGr.maxY = 0.1f;
            ucGr.minY = -6;
            ucGr.maxY = 6;

            x = new double[size];
            y = new double[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = (double)i;
                y[i] = 0;
            }
            sens = new Dt(Pens.Green, x, y);
            ucGr.addData(sens);
        }

        bool isReadThStarted;
        //ReadDataThread readDataThread = null;
        //MTADC mtadc = null;
        private void btnStartL502_Click(object sender, EventArgs e)
        {
            if (!isReadThStarted)
            {
                tbView.Enabled = false;
                tbSaveData.Enabled = false;
                isReadThStarted = true;
                btnStartL502.Text = "Стоп";
                Program.tube.reset();
                readDataThread = new ReadDataThread(Program.lCard, Program.tube.rtube);
                //zoneWriter = new ZoneWriterThread(Program.tube);
                Start();
            }
            else
            {
                Stop();
                //Снимаем все выходные сигналы кроме питания
                Program.signals.ClearAllSignals();
                tbView.Enabled = (Program.tube.rawDataSize>0);
                tbSaveData.Enabled = (Program.tube.rawDataSize > 0);
                isReadThStarted = false;
                btnStartL502.Text = "Старт";
                sb.Items["Time"].Text = string.Empty;
                frMain.setSb("Info",string.Empty);
            }
        }

        private bool Start()
        {
            //Запускаем таймер
            sw.Reset();
            sw.Start();
            //Включаем поле
            Program.rectifier.Start();
            //Поставим задержку на включение блока питания
            //WaitHelper.Wait(1);
            Program.lCard.Start();
            Program.mtdadc.start();
            readDataThread.Start();
            //zoneWriter.start();
            return true;
        }

        private bool Stop()
        {
            //Останавливаем обсчет зон
            //zoneWriter?.stop();
            //zoneWriter = null;
            //Останавливаем чтение данных с АЦП
            readDataThread?.Stop();
            readDataThread = null;
            //Останавливаем П217
            Program.mtdadc.stop();
            Program.lCard.Stop();
            Program.rectifier.Stop();
            sw.Stop();
            return true;
        }

        private void FRTestL502_Load(object sender, EventArgs e)
        {
            FormPosSaver.load(this);
            Program.lCard.onDataRead += lCardDataRead;
            tbView.Enabled = (Program.tube.rawDataSize > 0);
            tbSaveData.Enabled = (Program.tube.rawDataSize > 0);
            //В режиме эмуляции выставляем сигнал соленод (без него не запустится ReadDataThread)
            if (Program.signals.a1730 is PCIE_1730_virtual)
                Program.signals.set(Program.signals.iSOL, true);
        }

        private void FRTestL502_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            Program.lCard.onDataRead -= lCardDataRead;
        }
        
        private void tbView_Click(object sender, EventArgs e)
        {
            frMain.viewTube();
        }


        private void tbSaveLcard_Click(object sender, EventArgs e)
        {
            Tube tube = Program.tube;
            frMain.setSb("Info", "Запись данных в файл...");
            setSb("Info", "Запись данных в файл...");
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "csv",
                AddExtension = true,
                Filter = "Файлы csv (*.csv)|*.csv|Все файлы (*.*)|*.*",
                OverwritePrompt = true,
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName, false))
                {
                    for (int i = 0; i < tube.rawDataSize; i++)
                    {
                        writer.WriteLine(string.Format("{0}",tube.rtube.data[i]));
                    }
                    writer.Close();
                }
            }
            frMain.setSb("Info", string.Empty);
            setSb("Info", string.Empty);
        }
    }
}
