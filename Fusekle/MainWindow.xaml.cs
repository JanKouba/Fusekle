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
            NewGame(6);
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void NewGame(int itemCount)
        {  
            int cnt = itemCount;

            while (cnt-- > 0)
            {
                Fuska f = new Fuska(canvas, 75 * cnt, 0, "A");
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
            NewGame(6);
        }
    }
}
