using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Bot.Util
{
    public class Graphic
    {
        private Robot self;
        private IGraphics grapher 
        {
            get { return self.Graphics; }
        }
        public Color DefaultColor { get; set; }

        public bool DrawingIsEnabled { get; set; }

        public Graphic(Robot aRobot)
        {
            self = aRobot;
            this.DefaultColor = Color.White;
            this.DrawingIsEnabled = true;
        }

        public void Line(double X1, double Y1, double X2, double Y2)
        {
            this.Line(DefaultColor, (float)X1, (float)Y1, (float)X2, (float)Y2);
        }
        public void Line(Color aColor, double X1, double Y1, double X2, double Y2)
        {
            if (DrawingIsEnabled)
                grapher.DrawLine(new Pen(aColor), (float)X1, (float)Y1, (float)X2, (float)Y2);
        }

        public void Rectangle(double X, double Y, double Width, double Height)
        {
            this.Rectangle(DefaultColor, X, Y, Width, Height);
        }
        public void Rectangle(Color aColor, double X, double Y, double Width, double Height)
        {
            if(DrawingIsEnabled)
                grapher.DrawRectangle(new Pen(aColor), (float)X, (float)Y, (float)Width, (float)Height);
        }

        public void Circle(double CenterX, double CenterY, double Radius)
        {
            this.Circle(DefaultColor, CenterX, CenterY, Radius);
        }
        public void Circle(Color aColor, double CenterX, double CenterY, double Radius)
        {
            this.Elipse(aColor, CenterX - Radius, CenterY - Radius, Radius * 2, Radius * 2);
        }

        public void Elipse(double X, double Y, double Width, double Height)
        {
            this.Elipse(DefaultColor, X, Y, Width, Height);
        }
        public void Elipse(Color aColor, double X, double Y, double Width, double Height)
        {
            if (DrawingIsEnabled)
                grapher.DrawEllipse(new Pen(aColor), (float)X, (float)Y, (float)Width, (float)Height);
        }
    }
}
