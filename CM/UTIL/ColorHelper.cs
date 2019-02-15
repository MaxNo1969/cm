using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM
{
    public static class ColorHelper
    {
        /// <summary>
        /// Получаем цвет по значению
        /// </summary>
        /// <param name="_val">Значение</param>
        /// <returns>Цвет для заданного значения</returns>
        public static Color getColor(double _val)
        {
            if (_val > 1 || _val < 0) return Color.Gray;
            _val -= 0.5;
            int r;
            int g;
            if (_val > 0)
            {
                r = 255;
                g = (int)Math.Ceiling(255 * (1 - _val));
            }
            else
            {
                r = (int)Math.Ceiling(255 * (1 + _val));
                g = 255;
            }
            return Color.FromArgb(r, g, 0);
        }
        /// <summary>
        /// Получаем кисть по значению
        /// </summary>
        /// <param name="_val">Значение</param>
        /// <returns>Кисть для заданного значения</returns>
        public static Brush getBrush(double _val)
        {
            return new SolidBrush(getColor(_val));
        }
    }
}
