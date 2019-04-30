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
        public FRHallSensorView(Tube _tube, int _mrow, int _row, int _col, int _start = 0, int _cnt = 100) : base()
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
            txtStartMeas.Text = _start.ToString();
            txtCountMeas.Text = _cnt.ToString();
            selectedIndexChanged(null, null);
        }

        private void selectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSensorNum.SelectedIndex < 0 || cbRow.SelectedIndex < 0) return;
            double[] data = tube.getSensorData(0, cbSensorNum.SelectedIndex, 0, cbRow.SelectedIndex);
            int start = 0;
            int count = 1;
            try
            {

                start = Convert.ToInt32(txtStartMeas.Text);
                count = Convert.ToInt32(txtCountMeas.Text);
            }
            catch(Exception)
            {
                return;
            }
            if (start < 0 || start > data.Length - 1) return;
            if (count < 1 || start + count > data.Length) return;
            Text = string.Format(@"Данные по датчику Холла (Датчик:{0},Датчик Холла:{1}) Измерения ({2}-{3})- {4} измерений",
                cbSensorNum.SelectedIndex, cbRow.SelectedIndex, start,count,data.Length);
            double[] data1 = new double[count];
            //double avg = data.Average();
            for (int i = 0; i < count; i++)
                data1[i] = data[start + i]-tube.sensorsAvgValues[0, cbSensorNum.SelectedIndex, 0, cbRow.SelectedIndex];
            ch.addGraph("HallSensorData", Color.Green, data1);
            ch.setYInterval(0.1);
        }
    }
}
