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
            fusky.Clear();
            mista.Clear();
            canvas.Children.Clear();

            int cntM = 0;

            while (cntM < itemCount * 2)
            {
                Misto m = new Misto(75 * cntM, 150);
                canvas.Children.Add(m);
                mista.Add(m);
                cntM++;
            }

            int cntF = itemCount;

            while (cntF-- > 0)
            {
                int bodyColor = GetRandom(0, 9);
                int stripesColor = GetRandom(0, 9);
                
                for (int i = 1; i <= 2; i++)
                { 
                Fuska f = new Fuska(
                    canvas, 
                    75 * cntF, 
                    50,
                    bodyColor,
                    stripesColor
                    );
                    canvas.Children.Add(f);
                    fusky.Add(f);
                }
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

        private class Pair
        {
            private Fuska fuska1;
            private Fuska fuska2;
            private bool resolved = false;

            public Fuska Fuska1 { get => fuska1; set => fuska1 = value; }
            public Fuska Fuska2 { get => fuska2; set => fuska2 = value; }
            public bool Resolved { get => resolved; set => resolved = value; }

            public Pair (Fuska fuska_1, Fuska fuska_2)
            {
                fuska1 = fuska_1;
                fuska2 = fuska_2;
            }
        }

    }
}
