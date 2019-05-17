using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Protocol;

namespace CM
{
    /// <summary>
    /// Отображение каждого датчика отдельной строкой.
    /// </summary>
    /// <remarks>
    /// Данные беруться из RawTube
    /// </remarks>
    public partial class UCTubeAllSensors : UserControl
    {
        private Tube tube = null;
        /// <summary>
        /// Буфер для отрисовки
        /// </summary>
        private Bitmap backBuffer;
        private Graphics g;
        private Pen p;
        private Rectangle r;
        private Brush b;

        private byte[] bitmap;
        /// <summary>
        /// Конструктор
        /// </summary>
        public UCTubeAllSensors()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
        }

        int bitmapWidth;

        /// <summary>
        /// Инициализация. Указываем форму и трубу
        /// </summary>
        /// <param name="_tube">Труба</param>
        public void Init(Tube _tube)
        {
            tube = _tube;
            tube.onDataChanged += new DataChanged(x => Invalidate());
            bitmapWidth = (int)((double)tube.zones*Tube.GetsectionsPerZone() / tube.ptube.speed * Program.mtdadcFreq / Tube.sectionSize / 1000);
            if (bitmapWidth < tube.sections) bitmapWidth = tube.sections;
            if (tube.sections > 0)
            {
                backBuffer = new Bitmap(tube.sections, Tube.sectionSize);
                g = Graphics.FromImage(backBuffer);
                bitmap = new byte[tube.sections * Tube.sectionSize * 4];
                r = new Rectangle(tube.sections, 0, bitmapWidth - tube.sections, backBuffer.Height);
            }
            else
            {
                backBuffer = null;
                bitmap = null;
                r = new Rectangle(0, 0, Width, Height);
            }
            p = new Pen(Color.White, 2);
            b = new SolidBrush(Color.Gray);
        }

        private void data2bmpbytes()
        {
            if (bitmap == null) return;
            for (int sect = 0; sect < tube.sections; sect++)
            {
                for (int mcol = 0; mcol < Tube.mcols; mcol++)
                {
                    for (int mrow = 0; mrow < Tube.mrows; mrow++)
                    {
                        for (int col = 0; col < Tube.cols; col++)
                        {
                            for (int row = 0; row < Tube.rows; row++)
                            {
                                int y = mcol * Tube.rows * Tube.cols * Tube.rows +
                                    mrow * Tube.cols * Tube.rows + row * Tube.cols + col;
                                double val = Math.Abs(tube[mcol, mrow, col, row, sect]-tube.sensorsAvgValues[mcol,mrow,col,row]);
                                Color c = ColorHelper.getColor1(val);
                                int ind = tube.sections * 4 * y + sect * 4;
                                bitmap[ind + 3] = c.A;
                                bitmap[ind + 2] = c.R;
                                bitmap[ind + 1] = c.G;
                                bitmap[ind + 0] = c.B;
                            }
                        }
                    }
                }
            }
        }

        private void data2bmpbytesParallel()
        {
            if (bitmap == null) return;
            try
            {
                Parallel.For(0, tube.sections, sect =>
                {
                    Parallel.For(0, Tube.mcols, mcol =>
                    {
                        Parallel.For(0, Tube.mrows, mrow =>
                        {
                            Parallel.For(0, Tube.cols, col =>
                            {
                                Parallel.For(0, Tube.rows, row =>
                                {
                                    int y = mcol * Tube.rows * Tube.cols * Tube.rows +
                                        mrow * Tube.cols * Tube.rows + row * Tube.cols + col;
                                    double val = Math.Abs(tube[mcol, mrow, col, row, sect]-tube.sensorsAvgValues[mcol, mrow, col, row]);
                                    Color c = ColorHelper.getColor1(val);
                                    int ind = tube.sections * 4 * y + sect * 4;
                                    bitmap[ind + 3] = c.A;
                                    bitmap[ind + 2] = c.R;
                                    bitmap[ind + 1] = c.G;
                                    bitmap[ind + 0] = c.B;
                                });
                            });
                        });
                    });
                });
            }
            catch(Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message );
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Debug.WriteLine(logstr, "Error");
                }
                #endregion
            }
        }

        /// <summary>
        /// Пишем данные по всем датчикам
        /// </summary>
        private void data2bitmap()
        {
            if (backBuffer == null) return;
            for (int sect = 0; sect < tube.sections; sect++)
            {
                for (int mcol = 0; mcol < Tube.mcols; mcol++)
                {
                    for (int mrow = 0; mrow < Tube.mrows; mrow++)
                    {
                        for (int col = 0; col < Tube.cols; col++)
                        {
                            for (int row = 0; row < Tube.rows; row++)
                            {
                                int y = mcol * Tube.rows * Tube.cols * Tube.rows +
                                    mrow * Tube.cols * Tube.rows + row * Tube.cols + col;
                                double val = Math.Abs(tube[mcol, mrow, col, row, sect]-tube.sensorsAvgValues[mcol, mrow, col, row]);
                                backBuffer.SetPixel(sect, y, ColorHelper.getColor1(val));
                            }
                        }
                    }
                }
            }
        }
    
        /// <summary>
        /// Рисуем границы матриц
        /// </summary>
        private void sensorBounds2bitmap()
        {
            if (backBuffer == null) return;
            //Рисуем границы матриц
            int sensorSize = Tube.cols * Tube.rows;
            //Pen p = new Pen(Color.White, 2);
            Pen p = new Pen(Color.Black, 2);
            Graphics g = Graphics.FromImage(backBuffer);
            for (int y = 1; y < Tube.mcols * Tube.mrows; y++)
            {                
                g.DrawLine(p,0,y*sensorSize,backBuffer.Width,y*sensorSize);
            }
        }
        /// <summary>
        /// Рисуем границы зон
        /// </summary>
        private void zoneBounds2bitmap()
        {
            if (backBuffer == null) return;
            //Рисуем границы матриц
            int sensorSize = Tube.cols * Tube.rows;
            //Pen p = new Pen(Color.White, 2);
            Pen p = new Pen(Color.Black, 2);
            Graphics g = Graphics.FromImage(backBuffer);
            int zoneSize = backBuffer.Width / tube.zones;
            for (int x = 1; x < tube.zones; x++)
            {
                g.DrawLine(p, x*zoneSize, 0, x*zoneSize, backBuffer.Height);
            }
        }
        /// <summary>
        /// Обработчик OnPaint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCTubeAllSensors_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode)
            {
                Graphics g = e.Graphics;
                g.Clear(Color.Gray);
            }
            else
            {
                if (tube == null) return;
                try
                {
                    //data2bitmap();
                    data2bmpbytesParallel();
                    //data2bmpbytes();
                    if (backBuffer != null && bitmap != null && bitmap.Length > 0)
                        ImgHelper.setBitmapData(ref backBuffer, ref bitmap);
                    if (g != null && tube.sections < bitmapWidth) g.FillRectangle(b, r);
                    //Рисуем границы матриц
                    sensorBounds2bitmap();
                    //Рисуем зоны
                    //zoneBounds2bitmap();
                    if (backBuffer != null && Width > 0 && Height > 0)
                    {
                        Bitmap resized = ImgHelper.ResizeImage(backBuffer, Width, Height);
                        e.Graphics.DrawImage(resized, 0, 0);
                    }
                    else
                    {
                        Graphics g = e.Graphics;
                        g.Clear(Color.Gray);
                    }

                }
                catch (Exception ex)
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", ex.Message);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr, "Error");

                    }
                    #endregion
                }
            }
        }
    }
}
