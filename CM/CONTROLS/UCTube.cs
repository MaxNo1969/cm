﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormsExtras;
using Protocol;

namespace CM
{
    /// <summary>
    /// Форма для отображения трубы
    /// </summary>
    public partial class UCTube : UserControl
    {
        private FRTubeView frTubeView;

        private Tube tube = null;
        //private PhysTube ptube = null;

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
        /// Перо для рисования всяких границ
        /// </summary>
        private readonly Pen white2pix;

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

        public enum DrawType { Gradient, Pure }
        /// <summary>
        /// Значение Класс2 для редактора (нормализованное)
        /// </summary>
        [DisplayName("DrawType"), Description("Типп отрисовки"), Category("Труба"), DefaultValue(DrawType.Gradient)]
        public DrawType drawType
        {
            get
            {
                if (getColor == ColorHelper.getColor) return DrawType.Gradient;
                else return DrawType.Pure;
            }
            set
            {
                if(value==DrawType.Gradient)
                {
                    getColor = ColorHelper.getColor;
                    getBrush = ColorHelper.getBrush;
                }
                else
                {
                    getColor = ColorHelper.getPureColor;
                    getBrush = ColorHelper.getPureBrush;
                }
            }
        }

        Brush b;
        delegate Color GetColor(double _val);
        GetColor getColor;
        delegate Brush GetBrush(double _val);
        GetBrush getBrush;

