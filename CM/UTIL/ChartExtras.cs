using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace CM
{
    static class ChartExtras
    {
        public static string name = "Area";
        public static void addGraph(this Chart _ch, string _name, Color _color, IEnumerable<double> _data)
        {
            ChartArea a = _ch.ChartAreas.FindByName(name);
            if (a == null) _ch.ChartAreas.Add(name);
            _ch.ChartAreas[name].AxisX.Minimum = 0;
            Series ser = _ch.Series.FindByName(_name);
            if (ser == null) ser = _ch.Series.Add(_name);
            ser.Points.Clear();
            ser.ChartType = SeriesChartType.Line;
            ser.Color = _color;
            for (int i = 0; i < _data.Count(); i++)
                ser.Points.AddXY(i, _data.ElementAt(i));
        }
        public static void setYInterval(this Chart _ch,double _interval)
        {
            ChartArea a = _ch.ChartAreas.FindByName(name);
            if (a == null) _ch.ChartAreas.Add(name);
            a.AxisY.Interval = _interval;
        }
        public static void addGraph(this Chart _ch, string _name, Color _color, IEnumerable<double> _x,IEnumerable<double> _y)
        {
            ChartArea a = _ch.ChartAreas.FindByName(name);
            if (a == null) _ch.ChartAreas.Add(name);
            Series ser = _ch.Series.FindByName(_name);
            if (ser == null) ser = _ch.Series.Add(_name);
            ser.Points.Clear();
            ser.ChartType = SeriesChartType.Line;
            ser.Color = _color;
            for (int i = 0; i < _x.Count(); i++)
                ser.Points.AddXY(_x.ElementAt(i), _y.ElementAt(i));
        }
        public static void clearGraph(this Chart _ch, string _name)
        {
            Series ser = _ch.Series.FindByName(_name);
            if (ser != null)ser.Points.Clear();
        }
        public static void clearAllGraphs(this Chart _ch)
        {
            foreach(Series ser in _ch.Series)ser.Points.Clear();
        }
    }
}
