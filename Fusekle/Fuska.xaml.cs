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

        public event EventHandler EventFuskaPlaced;
        public event EventHandler EventFuskaMisplaced;
        public event EventHandler EventRightClick;

        private Rect rect;

        private Canvas myCanvas;

        private Misto engagedMisto;

        private bool _isRectDragInProg = false;

        private int _rowCount = 0;

        public Fuska(Canvas canvas, double startX, double startY, int bColor, int sColor, int rowCount)
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

            _rowCount = rowCount;
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
                EventFuskaPlaced?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Canvas.SetLeft(this, startPositionX);
                Canvas.SetTop(this, startPositionY);
                EventFuskaMisplaced?.Invoke(this, EventArgs.Empty);
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
                    misto.Index = indexMisto;

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

        private bool GetNeighbors(int indexMisto)
        {
            bool result = false;
            var mista = this.myCanvas.Children.OfType<Misto>();

            //Is it the first in a row?
            if (indexMisto == 0 || indexMisto % (mista.Count() / _rowCount) == 0)
                result = true;
            else
            {
                if (indexMisto % 2 == 1) // If indexMisto is odd
                    result = GetLeftNeighbor(indexMisto, _rowCount);
                if (indexMisto % 2 == 0) // If indexMisto is even
                    result = GetLeftNeighbor(indexMisto, _rowCount) || GetUpDownNeighbor(indexMisto, _rowCount);
            }

            return result;
        }

        private bool GetUpDownNeighbor(int indexCheckMisto, int rowCount)
        {
            bool result = false;
            var mista = this.myCanvas.Children.OfType<Misto>();

            //If index > total count / row count => check upper row
            if (indexCheckMisto >= mista.Count() / rowCount)
            {
                Misto rightNeighbor = mista.ElementAt(indexCheckMisto - mista.Count() / rowCount);
                if (rightNeighbor.Engaged)
                    result = true;
            }

            //If index + total count / row count< total count = > check lower row
            if (indexCheckMisto + mista.Count() / rowCount < mista.Count())
            {
                Misto rightNeighbor = mista.ElementAt(indexCheckMisto + mista.Count() / rowCount);
                if (rightNeighbor.Engaged)
                    result = true;
            }

            return result;

        }
      
        private bool GetLeftNeighbor(int indexCheckMisto, int rowCount)
        {
            bool result = false;
            var mista = this.myCanvas.Children.OfType<Misto>();

            Misto leftNeighbor = mista.ElementAt(indexCheckMisto - 1);

            if (indexCheckMisto % 2 == 0 && leftNeighbor.Engaged)
                result = true;
            else
                if (indexCheckMisto % 2 == 1)
            {
                if (leftNeighbor.BodyColor == this.BodyColor && leftNeighbor.StripesColor == this.stripesColor)
                    result = true;

                //if (indexCheckMisto < mista.Count() - 2)                            // the place is not the last one
                //    if ((indexCheckMisto + 1) % (mista.Count() / rowCount) != 0)    // and the place is not on the end of the row
                //        if (mista.ElementAt(indexCheckMisto + 1).Engaged && !mista.ElementAt(indexCheckMisto - 1).Engaged)           // and the place on the right is already engaged
                //            result = true;
            }

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

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            EventRightClick?.Invoke(this, EventArgs.Empty);
        }
    }


}

