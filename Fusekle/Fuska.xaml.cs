using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fusekle
{
    /// <summary>
    /// Interakční logika pro Fuska.xaml
    /// </summary>
    public partial class Fuska : UserControl
    {
        private bool engaged;
        private bool preEngaged;

        public bool Engaged { get => engaged; set => engaged = value; }
        public bool PreEngaged { get => preEngaged; set => preEngaged = value; }
       
        public Rect rect;
        bool _isRectDragInProg = false;
        Canvas MyCanvas;

        private Misto engagedMisto;

        public Fuska(Canvas canvas, double startX, double startY, string name)
        {
            InitializeComponent();
            gridMain.Height = 90;
            gridMain.Width = 50;

            Canvas.SetTop(this, startY);
            Canvas.SetLeft(this, startX);

            MyCanvas = canvas;
            Name = name;

            rect = new Rect(startX, startY, 20, 20);
        }


        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 0.5;
            _isRectDragInProg = true;
            this.CaptureMouse();
        }

        private void gridMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           this.Opacity = 1;
            _isRectDragInProg = false;
            this.ReleaseMouseCapture();
            if (preEngaged)
            {
                engaged = true;
                engagedMisto.Engaged = true;
                Canvas.SetLeft(this, engagedMisto.rect.Left);
                Canvas.SetTop(this, engagedMisto.rect.Top);

            }
        }

        private void gridMain_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }

        public void Moving(double X, double Y)
        {
            if (_isRectDragInProg)
            {
                double left = X - (this.ActualWidth / 2);
                double top = Y - (this.ActualHeight / 2);
                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);

                rect = new Rect(X, Y, 20, 20);

                var mista = this.MyCanvas.Children.OfType<Misto>();

                foreach (var misto in mista)
                {
                    if (!misto.Engaged)
                        if (rect.IntersectsWith(misto.rect))
                        {
                            misto.HighLight();
                            preEngaged = true;
                            engagedMisto = misto;
                            return;
                        }
                        else
                        {
                            misto.LowLight();
                            preEngaged = false;
                        }
                }
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.Engaged && _isRectDragInProg)
            {
                var mousePos = e.GetPosition(MyCanvas);
                Moving(mousePos.X, mousePos.Y);
            }
        }
    }
}

