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
using System.Threading;
using System.Windows.Threading;
using System.Xml.Linq;

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
        int itemCount = 0;
        int rowCount = 0;

        int langCode = 1033;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SoundPlayer.Play(SoundPlayer.Sounds.MenuMusic);

            //Init volume control
            SoundPlayer.Volume += 100;
            SoundPlayer.Volume -= 100;
            
            SetItemCount(20);

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"Images\wallpaper.jpg", UriKind.Relative));
            canvas.Background = ib;
            canvas.Visibility = Visibility.Hidden;
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
        private async void NewGame(int itemCount)
        {
            fusky.Clear();
            mista.Clear();
            canvas.Children.Clear();

            int rowM = rowCount;
            int cntM = ((itemCount * 2) / rowM) + 2;

            int currentRowM = 0;


            while (currentRowM < rowM)
            {
                int currentColumnM = 0;

                while (currentColumnM < cntM)
                {
                    Misto m = new Misto(75 * currentColumnM, 150 * (1 + currentRowM));
                    canvas.Children.Add(m);
                    mista.Add(m);
                    currentColumnM++;
                }
                currentRowM++;
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
                        10,
                        //75 * cntF, 
                        50,
                        bodyColor,
                        stripesColor,
                        rowM);
                    canvas.Children.Add(f);
                    fusky.Add(f);

                    f.EventFuskaPlaced += FuskaPlaced;
                    f.EventRightClick += FuskaRightClick;
                }


            }

            foreach (Fuska fuska in fusky)
            {
                Panel.SetZIndex(fuska, GetRandom(0, itemCount * 2));
            }

            canvas.Visibility = Visibility.Visible;

            CreateGameStats();
        }

        private void FuskaRightClick(object sender, EventArgs e)
        {
            Panel.SetZIndex((Fuska)sender, Panel.GetZIndex(((Fuska)sender))-1);
        }

        private void TimerGame_Tick(object sender, EventArgs e)
        {
            tsGame = DateTime.Now - dateTimeGame;
            labelGameTime.Content = tsGame.ToString(@"hh\:mm\:ss\.ff");
        }

        Label labelGameTime;
        Label labelFuskaLeft;
        private void CreateGameStats()
        {
            Grid gridGameStats = new Grid();
            gridGameStats.HorizontalAlignment = HorizontalAlignment.Left;
            gridGameStats.VerticalAlignment = VerticalAlignment.Top;
            gridGameStats.Height = 100;

            StackPanel stackPanel = new StackPanel();
            stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            stackPanel.Orientation  = Orientation.Horizontal;

            labelFuskaLeft = new Label();
            labelFuskaLeft.Style = (Style)FindResource("labelFusekle");
            stackPanel.Children.Add(labelFuskaLeft);

            labelGameTime = new Label();
            labelGameTime.Style = (Style)FindResource("labelMenuNewGame");
            stackPanel.Children.Add(labelGameTime);

            gridGameStats.Children.Add(stackPanel);  

            canvas.Children.Add(gridGameStats);

            dateTimeGame = DateTime.Now;
            tsGame = TimeSpan.Zero;

            timerGame = new DispatcherTimer();
            timerGame.Interval = TimeSpan.FromMilliseconds(10);
            timerGame.Tick += TimerGame_Tick;
            timerGame.Start();

        }

        TimeSpan tsGame ;
        DateTime dateTimeGame;
        DispatcherTimer timerGame;
        int fuskaLeft;
        
        

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

        private void FuskaPlaced(object sender, EventArgs e)
        {
            fuskaLeft = fusky.Count(obj => obj.Engaged == false);

            labelFuskaLeft.Content = fuskaLeft;
            DetectGameWin();
        }

        private void DetectGameWin()
        {
           if(fuskaLeft == 0) GameWin();

        }

        private void GameWin()
        {
            SoundPlayer.Play(SoundPlayer.Sounds.WinApplause);
            timerGame.Stop();

            Image myImage = new Image();
            BitmapImage myImageSource = new BitmapImage();
            myImageSource.BeginInit();
            myImageSource.UriSource = new Uri(@"Images\youwin.png", UriKind.Relative);
            myImageSource.EndInit();
            myImage.Source = myImageSource;
            myImage.HorizontalAlignment = HorizontalAlignment.Center;
            myImage.VerticalAlignment = VerticalAlignment.Center;

            canvas.Children.Add(myImage);

            Canvas.SetTop(myImage, (canvas.ActualHeight - myImageSource.Height) / 2);
            Canvas.SetLeft(myImage, (canvas.ActualWidth - myImageSource.Width) / 2);
        }

        private void SetItemCount(int add)
        {
            itemCount += add;

            if (itemCount > 40)
                itemCount = 8;
            if (itemCount < 8)
                itemCount = 40;

            switch (itemCount)
            {
                case var expression when itemCount < 10:
                    rowCount = 2;
                    break;
                case var expression when itemCount < 16:
                    rowCount = 3;
                    break;
                case var expression when itemCount < 20:
                    rowCount = 5;
                    break;
                case var expression when itemCount < 30:
                    rowCount = 6;
                    break;
                case var expression when itemCount < 41:
                    rowCount = 7;
                    break;
            }

            labelItemCount.Content = itemCount;
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
            NewGame(itemCount);
            SoundPlayer.Play(SoundPlayer.Sounds.NewGame);
        }

        private void labelDont_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Label)sender).Style = Resources["labelMenuSelected"] as Style;
            SoundPlayer.Play(SoundPlayer.Sounds.DontTouch);
            System.Threading.Thread.Sleep(1000);
            SoundPlayer.Play(SoundPlayer.Sounds.MenuMusic);
        }

        private void labelResume_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridMenu.Visibility = Visibility.Hidden;
            SoundPlayer.Close();
            canvas.Visibility = Visibility.Visible;
            timerGame.Start();
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
            {
                gridMenu.Visibility = Visibility.Visible;
                SoundPlayer.Play(SoundPlayer.Sounds.MenuMusic);
                canvas.Visibility = Visibility.Hidden;
                timerGame.Stop();
            }
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

        private void labelVolPlus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SoundPlayer.Volume < 1000)
                SoundPlayer.Volume += 100;
        }

        private void labelVolMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SoundPlayer.Volume > 0)
                SoundPlayer.Volume -= 100;
        }

        private void labelItemPlus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetItemCount(2);
        }

        private void labelItemMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetItemCount(-2);
        }
    }
}
