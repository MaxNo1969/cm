using FPS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CM
{
    /// <summary>
    /// Форма для просмотра данных по всем датчикам в одном ряду
    /// </summary>
    /// <remarks>
    /// <list>
    /// Вызывается из формы просмотра данных по трубе по двойному щелчку
    /// Показывает данные по всем датчикам в одном ряду
    /// </list>
    /// </remarks>
    public partial class FRRowAllSensorsView : FormSp
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_parent">MDIParent</param>
        /// <param name="_tube">Труба</param>
        /// <param name="_mrow">Ряд модуля</param>
        /// <param name="_row">Ряд датчика</param>
        public FRRowAllSensorsView(Form _parent,Tube _tube, int _mrow, int _row)
        {
            InitializeComponent();
            MdiParent = _parent;
            lay.RowCount = _tube.mcols*_tube.cols;
            lay.RowStyles.Clear();
            for (int i = 0; i < lay.RowCount; i++)
            {
                lay.RowStyles.Add(new RowStyle(SizeType.Percent, (float)(100.0 / lay.RowCount)));
                Chart c = new Chart();
                lay.Controls.Add(c,0,i);
                c.Dock = DockStyle.Fill;
                c.Tag = i;
                double[] data = _tube.getSensorData(i / _tube.cols, _mrow, i % _tube.cols, _row);
                double avg = data.Average();
                double max = data.Max();
                double min = data.Min();
                double maxAbs = Math.Max(Math.Abs(max), Math.Abs(min));
                for (int x = 0; x < data.Length; x++) data[x] = Math.Abs(data[x] - avg) / maxAbs;
                c.addGraph(string.Format("sensor{0}", i), Color.Green, data);
                c.ChartAreas["Area"].AxisX.LabelAutoFitMaxFontSize = 5;
                c.ChartAreas["Area"].AxisY.LabelAutoFitMaxFontSize = 5;
            }
        }
    }
}
