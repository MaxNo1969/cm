using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    public static class WaitHelper
    {
        /// <summary>
        /// подождать указанное время
        /// </summary>
        /// <param name="seconds"> время в секундах </param>
        /// <summary>
        /// подождать указанное время
        /// </summary>
        /// <param name="seconds"> время в секундах </param>
        public static void Wait(int _milliseconds)
        {
            int ticks = System.Environment.TickCount + _milliseconds;
            while (System.Environment.TickCount < ticks)
            {
                Application.DoEvents();
            }
        }
    }
}
