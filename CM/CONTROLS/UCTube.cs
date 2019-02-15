using Protocol;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Форма для отображения трубы
    /// </summary>
    public partial class UCTube : UserControl
    {
        private FRTubeView frTubeView;

        private PhysTube ptube = null;
        private float cellXSize;
        private int xCells;
        private float cellYSize;
        private int yCells;
        /// <summary>
        /// Координата начала отображаемого окна в логических единицах
        /// </summary>
        public int winStart;
        private int winWidth;

        /// <summary>
        /// Текущее положение указателя по X
        /// </summary>
        public int curCellX;
        /// <summary>
        /// Текущее положение указателя по Y
        /// </summary>
        public int curCellY;

        /// <summary>
        /// Буфер для отрисовки
        /// </summary>
        private Image backBuffer = null;
        /// <summary>
        /// Количество зон отображаемых в верхнем окне
        /// </summary>
        [DisplayName("numZones"), Description("Количество отображаемых за раз зон"), Category("Труба"), DefaultValue(5)]
        public int numZones { get; set; }

        /// <summary>
        /// Размер верхней части окна в процентах
        /// </summary>
        [DisplayName("upWinPercent"), Description("Размер верхней части в процентах"), Category("Труба"), DefaultValue(50)]
        public int upWinPercent { get; set; }

        /// <summary>
        /// Возможность редактировать трубу
        /// </summary>
        [DisplayName("editable"), Description("Редактирование разрешено"), Category("Труба"), DefaultValue(false)]
        public bool editable { get; set; }

        /// <summary>
        /// Значение Класс2 для редактора (нормализованное)
        /// </summary>
        [DisplayName("normalizedClass2Value"), Description("Значение Класс2 для редактора"), Category("Труба"), DefaultValue(0.7)]
        public double normalizedClass2Value { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UCTube()
        {
            InitializeComponent();
            editable = false;

            numZones = 5;
            upWinPercent = 50;
            normalizedClass2Value = 0.7;

            winStart = 0;
            curCellX = 0;
            curCellY = 0;
        }
        /// <summary>
        /// Инициализация. Указываем форму и трубу
        /// </summary>
        /// <param name="_tube"></param>
        /// <param name="_frTubeView"></param>
        public void Init(PhysTube _ptube, FRTubeView _frTubeView)
        {
            ptube = _ptube;
            //xCells берем по размеру зоны и количеству зон в отображени
            xCells = ptube.logZoneSize * numZones;
            yCells = ptube.mrows * ptube.rows;
            frTubeView = _frTubeView;
            frTubeView.updateSb();
        }
        /// <summary>
        /// Обработчик OnPaint. Рисование картинки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCTube_Paint(object sender, PaintEventArgs e)
        {
            if (ptube == null) return;
            try
            {
                //Ширину ячеек выбираем в зависимости от ширины окна
                cellXSize = (float)Width / (float)xCells;
                //Вычислим высоту верхней части
                int height = (int)(Height * upWinPercent / 100f);
                cellYSize = (float)height / (float)yCells;
                //Рисуем верхнюю чатсь в буфер для избежания мерцания. 
                backBuffer = new Bitmap(Width, height);
                Pen zoneBorderPen = new Pen(Color.White, 2f);
                using (Graphics g = Graphics.FromImage(backBuffer))
                {
                    for (int x = 0; x < xCells; x++)
                    {
                        for (int y = 0; y < yCells; y++)
                        {
                            Brush b = (y < ptube.Height && winStart + x < ptube.Width) ? ColorHelper.getBrush(ptube[winStart + x, y]) : Brushes.White;
                            g.FillRectangle(b, x * cellXSize, y * cellYSize, cellXSize, cellYSize);
                            g.DrawRectangle(Pens.Black, x * cellXSize, y * cellYSize, cellXSize, cellYSize);
                        }
                    }
                    //рисуем границы зон
                    for (int x = 0; x < xCells; x += ptube.logZoneSize)
                    {
                        g.DrawLine(zoneBorderPen, x * cellXSize, 0, x * cellXSize, Height);
                    }
                    //Рисуем границы матриц
                    for (int i = 1; i < ptube.mrows; i++)
                    {
                        g.DrawLine(zoneBorderPen, 0, i * cellYSize * ptube.mrows*ptube.rows, Width, 
                            i * cellYSize * ptube.mrows * ptube.rows);
                    }
                    //Рисуем курсор текущей ячейки
                    g.DrawRectangle(zoneBorderPen, curCellX * cellXSize, curCellY * cellYSize, cellXSize, cellYSize);
                }
                e.Graphics.DrawImage(backBuffer, 0, 0);
                backBuffer.Dispose();
                //Количество верхних блоков во всей трубе
                //float cntWins = model.Width / xCells;
                //Размер одного окна в нижней части
                //winWidth = (float)Width * xCells / (float)tube.Width;
                winWidth = ptube.logZoneSize * numZones;
                //Вычислим высоту нижней части
                height = Height * (100 - upWinPercent) / 100;
                backBuffer = new Bitmap(ptube.Width, ptube.Height);
                //ToDo - сделать заполнение Bitmap-а как UCTubeAllSensors 
                for (int x = 0; x < ptube.Width; x++)
                    for (int y = 0; y < ptube.Height; y++)
                    {
                        (backBuffer as Bitmap).SetPixel(x, y, ColorHelper.getColor(ptube[x, y]));
                    }
                if (winStart + 1 < ptube.Width)
                {
                    for (int y = 0; y < ptube.Height; y++)
                    {
                        (backBuffer as Bitmap).SetPixel(winStart, y, Color.White);
                        (backBuffer as Bitmap).SetPixel(winStart + 1, y, Color.White);
                    }
                }
                if (winStart + winWidth + 1 < ptube.Width)
                {
                    for (int y = 0; y < ptube.Height; y++)
                    {
                        (backBuffer as Bitmap).SetPixel(winStart + winWidth, y, Color.White);
                        (backBuffer as Bitmap).SetPixel(winStart + winWidth + 1, y, Color.White);
                    }
                }
                //Рисуем границы матриц
                for (int i = 1; i < ptube.mrows; i++)
                {
                    for (int x = 0; x < ptube.Width; x++)
                    {
                        (backBuffer as Bitmap).SetPixel(x, i * ptube.rows, Color.White);
                    }
                }

                backBuffer = ImgHelper.ResizeImage(backBuffer, Width, height);
                e.Graphics.DrawImage(backBuffer, 0, Height * upWinPercent / 100);
                backBuffer.Dispose();
                backBuffer = null;
            }
            catch(Exception ex)
            {
                #region Логирование
                {
                    string msg = ex.Message;
                    string logstr = string.Format("{0}: {1}: {2}",GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name,msg);
                    log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr);
                }
                #endregion
            }
        }
        /// <summary>
        /// Переопределяем виртуальную функцию для предотвращения мерцания
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Поскольку перекрываем всю поверхность то ничего не делаем        
        }

        private void UCTube_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void UCTube_MouseClick(object sender, MouseEventArgs e)
        {
            bool needInvalidate = false;
            if (e.Y < Height * upWinPercent / 100)
            {
                int tmp = (int)((float)e.X / cellXSize);
                if (winStart + tmp < ptube.Width) 
                {
                    curCellX = tmp;
                    curCellY = (int)((float)e.Y / cellYSize);
                    needInvalidate = true;
                }
            }
            else
            {
                winStart = ((e.X * ptube.Width / Width) / ptube.logZoneSize) * ptube.logZoneSize;
                curCellX = 0;
                curCellY = 0;
                needInvalidate = true;
            }
            if (needInvalidate)
            {
                frTubeView.updateSb();
                Invalidate();
            }
        }

        private void UCTube_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bool needInvalidate = false;
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if (winStart > 0)
                        {
                            winStart -= ptube.logZoneSize;
                            curCellX = 0;
                            curCellY = 0;
                            needInvalidate = true;
                        }
                        break;
                    case Keys.Right:
                        if (winStart < ptube.Width - ptube.logZoneSize - 1)
                        {
                            winStart += ptube.logZoneSize;
                            curCellX = 0;
                            curCellY = 0;
                            needInvalidate = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if (winStart >= ptube.logZoneSize * numZones)
                        {
                            winStart -= ptube.logZoneSize * numZones;
                            curCellX = 0;
                            curCellY = 0;
                            needInvalidate = true;
                        }
                        break;
                    case Keys.Right:
                        if (winStart < ptube.Width - ptube.logZoneSize * numZones - 1)
                        {
                            winStart += ptube.logZoneSize * numZones;
                            curCellX = 0;
                            curCellY = 0;
                            needInvalidate = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if (curCellX > 0)
                        {
                            curCellX--;
                            needInvalidate = true;
                        }
                        break;
                    case Keys.Right:
                        if (curCellX < xCells - 1)
                        {
                            curCellX++;
                            needInvalidate = true;
                        }
                        break;
                    case Keys.Up:
                        if (curCellY > 0)
                        {
                            curCellY--;
                            needInvalidate = true;
                        }
                        break;
                    case Keys.Down:
                        if (curCellY < yCells - 1)
                        {
                            curCellY++;
                            needInvalidate = true;
                        }
                        break;
                    default:
                        break;
                }
                if (editable)
                {
                    //Проверка на выход за пределы трубы
                    if (winStart + curCellX < ptube.Width)
                    {
                        switch (e.KeyCode)
                        {

                            case Keys.G:
                                ptube[winStart + curCellX, curCellY] = 0;
                                needInvalidate = true;
                                break;
                            case Keys.B:
                                ptube[winStart + curCellX, curCellY] = 1;
                                needInvalidate = true;
                                break;
                            case Keys.D2:
                                ptube[winStart + curCellX, curCellY] = normalizedClass2Value;
                                needInvalidate = true;
                                break;
                            case Keys.N:
                                ptube[winStart + curCellX, curCellY] = PhysTube.undefined;
                                needInvalidate = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {

                        case Keys.Enter:

                            break;
                        default:
                            break;
                    }

                }
            }
            if (needInvalidate)
            {
                frTubeView.updateSb();
                Invalidate();
            }
        }
        /// <summary>
        /// Возврашает номер зоны для текущей ячейки
        /// </summary>
        /// <returns></returns>
        public int GetZoneNum()
        {
            return (winStart + curCellX) / ptube.logZoneSize;
        }

        private void UCTube_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Y < Height * upWinPercent / 100)
            {
                int row =(int)(e.Y / cellYSize);
                int mrow = row / ptube.rows;
                row = row % ptube.rows;
                FRRowAllSensorsView frm = new FRRowAllSensorsView(frTubeView.MdiParent, ptube.tube, mrow, row);
                frm.Show();
            }
        }
    }
}
