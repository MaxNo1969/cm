using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CM
{
    /// <summary>
    /// Свой UserControl для отображения графиков  врельном времени
    /// </summary>
    public partial class UCGraph : UserControl, IDisposable
    {

        List<Dt> graphics;
        
        /// <summary>
        /// Масштабирование: количиство логических единиц на пиксел
        /// </summary>
        [DisplayName("xScale"), Description("Масштабирование: количиство логических единиц на пиксел"), Category("Данные")]
        public float xScale { get; set; }
        /// <summary>
        /// Масштабирование: количиство логических единиц на пиксел
        /// </summary>
        [DisplayName("yScale"), Description("Масштабирование: количиство логических единиц на пиксел"), Category("Данные")]
        public float yScale { get; set; }
        /// <summary>
        ///сдвиг начала координат в экранных координатах
        ///</summary>
        [DisplayName("xOffset"), Description("Сдвиг начала координат в экранных координатах"), Category("Сетка"), DefaultValue(0f)]
        public float xOffset { get; set; }
        /// <summary>
        ///сдвиг начала координат в экранных координатах
        ///</summary>
        [DisplayName("yOffset"), Description("Сдвиг начала координат в экранных координатах"), Category("Сетка"), DefaultValue(0f)]
        public float yOffset { get; set; }

        /// <summary>
        /// Минимальное значение X
        /// </summary>
        [DisplayName("minX"), Description("Минимальное значение X"), Category("Данные"), DefaultValue(0f)]
        public float minX { get; set; }
        /// <summary>
        /// Минимальное значение Y
        /// </summary>
        [DisplayName("minY"), Description("Минимальное значение Y"), Category("Данные"), DefaultValue(100f)]
        public float minY { get; set; }
        /// <summary>
        /// Максимальное значение X
        /// </summary>
        [DisplayName("maxX"), Description("Максимальное значение X"), Category("Данные"), DefaultValue(-10f)]
        public float maxX { get; set; }
        /// <summary>
        /// Максимальное значение Y
        /// </summary>
        [DisplayName("maxY"), Description("Максимальное значение Y"), Category("Данные"), DefaultValue(10f)]
        public float maxY { get; set; }
        
        //Пересчет координат из логических в экраные
        PointF log2scr(PointF _pt)
        {
           return new PointF(_pt.X / xScale + xOffset, -_pt.Y / yScale + yOffset);
        }
        PointF log2scr(float _x,float _y)
        {
            return new PointF(_x / xScale + xOffset, -_y / yScale + yOffset);
        }
        float log2scrX(float _x) 
        { 
            return _x / xScale + xOffset; 
        }
        float log2scrY(float _y) 
        { 
            return -_y / yScale + yOffset; 
        }

        //Пересчет координат из экраных в логические
        PointF scr2log(PointF _pt)
        {
            return new PointF((_pt.X - xOffset) * xScale, (_pt.Y - yOffset) * yScale);
        }
        PointF scr2log(float _x, float _y)
        {
            return new PointF((_x - xOffset) * xScale, (_y - yOffset) * yScale);
        }
        float scr2logX(float _x) 
        { 
            return (_x - xOffset) * xScale; 
        }
        float scr2logY(float _y) 
        { 
            return (_y - yOffset) * yScale; 
        }

        
        //Рисование сетки
        /// <summary>
        /// Шаг сетки по X в логических координатах
        /// </summary>
        [DisplayName("stepGridX"), Description("Шаг сетки по X в логических координатах"), Category("Сетка"), DefaultValue(50)]
        public float stepGridX { get; set; }
        //private float stepGridX = 50f;
        /// <summary>
        /// Шаг сетки по Y
        /// </summary>
        [DisplayName("stepGridY"), Description("Шаг сетки по Y в логических координатах"), Category("Сетка"), DefaultValue(0.2f)]
        public float stepGridY { get; set; }
        /// <summary>
        /// Подписи на оси Х
        /// </summary>
        [DisplayName("showGridX"), Description("Подписи на оси Х"), Category("Сетка"), DefaultValue(true)]
        public bool showGridX { get; set; }
        /// <summary>
        /// Подписи на оси Y
        /// </summary>
        [DisplayName("showGridY"), Description("Подписи на оси Y"), Category("Сетка"), DefaultValue(true)]
        public bool showGridY { get; set; }

        private static int gridFontSize = 6;
        private static Font gridFont = new Font("Verdana", gridFontSize, FontStyle.Regular);

        /// <summary>
        /// Формат надписей по оси X
        /// </summary>
        public string gridXFormatString = "{0}";
        /// <summary>
        /// Формат надписей по оси Y
        /// </summary>
        public string gridYFormatString = "{0,6:f3}";                

        void drawGrid(Graphics _g)
        {
            for (float x = 0; x > minX; x -= stepGridX)
            {
                _g.DrawLine(Pens.Black, log2scrX(x), log2scrY(minY), log2scrX(x), log2scrY(maxY));
                //if(showGridX)_g.DrawString(string.Format(gridXFormatString, x), gridFont, Brushes.Black, log2scrX(x) + 3, log2scrY(0)/* - 12*/);
                if(showGridX)_g.DrawString(string.Format(gridXFormatString, x), gridFont, Brushes.Black, log2scrX(x) + 3, log2scrY(0)/* - 12*/);
            }
            //int i = 0;
            //for (float x = 0; x < maxX && i<graphics[0].x.Length; x += stepGridX,i+=(int)stepGridX)
            //{
            //    _g.DrawLine(Pens.Black, log2scrX(x), log2scrY(minY), log2scrX(x), log2scrY(maxY));
            //    if (showGridX) _g.DrawString(string.Format(gridXFormatString, graphics[0].x[i]), gridFont, Brushes.Black, log2scrX(x) + 3, log2scrY(0)/* - 12*/);
            //}
            for (float x = 0; x < maxX; x += stepGridX)
            {
                _g.DrawLine(Pens.Black, log2scrX(x), log2scrY(minY), log2scrX(x), log2scrY(maxY));
                if (showGridX) _g.DrawString(string.Format(gridXFormatString, x), gridFont, Brushes.Black, log2scrX(x) + 3, log2scrY(0)/* - 12*/);
            }
            for (float y = 0; y > minY; y -= stepGridY)
            {
                _g.DrawLine(Pens.Black, log2scrX(minX), log2scrY(y), log2scrX(maxX), log2scrY(y));
                if (showGridY) _g.DrawString(string.Format(gridYFormatString, y), gridFont, Brushes.Black, log2scrX(0) + 3, log2scrY(y));
            }
            for (float y = 0; y < maxY; y += stepGridY)
            {
                _g.DrawLine(Pens.Black, log2scrX(minX), log2scrY(y), log2scrX(maxX), log2scrY(y));
                if (showGridY) _g.DrawString(string.Format(gridYFormatString, y), gridFont, Brushes.Black, log2scrX(0) + 3, log2scrY(y));
            }
        }

        void drawData(Graphics _g,double[] _dataX, double[] _dataY,Pen _p)
        {
            if (_dataX != null && _dataX.Length > 0 && _dataY != null && _dataY.Length > 0)
            {
                int sz = (_dataX.Length < _dataY.Length) ? _dataX.Length : _dataY.Length;
                for (int i = 0; i < sz-1; i++)
                {
                    /*
                    if (_dataY[i] == Tresholds.treshUnknown || _dataY[i + 1] == Tresholds.treshUnknown)
                        continue;
                    */ 
                    _g.DrawLine(_p, log2scrX((float)_dataX[i]), log2scrY((float)_dataY[i]),
                        log2scrX((float)_dataX[i+1]), log2scrY((float)_dataY[i + 1]));
                }
            }
        }

        void drawData(Graphics _g, Dt _dt)
        {
            drawData(_g, _dt.x, _dt.y, _dt.p);
        }

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public UCGraph()
        {
            InitializeComponent();
            stepGridX = 50f;
            stepGridY = 0.002f;
            xScale = 1f;
            yScale = 0.0001f;
            graphics = new List<Dt>();
        }

        
        //Рисуем на битмапе для предотвращения мерцания
        private Image backBuffer = null;
        private void UCGraph_Paint(object sender, PaintEventArgs e)
        {
            //Font font = new Font("Verdana", 5, FontStyle.Regular);
            backBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (Graphics g = Graphics.FromImage(backBuffer))
            {
                g.Clear(BackColor);
                xOffset = 0;
                //float minX = scr2logX(0f);
                //float maxX = scr2logX(width);
                //float minX = (float)graphics[0].x[0];
                //float maxX = (float)graphics[0].x[graphics[0].x.Length - 1];
                if (maxX - minX > 0)
                    xScale = (maxX - minX) / ClientSize.Width;
                else
                    xScale = 50f;
                yOffset = ClientSize.Height / 2f;
                //float minY = (0f-yOffset)/yScale;
                //float minY = scr2logY(0f);
                //float maxY = scr2logY(height);
                //float minY = (float)-5;
                //float maxY = (float)5;
                //if (maxY - minY != 0)
                if (maxY - minY > 0)
                    yScale = (maxY - minY) / ClientSize.Height;
                else
                    yScale = 0.2f;

                drawGrid(g);
                foreach (Dt _dt in graphics)
                {
                    drawData(g, _dt);
                }
            }
            e.Graphics.DrawImage(backBuffer, 0, 0);
            backBuffer.Dispose();
        }

        /// <summary>
        /// Предотвращаем мерцания при перерисовке фона
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void UCGraph_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Добавление графика
        /// </summary>
        /// <param name="_dt"></param>
        public void addData(Dt _dt)
        {
            graphics.Add(_dt);
            Invalidate();
        }
        /// <summary>
        /// Добавление графика
        /// </summary>
        /// <param name="_p">Карандаш для рисования</param>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void addData(Pen _p, double[] _x, double[] _y)
        {
            graphics.Add(new Dt(_p,_x,_y));
            Invalidate();
        }
        /// <summary>
        /// Удаление графика
        /// </summary>
        /// <param name="_ind"></param>
        public void removeAt(int _ind)
        {
            graphics.RemoveAt(_ind);
            Invalidate();
        }
        /// <summary>
        /// Удаление графика
        /// </summary>
        /// <param name="_dt"></param>
        public void remove(Dt _dt)
        {
            graphics.Remove(_dt);
            Invalidate();
        }

        void IDisposable.Dispose()
        {
            gridFont.Dispose();
            base.Dispose();
        }
    }
    /// <summary>
    ///Представление для одного графика на форме
    /// </summary>
    public class Dt
    {
        /// <summary>
        /// Карандаш для рисования
        /// </summary>
        public Pen p;
        /// <summary>
        /// Координаты X
        /// </summary>
        public double[] x;
        /// <summary>
        /// Координаты Y
        /// </summary>
        public double[] y;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_p">Карандаш для рисования</param>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public Dt(Pen _p, double[] _x, double[] _y)
        {
            p = _p;
            x = _x;
            y = _y;
        }
    }
}
