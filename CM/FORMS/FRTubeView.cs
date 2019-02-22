using System.Windows.Forms;
using System.Drawing;
using System;
using Protocol;
using System.Diagnostics;
using FPS;
using System.Collections;
using System.Collections.Generic;

namespace CM
{
    /// <summary>
    /// Отображение трубы
    /// </summary>
    public partial class FRTubeView : FormSp
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
        PhysTube ptube = null;
        FRMain frm;
        
        /// <summary>
        /// Возможность редактировать трубу
        /// </summary>
        public bool editable { get { return ucTube.editable; } set { ucTube.editable = value; } }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба для рисования</param>
        public FRTubeView(PhysTube _ptube,FRMain _frMain)
        {
            ptube = _ptube;
            frm = _frMain;
            MdiParent = frm;
            InitializeComponent();
            ucTube.Init(ptube,this);
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
            FormPosSaver.load(this);
            ptube.tube.onDataChanged += tube_onDataChanged;
        }

        void tube_onDataChanged(IEnumerable<double> _data)
        {
            if (ptube.endWritedX % (ptube.logZoneSize*(ucTube.numZones+1)) == 0)
                ucTube.winStart = ptube.endWritedX;
            ucTube.curCellX = ptube.endWritedX % (ptube.logZoneSize*ucTube.numZones);
            ucTube.curCellY = ptube.endWritedY;
            ucTube.Invalidate();
            updateSb();
        }

        private void FRTubeView_FormClosing(object sender, FormClosingEventArgs e)
        {
            ptube.tube.onDataChanged -= tube_onDataChanged;
            //Сохраняем положение главного окна
            FormPosSaver.save(this);
        }
        /// <summary>
        /// Вывод информации в строку статуса
        /// </summary>
        public void updateSb()
        {
            setSb("Zone",string.Format("Зона: {0} ({1,5:f2}-{2,5:f2})", ucTube.GetZoneNum(), 
                ptube.l2px(ucTube.GetZoneNum() * ptube.logZoneSize) / 1000f,
                ptube.l2px((ucTube.GetZoneNum() + 1) * ptube.logZoneSize) / 1000f));
            setSb("PositionX", string.Format("{0,6:f3} М", ptube.l2px(ucTube.winStart + ucTube.curCellX) / 1000f));
            setSb("PositionY", string.Format("{0,3} мм", ptube.l2py(ucTube.curCellY)));

            double val = PhysTube.undefined;
            if (ucTube.winStart + ucTube.curCellX < ptube.Width && ucTube.curCellY < ptube.Height)
                val = ptube[ucTube.winStart + ucTube.curCellX, ucTube.curCellY];
            if (val == PhysTube.undefined || ucTube.winStart + ucTube.curCellX >= ptube.Width ||
                ucTube.curCellY >= ptube.Height)
            {
                setSb("Value","Н/Д");
            }
            else
            {
                setSb("Value", string.Format("{0,5:f3}", ptube[ucTube.winStart + ucTube.curCellX, ucTube.curCellY]));
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
                    Tube.save(ptube.tube, sfd.FileName);
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
                if (!Tube.load(out ptube.tube, ofd.FileName))
                {
                    MessageBox.Show(string.Format("Не удалось загрузить трубу из файла {0}", ofd.FileName), "Ошибка");
                }
                ptube.endWritedX = 0;
                ptube.endWritedY = 0;
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
