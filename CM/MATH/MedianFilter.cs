using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CM
{
    class Median
    {
        private static double[] _filter(double[] _data, int winWidth)
        {
            double[] window = new double[winWidth];
            double[] result = new double[_data.Length];
            // Move window through all elements of the signal
            for (int i = (winWidth - 1) / 2; i < _data.Count() - (winWidth - 1) / 2; i++)
            {
                Array.Copy(_data, i - (winWidth - 1) / 2, window, 0, winWidth);
                for (int j = 0; j < (winWidth - 1) / 2; j++)
                {
                    // Find position of minimum element
                    int min = j;
                    for (int k = j + 1; k < winWidth; k++)
                        if (window[k] < window[min]) min = k;
                    // Put found minimum element in its place
                    double temp = window[j];
                    window[j] = window[min];
                    window[min] = temp;
                }
                // Get result - the middle element
                result[i - (winWidth - 1) / 2] = window[(winWidth - 1) / 2];
            }
            return result;
        }
        public static double[] Filter(double[] _data, int winWidth)
        {
            // Check arguments
            if (_data == null || _data.Count() < 1) return null;
            // Treat special case N = 1
            if (_data.Count() == 1) return new double[] { _data[0] };
            // Allocate memory for signal extension
            double[] extension = new double[_data.Length + (winWidth - 1)];
            // Check memory allocation
            if (extension == null) return null;
            // Create signal extension
            Array.Copy(_data, 0, extension, (winWidth - 1) / 2, _data.Length);
            for (int i = 0; i < (winWidth - 1) / 2; ++i)
            {
                extension[i] = _data[0];
                extension[_data.Length + (winWidth - 1) / 2 + i] = _data[_data.Length - 1];
            }
            // Call median filter implementation
            return _filter(extension, winWidth);
        }
    }
}
