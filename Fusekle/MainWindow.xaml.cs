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

        int langCode = 1033;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Returns random int
        /// </summary>
        /// <param name="min">From</param>
        /// <param name="max">Up To</param>
        /// <returns></returns>
        public int GetRandom(int min, int max)
        {
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new Random(seed).Next(min, max);
        }

        /// <summary>
        /// Starts new game 
        /// </summary>
        /// <param name="itemCount">Number of socks pairs</param>
        private void NewGame(int itemCount)
        {  
            int cnt = itemCount;

            while (cnt-- > 0)
            {
                Fuska f = new Fuska(
                    canvas, 
                    75 * cnt, 
                    50,
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

        /// <summary>
        /// Sets language by langCode (default = 1033)
        /// </summary>
        private void SetLanguage()
        {
            if (langCode == 1033)
                langCode = 1029;
            else
                langCode = 1033;

            ResourceDictionary dict = new ResourceDictionary();
            switch (langCode)
            {
                case 1033:
                    dict.Source = new Uri("..\\Languages\\Dict-EN.xaml", UriKind.Relative);
                    break;
                case 1029:
                    dict.Source = new Uri("..\\Languages\\Dict-CZ.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);

        }

        #region Menu interaction

        private void labelNewGame_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).Style = Resources["labelMenuSelected"] as Style;
        }

        private void labelNewGame_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).Style = Resources["labelMenuNewGame"] as Style;
        }

        private void labelDont_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).Style = Resources["labelMenuSelected"] as Style;
        }

        private void labelDont_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).Style = Resources["labelMenu"] as Style;
        }

        private void labelNewGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridMenu.Visibility = Visibility.Hidden;

            fusky.Clear();
            mista.Clear();
            canvas.Children.Clear();
            NewGame(9);
        }

        private void labelDont_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Label)sender).Style = Resources["labelMenuSelected"] as Style;
        }

        private void labelResume_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridMenu.Visibility = Visibility.Hidden;
        }
             
        private void labelLang_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetLanguage();
        }

        private void labelExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                gridMenu.Visibility = Visibility.Visible;
        }

        #endregion

    }
}