        /// <summary>
        /// Конструктор
        /// </summary>
        public UCTube()
        {
            drawType = DrawType.Gradient;
            InitializeComponent();
            editable = false;

            numZones = 5;
            upWinPercent = 50;
            normalizedClass2Value = 0.7;

            white2pix = new Pen(Color.White, 2);

            winStart = 0;
            curCellX = 0;
            curCellY = 0;
        }
        /// <summary>
        /// Инициализация. Указываем форму и трубу
        /// </summary>
        /// <param name="_tube"></param>
        /// <param name="_frTubeView"></param>
        public void Init(Tube _tube, FRTubeView _frTubeView)
        {
            tube = _tube;
            //ptube = tube.ptube;
            //xCells берем по размеру зоны и количеству зон в отображени
            xCells = tube.ptube.logZoneSize * numZones;
            yCells = tube.ptube.mrows * tube.ptube.rows;
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
            if (tube == null) return;
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
                            if (y < tube.ptube.Height && winStart + x < tube.ptube.Width)
                            {
                                b = getBrush(tube[winStart + x, y]);
                            }
                            else
                                b = Brushes.White;
                            g.FillRectangle(b, x * cellXSize, y * cellYSize, cellXSize, cellYSize);
                            g.DrawRectangle(Pens.Black, x * cellXSize, y * cellYSize, cellXSize, cellYSize);
                        }
                    }
                    //рисуем границы зон
                    for (int x = 0; x < xCells; x += tube.ptube.logZoneSize)
                    {
                        g.DrawLine(zoneBorderPen, x * cellXSize, 0, x * cellXSize, Height);
                    }
                    //Рисуем границы матриц
                    for (int i = 1; i < tube.ptube.mrows; i++)
                    {
                        g.DrawLine(zoneBorderPen, 0, i * cellYSize * tube.ptube.mrows*tube.ptube.rows, Width, 
                            i * cellYSize * tube.ptube.mrows * tube.ptube.rows);
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
                winWidth = tube.ptube.logZoneSize * numZones;
                //Вычислим высоту нижней части
                height = Height * (100 - upWinPercent) / 100;
                backBuffer = new Bitmap(tube.ptube.Width, tube.ptube.Height);
                //ToDo - сделать заполнение Bitmap-а как UCTubeAllSensors 
                for (int x = 0; x < tube.ptube.Width; x++)
                    for (int y = 0; y < tube.ptube.Height; y++)
                    {
                        (backBuffer as Bitmap).SetPixel(x, y, getColor(tube[x, y]));
                    }

                using (Graphics g = Graphics.FromImage(backBuffer))
                {
                    if (winStart + 1 < tube.ptube.Width)
                        g.DrawLine(white2pix, winStart, 0, winStart, backBuffer.Height);
                    if (winStart + winWidth + 1 < tube.ptube.Width)
                        g.DrawLine(white2pix, winStart + winWidth, 0, winStart + winWidth, backBuffer.Height);
                    for (int i = 1; i < tube.ptube.mrows; i++)
                        g.DrawLine(Pens.White, 0, i * tube.ptube.rows, backBuffer.Width, i * tube.ptube.rows);
                }
                //if (winStart + 1 < tube.ptube.Width)
                //{
                //    for (int y = 0; y < tube.ptube.Height; y++)
                //    {
                //        (backBuffer as Bitmap).SetPixel(winStart, y, Color.White);
                //        (backBuffer as Bitmap).SetPixel(winStart + 1, y, Color.White);
                //    }
                //}
                //if (winStart + winWidth + 1 < tube.ptube.Width)
                //{
                //    for (int y = 0; y < tube.ptube.Height; y++)
                //    {
                //        (backBuffer as Bitmap).SetPixel(winStart + winWidth, y, Color.White);
                //        (backBuffer as Bitmap).SetPixel(winStart + winWidth + 1, y, Color.White);
                //    }
                //}
                //Рисуем границы матриц
                //for (int i = 1; i < tube.ptube.mrows; i++)
                //{
                //    for (int x = 0; x < tube.ptube.Width; x++)
                //    {
                //        (backBuffer as Bitmap).SetPixel(x, i * tube.ptube.rows, Color.White);
                //    }
                //}

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
                    Log.add(logstr, LogRecord.LogReason.error);
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
                if (winStart + tmp < tube.ptube.Width) 
                {
                    curCellX = tmp;
                    curCellY = (int)((float)e.Y / cellYSize);
                    needInvalidate = true;
                }
            }
            else
            {
                winStart = ((e.X * tube.ptube.Width / Width) / tube.ptube.logZoneSize) * tube.ptube.logZoneSize;
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
                            winStart -= tube.ptube.logZoneSize;
                            curCellX = 0;
                            curCellY = 0;
                            needInvalidate = true;
                        }
                        break;
                    case Keys.Right:
                        if (winStart < tube.ptube.Width - tube.ptube.logZoneSize - 1)
                        {
                            winStart += tube.ptube.logZoneSize;
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
                        if (winStart >= tube.ptube.logZoneSize * numZones)
                        {
                            winStart -= tube.ptube.logZoneSize * numZones;
                            curCellX = 0;
                            curCellY = 0;
                            needInvalidate = true;
                        }
                        break;
                    case Keys.Right:
                        if (winStart < tube.ptube.Width - tube.ptube.logZoneSize * numZones - 1)
                        {
                            winStart += tube.ptube.logZoneSize * numZones;
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
                    if (winStart + curCellX < tube.ptube.Width)
                    {
                        switch (e.KeyCode)
                        {

                            case Keys.G:
                                tube.ptube[winStart + curCellX, curCellY] = 0;
                                needInvalidate = true;
                                break;
                            case Keys.B:
                                tube.ptube[winStart + curCellX, curCellY] = 1;
                                needInvalidate = true;
                                break;
                            case Keys.D2:
                                tube.ptube[winStart + curCellX, curCellY] = normalizedClass2Value;
                                needInvalidate = true;
                                break;
                            case Keys.N:
                                tube.ptube[winStart + curCellX, curCellY] = PhysTube.undefined;
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
            return (winStart + curCellX) / tube.ptube.logZoneSize;
        }

        private void UCTube_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Y < Height * upWinPercent / 100)
            {
                int row =(int)(e.Y / cellYSize);
                int mrow = row / tube.ptube.rows;
                row = row % tube.ptube.rows;
                FRRowAllSensorsView frm = new FRRowAllSensorsView(frTubeView.MdiParent, tube, mrow, row);
                frm.Show();
            }
        }
    }
}
