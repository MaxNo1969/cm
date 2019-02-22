using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace CM
{
    /// <summary>
    /// Отображение каждого датчика отдельной строкой.
    /// </summary>
    /// <remarks>
    /// Данные беруться из RawTube и нормализуются согласно SensorCorrectionData
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
            bitmapWidth = (int)((double)tube.len / DefaultValues.Speed * DefaultValues.freq / tube.sectionSize / 1000);
            backBuffer = new Bitmap(tube.sections, tube.sectionSize);
            //backBuffer = new Bitmap(bitmapWidth, tube.sectionSize);
            p = new Pen(Color.White, 2);
            b = new SolidBrush(Color.White);
            r = new Rectangle(tube.sections, 0, backBuffer.Width, backBuffer.Height);
            g = Graphics.FromImage(backBuffer);
            bitmap = new byte[tube.sections * tube.sectionSize * 4];

        }

        private void data2bmpbytes()
        {
            for (int sect = 0; sect < tube.sections; sect++)
            {
                for (int mcol = 0; mcol < tube.mcols; mcol++)
                {
                    for (int mrow = 0; mrow < tube.mrows; mrow++)
                    {
                        for (int col = 0; col < tube.cols; col++)
                        {
                            for (int row = 0; row < tube.rows; row++)
                            {
                                int y = mcol * tube.rows * tube.cols * tube.rows +
                                    mrow * tube.cols * tube.rows + row * tube.cols + col;
                                double val = tube.getNorm(mcol, mrow, col, row, sect);
                                Color c = ColorHelper.getColor(val);
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
            Parallel.For(0, tube.sections, sect =>
            {
                Parallel.For(0, tube.mcols, mcol =>
                      {
                  Parallel.For(0, tube.mrows, mrow =>
                    {
                        Parallel.For(0, tube.cols, col =>
                         {
                             Parallel.For(0, tube.rows, row =>
                              {

                                  int y = mcol * tube.rows * tube.cols * tube.rows +
                                      mrow * tube.cols * tube.rows + row * tube.cols + col;
                                  double val = tube.getNorm(mcol, mrow, col, row, sect);
                                  Color c = ColorHelper.getColor(val);
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

        /// <summary>
        /// Пишем данные по всем датчикам
        /// </summary>
        private void data2bitmap()
        {
            for (int sect = 0; sect < tube.sections; sect++)
            {
                for (int mcol = 0; mcol < tube.mcols; mcol++)
                {
                    for (int mrow = 0; mrow < tube.mrows; mrow++)
                    {
                        for (int col = 0; col < tube.cols; col++)
                        {
                            for (int row = 0; row < tube.rows; row++)
                            {
                                int y = mcol * tube.rows * tube.cols * tube.rows +
                                    mrow * tube.cols * tube.rows + row * tube.cols + col;
                                double val = tube.getNorm(mcol, mrow, col, row, sect);
                                backBuffer.SetPixel(sect, y, ColorHelper.getColor(val));
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
            //Рисуем границы матриц
            int sensorSize = tube.cols * tube.rows;
            Pen p = new Pen(Color.White, 2);
            Graphics g = Graphics.FromImage(backBuffer);
            for (int y = 1; y < tube.mcols * tube.mrows; y++)
            {                
                g.DrawLine(p,0,y*sensorSize,backBuffer.Width,y*sensorSize);
            }
        }
        /// <summary>
        /// Рисуем границы зон
        /// </summary>
        private static void zoneBounds2bitmap()
        {
            //Рисуем зоны
            //for (int i = 1; i < tube.strobes.Count; i++)
            //{
            //    for (int y = 0; y < tube.sectionSize; y++)
            //    {
            //        backBuffer.SetPixel(tube.strobes[i].bound, y, Color.White);
            //        backBuffer.SetPixel(tube.strobes[i].bound + 1, y, Color.White);
            //    }
            //}
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
                    if (tube.sections < bitmapWidth) g.FillRectangle(b, r);
                    if(bitmap.Length>0)ImgHelper.setBitmapData(ref backBuffer, ref bitmap);
                    //Рисуем границы матриц
                    sensorBounds2bitmap();
                    //Рисуем зоны
                    //zoneBounds2bitmap();
                    Bitmap resized = ImgHelper.ResizeImage(backBuffer, Width, Height);
                    e.Graphics.DrawImage(resized, 0, 0);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format(
                        "{0}.{1}:{2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
                        );
                }
            }
        }
    }
}
