using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fusekle
{
    /// <summary>
    /// Interakční logika pro Misto.xaml
    /// </summary>
    public partial class Misto : UserControl
    {
        public Misto(double startX, double startY)
        {
            InitializeComponent();

            Canvas.SetLeft(this, startX);
            Canvas.SetTop(this, startY);

            rect = new Rect(startX, startY, 50, 90);
        }

        public Rect rect;
        public bool isHighLighted = false;

        private int bodyColor;
        private int stripesColor;
        private bool engaged;

        public bool Engaged { get => engaged; set { engaged = value; OnEngaged(); } }
        public int BodyColor { get => bodyColor; set => bodyColor = value; }
        public int StripesColor { get => stripesColor; set => stripesColor = value; }

        public void HighLight()
        {
            gridMain.Style = Resources["HighLight"] as Style;
            this.Opacity = 0.5;
            isHighLighted = true;
        }

        public void LowLight()
        {
            gridMain.Style = Resources["LowLight"] as Style;
            this.Opacity = 0.2;
            isHighLighted = false;
        }

        public void WrongLight()
        {
            gridMain.Style = Resources["WrongLight"] as Style;
            this.Opacity = 0.5;
            isHighLighted = false;
        }

        private void OnEngaged()
        {
            if(engaged)
                gridMain.Style = Resources["Engaged"] as Style;

        }
    }
}
