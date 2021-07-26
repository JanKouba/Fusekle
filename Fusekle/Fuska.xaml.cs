using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
        private double startPositionX;
        private double startPositionY;

        public bool Engaged { get => engaged; set => engaged = value; }
        public bool PreEngaged { get => preEngaged; set => preEngaged = value; }
        public Rect Rect { get => rect; }
        public int BodyColor { get => bodyColor; }
        public int StripesColor { get => stripesColor; }

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
            startPositionX = Canvas.GetLeft(this);
            startPositionY = Canvas.GetTop(this);

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
                this.engaged = true;
                engagedMisto.Engaged = true;
                engagedMisto.BodyColor = this.bodyColor;
                engagedMisto.StripesColor = this.stripesColor;
                Canvas.SetLeft(this, engagedMisto.rect.Left);
                Canvas.SetTop(this, engagedMisto.rect.Top);
                SoundPlayer.Play(SoundPlayer.Sounds.GoodStep);
            }
            else
            { 
                SoundPlayer.Play(SoundPlayer.Sounds.WrongStep);
              
                Canvas.SetLeft(this, startPositionX);
                Canvas.SetTop(this, startPositionY);
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

                int indexMisto = 0;
                foreach (var misto in mista)
                {
                    if (!misto.Engaged)
                        if (Rect.IntersectsWith(misto.rect))
                        {
                            if (!GetNeighbors(indexMisto))
                            {
                                misto.WrongLight();
                               
                            }
                            else
                            {
                                misto.HighLight();
                                preEngaged = true;
                                engagedMisto = misto;
                                
                            }
                            return;
                        }
                        else
                        {
                            misto.LowLight();
                            preEngaged = false;
                        }
                    indexMisto++;
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

        private bool GetNeighbors(int indexCheckMisto)
        {
            bool result = false;

            if (indexCheckMisto % 2 == 0)
                result = GetRightNeighbor(indexCheckMisto);
            if (indexCheckMisto % 2 == 1)
                result = GetLeftNeighbor(indexCheckMisto);

            return result;

        }
        private bool GetRightNeighbor(int indexCheckMisto)
        {
            bool result = false;
            var mista = this.myCanvas.Children.OfType<Misto>();

            Misto rightNeighbor = mista.ElementAt(indexCheckMisto+1);
            if (!rightNeighbor.Engaged)
                result = true;
            else if (rightNeighbor.BodyColor == this.BodyColor && rightNeighbor.StripesColor == this.stripesColor)
                result = true;

            return result;

        }

        private bool GetLeftNeighbor(int indexCheckMisto)
        {
            bool result = false;
            var mista = this.myCanvas.Children.OfType<Misto>();

            Misto rightNeighbor = mista.ElementAt(indexCheckMisto-1);
            if (!rightNeighbor.Engaged)
                result = true;
            else if (rightNeighbor.BodyColor == this.BodyColor && rightNeighbor.StripesColor == this.stripesColor)
                result = true;

            return result;
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

            fuskaStripe1.Style = style;
            fuskaStripe2.Style = style;
            fuskaStripe3.Style = style;

        }
    }
}

