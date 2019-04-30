using Protocol;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FormsExtras;
using System.Collections.Generic;

namespace CM
{
    /// <summary>
    /// Просмотр сырых данных по трубе
    /// </summary>
    public partial class FRTubeRawView : MyMDIForm
    {
        Tube tube;
        /// <summary>
        /// Максимальное количество отображаемых за раз секций (при большом количестве Chart начинает тормозить)
        /// </summary>
        private const int maxNumSections = 5;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба для просмотра</param>
        public FRTubeRawView(Tube _tube, Form _frm, ToolStripMenuItem _menu = null):base(_frm,_menu,null)
        {
            tube = _tube;
            InitializeComponent();
            txtStart.TextChanged += new EventHandler((object sender, EventArgs e) => { updateChart(); });
            txtCount.TextChanged += new EventHandler((object sender, EventArgs e) => { updateChart(); });
            //Настраиваем Chart
            ChartArea a = chart.ChartAreas["Area"];
            a.AxisX.Minimum = 0;
            a.AxisX.Interval = 50;
            lblSections.Text = string.Format("Измерения({0} - {1})", 0, tube.sections / tube.rtube.sectionSize);
            lblCount.Text = string.Format("Измерения({0} - {1})", 1, maxNumSections);
        }

        void tube_onDataChanged(IEnumerable<double> _data)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    lblSections.Text = string.Format("Измерения({0} - {1})", 0, tube.sections / tube.rtube.sectionSize);
                    //updateChart();
                }));
            }
            else
            {
                lblSections.Text = string.Format("Измерения({0} - {1})", 0, tube.sections / tube.rtube.sectionSize);
                updateChart();
            }
        }

        private void updateChart()
        {
            if (tube.sections==0)
            {
                chart.clearAllGraphs();
                return;
            }
            int start = 0;
            int count = 1; 
            try
            {
                start = Convert.ToInt32(txtStart.Text);
                count = Convert.ToInt32(txtCount.Text);
            }
            catch (Exception e)
            {
                #region Логирование Ошибка
                {
                    string msg = e.Message;
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr,"Error");
                }
                #endregion 
                return;
            }
            if (start < 0 || count < 1 || count > maxNumSections) return;
            chart.addGraph("RawData", Color.Green, tube.getSectionsData(start, count));
            chart.Update();
        }

        private void FRTubeRawView_Load(object sender, EventArgs e)
        {
            tube.onDataChanged += tube_onDataChanged;
            updateChart();
        }

        private void FRTubeRawView_FormClosing(object sender, FormClosingEventArgs e)
        {
            tube.onDataChanged -= tube_onDataChanged;
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            updateChart();
        }
    }
}
