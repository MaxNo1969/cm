using FormsExtras;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Форма для отображения текущих принимаемых данных с АЦП
    /// </summary>
    public partial class FRADCData : MyMDIForm
    {
        LCard lCard;
        /// <summary>
        /// Блокировка
        /// </summary>
        private object block = new object();
        /// <summary>
        /// Массивы для передачи данных в картинку с графиком
        /// x  - индекс в массиве
        /// y - значение (нормализовано)
        /// </summary>
        double[] x;
        double[] y;
        Dt sens = null;
        //сколько данных за раз выводим на график
        int size = 2048;

        //static int start = 0;
        /// <summary>
        /// Обработчик события записи очередной порции данных в трубу (вызывается в потоке readDataThread для класса Tube.LogTube.RawTube)
        /// </summary>
        /// <param name="_data">Данные для вывода в график</param>
        public void lcardDataRead(IEnumerable<double> _data)
        {
            if (_data != null)
            {
                IEnumerator<double> enumerator = _data.GetEnumerator();
                enumerator.Reset();
                for (int i = 0; i < size && enumerator.MoveNext(); i++)
                {
                    y[i] = enumerator.Current;
                }
            }
            ucGr.Invalidate();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_readThread">Поток чтения данных АЦП</param>
        public FRADCData(LCard _lCard)
        {
            lCard=_lCard;
            InitializeComponent();
            //ucGr.xScale = 0.5f;
            //ucGr.stepGridX = 100f;
            //ucGr.yScale = 0.01f;
            ucGr.stepGridY = 0.2f;
            ucGr.gridYFormatString = "{0,4:f2}";

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

        private void FRADCData_Load(object sender, EventArgs e)
        {
            lCard.onDataRead += lcardDataRead;
        }

        private void FRADCData_FormClosing(object sender, FormClosingEventArgs e)
        {
            lCard.onDataRead -= lcardDataRead;
        }
    }
}
