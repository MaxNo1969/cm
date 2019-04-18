using System.Windows.Forms;
using System.Drawing;
using System;
using System.Diagnostics;
using FormsExtras;
using System.Collections;
using System.Collections.Generic;

namespace CM
{
    /// <summary>
    /// Отображение трубы
    /// </summary>
    public partial class FRTubeView : MyMDIForm
    {
        #region Доступ к статусу из других потоков
        /// <summary>
        /// Доступ к статусу из других потоков
        /// </summary>
        private delegate void SetSbText(string _sbItem, string _sbText);
        private static SetSbText sbText = null;
        void setSbText(string _sbItem, string _sbText)
        {
            this.sb.Items[_sbItem].Text = _sbText;
        }
        private static object[] args = null;
        private void prepareSbText()
        {
            sbText += setSbText;
            args = new object[2];
        }
        /// <summary>
        /// Вызов Invoke для обновления статусбара из другого потока
        /// почему-то толком не работает. Надо разбираться. 
        /// </summary>
        /// <param name="_sbItem">Имя пля в статусбаре</param>
        /// <param name="_sbText">Выводимая строка</param>
        public void setSb(string _sbItem, string _sbText)
        {
            if (InvokeRequired)
            {
                //args[0] = _sbItem;
                //args[1] = _sbText;
                //BeginInvoke(sbText, args);
                BeginInvoke(new Action(() => { sb.Items[_sbItem].Text = _sbText; }));
            }
            else
            {
                sb.Items[_sbItem].Text = _sbText;
            }
        }
        #endregion
        Tube tube = null;
        FRMain frm;
        
        /// <summary>
        /// Возможность редактировать трубу
        /// </summary>
        public bool editable { get { return ucTube.editable; } set { ucTube.editable = value; } }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба для рисования</param>
        public FRTubeView(Tube _tube,FRMain _frMain)
        {
            tube = _tube;
            frm = _frMain;
            MdiParent = frm;
            InitializeComponent();
            ucTube.Init(tube, this);
        }
        /// <summary>
        /// Бработка изменения размеров
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            ucTube.Size = new Size(ClientSize.Width, ClientSize.Height - sb.Height);
        }

        private void FRTubeModel_Load(object sender, System.EventArgs e)
        {
            //восстановление размеров главного окна        
            tube.onDataChanged += tube_onDataChanged;
        }

        void tube_onDataChanged(IEnumerable<double> _data)
        {
            if (tube.ptube.endWritedX % (tube.ptube.logZoneSize*(ucTube.numZones+1)) == 0)
                ucTube.winStart = tube.ptube.endWritedX;
            ucTube.curCellX = tube.ptube.endWritedX % (tube.ptube.logZoneSize*ucTube.numZones);
            ucTube.curCellY = tube.ptube.endWritedY;
            ucTube.Invalidate();
            updateSb();
        }

        private void FRTubeView_FormClosing(object sender, FormClosingEventArgs e)
        {
            tube.onDataChanged -= tube_onDataChanged;
        }
        /// <summary>
        /// Вывод информации в строку статуса
        /// </summary>
        public void updateSb()
        {
            setSb("Zone",string.Format("Зона: {0} ({1,5:f2}-{2,5:f2})", ucTube.GetZoneNum(),
                tube.ptube.l2px(ucTube.GetZoneNum() * tube.ptube.logZoneSize) / 1000f,
                tube.ptube.l2px((ucTube.GetZoneNum() + 1) * tube.ptube.logZoneSize) / 1000f));
            setSb("PositionX", string.Format("{0,6:f3} М", tube.ptube.l2px(ucTube.winStart + ucTube.curCellX) / 1000f));
            setSb("PositionY", string.Format("{0,3} мм", tube.ptube.l2py(ucTube.curCellY)));

            double val = PhysTube.undefined;
            if (ucTube.winStart + ucTube.curCellX < tube.ptube.Width && ucTube.curCellY < tube.ptube.Height)
                val = tube.ptube[ucTube.winStart + ucTube.curCellX, ucTube.curCellY];
            if (val == PhysTube.undefined || ucTube.winStart + ucTube.curCellX >= tube.ptube.Width ||
                ucTube.curCellY >= tube.ptube.Height)
            {
                setSb("Value","Н/Д");
            }
            else
            {
                setSb("Value", string.Format("{0,5:f3}", tube.ptube[ucTube.winStart + ucTube.curCellX, ucTube.curCellY]));
            }
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            if(true)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    DefaultExt = "bin",
                    AddExtension = true,
                    Filter = "Файлы bin (*.bin)|*.bin|Все файлы (*.*)|*.*",
                    OverwritePrompt = true,
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Tube.save(tube, sfd.FileName);
                }
            }
        }

        private void miLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "bin",
                AddExtension = true,
                Filter = "Файлы bin (*.bin)|*.bin|Все файлы (*.*)|*.*",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!Tube.load(ref tube, ofd.FileName))
                {
                    MessageBox.Show(string.Format("Не удалось загрузить трубу из файла {0}", ofd.FileName), "Ошибка");
                }
                tube.ptube.endWritedX = 0;
                tube.ptube.endWritedY = 0;
            }
        }

        private void miReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистить данные по трубе?", "Очистка", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
            }
        }
        private void miFill_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Заполнить данные нулями?", "Очистка", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
            }
        }
    }
}
