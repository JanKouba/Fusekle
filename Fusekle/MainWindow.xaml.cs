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
using System.Text.RegularExpressions;


namespace Fusekle
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<Fuska> fusky = new List<Fuska>();
        List<Misto> mista = new List<Misto>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //for test: just create 6 pieces of item(fuska) and the same amount of places(misto)
            NewGame(9);
        }

        public int GetRandom(int min, int max)
        {
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new Random(seed).Next(min, max);
        }


        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void NewGame(int itemCount)
        {  
            int cnt = itemCount;

            while (cnt-- > 0)
            {
                Fuska f = new Fuska(
                    canvas, 
                    75 * cnt, 
                    0,
                    GetRandom(0, 9),
                    GetRandom(0, 9)
                    );

                Misto m = new Misto(75 * cnt, 150);

                canvas.Children.Add(m);
                canvas.Children.Add(f);

                fusky.Add(f);
                mista.Add(m);
            }
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            fusky.Clear();
            mista.Clear();
            canvas.Children.Clear();
            NewGame(9);
        }
    }
}
