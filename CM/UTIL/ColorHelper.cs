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
        public static Color getPureColor(double _val)
        {
            if (double.IsNaN(_val) || double.IsNegativeInfinity(_val) || double.IsPositiveInfinity(_val)) return Color.Gray;
            if (_val > Program.settings.Current.Border1) return Color.Red;
            else if (_val > Program.settings.Current.Border2) return Color.Yellow;
            else return Color.Green;
        }

        public static Brush getPureBrush(double _val)
        {
            return new SolidBrush(getPureColor(_val));
        }

        /// <summary>
        /// Получаем цвет по значению
        /// </summary>
        /// <param name="_val">Значение</param>
        /// <returns>Цвет для заданного значения</returns>
        public static Color getColor(double _val)
        {
            if(double.IsNaN(_val) || double.IsNegativeInfinity(_val) || double.IsPositiveInfinity(_val)) return Color.Gray;
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
        private static int rColorVal = 230;
        public static Color getColor1(double _val)
        {
            if (double.IsNaN(_val) || double.IsNegativeInfinity(_val) || double.IsPositiveInfinity(_val)) return Color.Gray;

            if (_val > Program.settings.Current.Border1) return Color.FromArgb(255, 0, 0);
            _val = _val - Program.settings.Current.Border2;
            int r;
            int g;
            if (_val > 0)
            {
                _val = _val / (Program.settings.Current.Border1 - Program.settings.Current.Border2);
                r = rColorVal;
                g = (int)Math.Ceiling(255 * (1 - _val));
            }
            else
            {
                _val = _val / Program.settings.Current.Border2;
                r = (int)Math.Ceiling(rColorVal * (1 + _val));
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
        public static Brush getBrush1(double _val)
        {
            return new SolidBrush(getColor1(_val));
        }
    }
}
