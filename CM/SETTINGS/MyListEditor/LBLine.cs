using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Protocol;

namespace CM
{
    class LBLine
    {
        ListBox LB;
        int curY;
        int curW;
        public enum InsertPosition { Before, After, None }
        public InsertPosition IP { get; private set; }
        public int CurrentIndex { get; private set; }

        Color ColorLine;
        Color ColorEmpty;
        public LBLine(ListBox _LB)
        {
            LB = _LB;
            ColorLine = Color.Blue;
            ColorEmpty = LB.BackColor;
            IP = InsertPosition.None;
            CurrentIndex = -1;
        }
        public bool IsSelected(int _X, int _Y)
        {
            int index = LB.IndexFromPoint(_X, _Y);
            if (index < 0)
                return (false);
            return (index == LB.SelectedIndex);
        }
        public bool IsIndex(int _mouseX, int _mouseY)
        {
            Point point = LB.PointToClient(new Point(_mouseX, _mouseY));
            return (LB.IndexFromPoint(point.X, point.Y) >= 0);
        }
        public bool Draw(int _mouseX, int _mouseY)
        {
            ClearLine();
            Point point = LB.PointToClient(new Point(_mouseX, _mouseY));
            CurrentIndex = LB.IndexFromPoint(point.X, point.Y);
            if (CurrentIndex < 0)
                return (false);
            if (CurrentIndex == LB.SelectedIndex)
            {
                CurrentIndex = -1;
                return (false);
            }
//            DrawLine(CurrentIndex, point.Y);
            Rectangle bounds;
            bounds = LB.GetItemRectangle(CurrentIndex);
            int half = bounds.Top + (bounds.Height / 2);
            IP = point.Y < half ? InsertPosition.Before : InsertPosition.After;
            Color col = Color.Red;
            if (IP == InsertPosition.Before)
            {
                if (CurrentIndex - 1 == LB.SelectedIndex)
                {
                    CurrentIndex = -1;
                    return (false);
                }
                curY = bounds.Top;
            }
            else if (IP == InsertPosition.After)
            {
                if (CurrentIndex + 1 == LB.SelectedIndex)
                {
                    CurrentIndex = -1;
                    return (false);
                }
                curY = bounds.Bottom;
                col = Color.Blue;
            }
            curW = Math.Min(bounds.Width, LB.ClientSize.Width);
            DrawLine(ColorLine, curY, curW);
            return (true);
        }
        public void ClearLine()
        {
            if (CurrentIndex >= 0)
                DrawLine(ColorEmpty, curY, curW);
            CurrentIndex = -1;
        }
        void DrawLine(Color _color, int _y, int _width)
        {
            using (Graphics g = LB.CreateGraphics())
            {
                using (Pen pen = new Pen(_color))
                {
                    g.DrawLine(pen, 0, _y, _width, _y);
                }
            }
        }
    }
}
