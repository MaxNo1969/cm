using FormsExtras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Форма для просмотра данных по одному датчику Холла
    /// </summary>
    public partial class FRHallSensorView : MyMDIForm
    {
        Tube tube;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба</param>
        /// <param name="_mrow"></param>
        /// <param name="_row"></param>
        /// <param name="_col"></param>
        /// <param name="_start"></param>
        /// <param name="_cnt"></param>
        public FRHallSensorView(Form _mdiParent, Tube _tube, int _mrow, int _row, int _col, int _start = 0, int _cnt = 100) : base(_mdiParent)
        {
            tube = _tube;
            InitializeComponent();
            for (int mrow = 0; mrow < Tube.mrows; mrow++)
            {
                cbSensorNum.Items.Add(mrow.ToString());
            }
            cbSensorNum.SelectedIndex = _mrow;
            for (int row = 0; row < Tube.rows; row++)
            {
                cbRow.Items.Add(row.ToString());
            }
            cbRow.SelectedIndex = _row;
            txtStart.Text = tube.ptube.l2px(_start).ToString();
            txtEnd.Text = tube.ptube.l2px(_start+_cnt).ToString();
            selectedIndexChanged(null, null);
        }

        private void selectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSensorNum.SelectedIndex < 0 || cbRow.SelectedIndex < 0) return;
            //double[] data = tube.getSensorData(0, cbSensorNum.SelectedIndex, 0, cbRow.SelectedIndex);
            int start = 0;
            int end = tube.ptube.l2px(tube.ptube.Width);
            //try
            //{

            //    start = Convert.ToInt32(txtStart.Text);
            //    end = Convert.ToInt32(txtEnd.Text);
            //}
            //catch(Exception)
            //{
            //    return;
            //}
            if (start < 0 || start > tube.ptube.l2px(tube.ptube.Width)) return;
            if (end < 1 || end > tube.ptube.l2px(tube.ptube.Width)) return;
            int startMeas = tube.ptube.p2lx(start);
            int cntMeas = tube.ptube.p2lx(end - start);
            Text = string.Format(@"Данные по датчику Холла (Датчик:{0},Датчик Холла:{1}) Измерения ({2}-{3})- {4} измерений",
                cbSensorNum.SelectedIndex, cbRow.SelectedIndex, startMeas,startMeas+cntMeas,cntMeas);
            double[] x = new double[cntMeas];
            double[] y = new double[cntMeas];
            double[] badBound = new double[cntMeas];
            double[] c2Bound = new double[cntMeas];
            for (int i = 0; i < cntMeas; i++)
            {
                x[i] = tube.ptube.l2px(start + i);
                //y[i] = Math.Abs(data[startMeas + i] - tube.sensorsAvgValues[0, cbSensorNum.SelectedIndex, 0, cbRow.SelectedIndex]);
                double val = tube.ptube.data[startMeas + i, Convert.ToInt32(cbSensorNum.Text) * Tube.rows + Convert.ToInt32(cbRow.Text)];
                y[i] = double.IsInfinity(val)?0:val;
                badBound[i] = Program.settings.Current.Border1;
                c2Bound[i] = Program.settings.Current.Border2;
            }
            ch.addAreaGraph("HallSensorData", Color.Green, x, y);
            ch.addGraph("badBound", Color.Red, x, badBound);
            ch.addGraph("z2Bound", Color.Yellow, x, c2Bound);
            ch.setYInterval(0.1);
            ch.ChartAreas[0].AxisY.Minimum = 0;
            //ch.ChartAreas[0].AxisY.Maximum = Program.settings.Current.Border1 * 2;
            ch.ChartAreas[0].AxisY.Maximum = 1;
            ch.setXInterval(100);
            ch.ChartAreas[0].AxisX.Minimum = start; 
            ch.ChartAreas[0].AxisX.Maximum = end; 
            ch.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 5;
            ch.ChartAreas[0].AxisY.LabelAutoFitMaxFontSize = 5;
        }
    }
}
