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
        private int bodyColor;
        private int stripesColor;

        public bool Engaged { get => engaged; set => engaged = value; }
        public bool PreEngaged { get => preEngaged; set => preEngaged = value; }
        public Rect Rect { get => rect; }
        public int BodyColor { get => bodyColor; }
        public int StripesColor { get => stripesColor; set => stripesColor = value; }

        private Rect rect;
      
        private Canvas myCanvas;

        private Misto engagedMisto;

        private bool _isRectDragInProg = false;

        public Fuska(Canvas canvas, double startX, double startY, int bColor, int sColor)
        {
            InitializeComponent();
            gridMain.Height = 90;
            gridMain.Width = 50;

            Canvas.SetTop(this, startY);
            Canvas.SetLeft(this, startX);

            myCanvas = canvas;
           
            rect = new Rect(startX, startY, 20, 20);

            bodyColor = bColor;
            stripesColor = sColor;

            SetColors();

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

                var mista = this.myCanvas.Children.OfType<Misto>();

                foreach (var misto in mista)
                {
                    if (!misto.Engaged)
                        if (Rect.IntersectsWith(misto.rect))
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
                var mousePos = e.GetPosition(myCanvas);
                Moving(mousePos.X, mousePos.Y);
            }
        }

        private void SetColors()
        {
            Style style = new Style();
            
            switch (bodyColor)
            {
                case 0: 
                    style = Resources["Black"] as Style;
                    break;
                case 1:
                    style = Resources["White"] as Style;
                    break;
                case 2:
                    style = Resources["OrangeRed"] as Style;
                    break;
                case 3:
                    style = Resources["DarkGray"] as Style;
                    break;
                case 4:
                    style = Resources["Goldenrod"] as Style;
                    break;
                case 5:
                    style = Resources["Brown"] as Style;
                    break;
                case 6:
                    style = Resources["Teal"] as Style;
                    break;
                case 7:
                    style = Resources["BlueViolet"] as Style;
                    break;
                case 8:
                    style = Resources["Olive"] as Style;
                    break;
                case 9:
                    style = Resources["DeepSkyBlue"] as Style;
                    break;
            }

            fuskaBody.Style = style;

            switch (stripesColor)
            {
                case 0:
                    style = Resources["Black"] as Style;
                    break;
                case 1:
                    style = Resources["White"] as Style;
                    break;
                case 2:
                    style = Resources["OrangeRed"] as Style;
                    break;
                case 3:
                    style = Resources["DarkGray"] as Style;
                    break;
                case 4:
                    style = Resources["Goldenrod"] as Style;
                    break;
                case 5:
                    style = Resources["Brown"] as Style;
                    break;
                case 6:
                    style = Resources["Teal"] as Style;
                    break;
                case 7:
                    style = Resources["BlueViolet"] as Style;
                    break;
                case 8:
                    style = Resources["Olive"] as Style;
                    break;
                case 9:
                    style = Resources["DeepSkyBlue"] as Style;
                    break;
            }

            stripe1.Style = style;
            stripe2.Style = style;
            stripe3.Style = style;

        }
    }
}

