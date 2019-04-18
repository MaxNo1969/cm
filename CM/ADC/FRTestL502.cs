using FormsExtras;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        /// <summary>
        /// Обработчик события записи очередной порции данных в трубу (вызывается в потоке readDataThread для класса Tube.LogTube.RawTube)
        /// </summary>
        /// <param name="_data">Данные для вывода в график</param>
        public void tubeDataChanged(IEnumerable<double> _data)
        {
            //lock (block)
            if(_data!=null)
            {
                IEnumerator<double> enumerator = _data.GetEnumerator();
                enumerator.Reset();
                for (int i = 0; i < size && enumerator.MoveNext(); i++)
                {
                    y[i] = enumerator.Current;
                }
                //Array.Copy(_data, y, size);
            }
            ucGr.Invalidate();
            setSb("DataSize", string.Format("{0}", Program.tube.rawDataSize));
            setSb("Time",string.Format(@"{0:mm\:ss\.ff}", sw.Elapsed));
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
            tbSaveData.Enabled = !(Program.tube.rawDataSize > 0);
            ucGr.showGridX = true;
            ucGr.showGridY = true;
            ucGr.minX = 0;
            ucGr.maxX = size;
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
        ReadDataThread readDataThread = null;
        //MTADC mtadc = null;
        private void btnStartL502_Click(object sender, EventArgs e)
        {
            if (!isReadThStarted)
            {
                tbView.Enabled = false;
                tbSaveData.Enabled = false;
                isReadThStarted = true;
                btnStartL502.Text = "Стоп";
                sw.Reset();
                sw.Start();
                Program.tube.reset();
                readDataThread = new ReadDataThread(Program.lCard, Program.tube);
                readDataThread.Start();
            }
            else
            {
                tbView.Enabled = (Program.tube.rawDataSize>0);
                tbSaveData.Enabled = (Program.tube.rawDataSize > 0);
                isReadThStarted = false;
                btnStartL502.Text = "Старт";
                readDataThread.Stop();
                readDataThread = null;
                sw.Stop();
                sb.Items["Time"].Text = string.Empty;
                frMain.setSb("Info",string.Empty);
            }
        }

        private void FRTestL502_Load(object sender, EventArgs e)
        {
            FormPosSaver.load(this);
            Program.tube.onDataChanged += tubeDataChanged;
            tbView.Enabled = !(Program.tube.rawDataSize > 0);
            tbSaveData.Enabled = !(Program.tube.rawDataSize > 0);
        }

        private void FRTestL502_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.tube.onDataChanged -= tubeDataChanged;
        }
        
        private void tbView_Click(object sender, EventArgs e)
        {
            //FRCutView frm = new FRCutView(frMain)
            //{
            //    MdiParent = frMain,
            //};
            //frm.Show();
        }


        private void tbSaveLcard_Click(object sender, EventArgs e)
        {
            frMain.setSb("Info", "Запись данных в файл...");
            setSb("Info", "Запись данных в файл...");
            SaveFileDialog sfd = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "csv",
                Filter = "Файлы CSV (*.csv)|*.csv|Все файлы (*.*)|*.*",
            };
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName, false))
                {

                    for (int i = 0; i < Program.tube.rawDataSize; i++)
                    {
                        writer.WriteLine(Program.tube.rtube.data[i].ToString());
                    }
                    writer.Close();
                }
            }
            frMain.setSb("Info", string.Empty);
            setSb("Info", string.Empty);
        }
    }
}
