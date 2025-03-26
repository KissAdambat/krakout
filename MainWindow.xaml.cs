using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Krakout_11B {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        double xSeb = 5;
        double ySeb = 5;
        int pontszam = 0;
        public MainWindow() {
            InitializeComponent();
            //labda.CacheMode = new BitmapCache();
            //// Sync rendering with frame refresh rate
            //CompositionTarget.Rendering += mozgatas;
            //// Lock the animation frame rate to 60 FPS
            //Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline),
            //    new FrameworkPropertyMetadata { DefaultValue = 60 });
            jatekter.Visibility = Visibility.Hidden;
        }

        private void mozgatas(object sender, EventArgs e) {
            // a játékos mozgatása
            Canvas.SetLeft(jatekos, Mouse.GetPosition(jatekter).X);
            var labdaX = Canvas.GetLeft(labda);
            var labdaY = Canvas.GetTop(labda);
            // nézzük a képernyő határait
            if (labdaX > 950) xSeb = -5;
            if (labdaX < 0) xSeb = 5;
            if (labdaY > 550) {
                // ha a labda eléri a képernyő alját, vonjon le egy pontot
                ySeb = -5;
                lbPontszam.Content = --pontszam;
            }
            if (labdaY < 0) ySeb = 5;

            // ütközésvizsgálat a labda és a játékos között
            var jatekosX = Canvas.GetLeft(jatekos);
            var jatekosY = Canvas.GetTop(jatekos);
            if (labdaX + labda.Width > jatekosX
                && labdaX < jatekosX + jatekos.Width
                && labdaY + labda.Height > jatekosY
                && labdaY < jatekosY + jatekos.Height
                ) {
                ySeb = -5;
                lbPontszam.Content = ++pontszam;
            }

            foreach (var tegla in jatekter.Children.OfType<Image>())
            {
                var teglaX = Canvas.GetLeft(tegla);
                var teglaY = Canvas.GetTop(tegla);
                if (labdaX + labda.Width > teglaX
                    && labdaX < teglaX + tegla.Width
                    && labdaY + labda.Height > teglaY
                    && labdaY < teglaY + tegla.Height
                    )
                {
                    ySeb *= -1;
                    jatekter.Children.Remove(tegla);
                    lbPontszam.Content = ++pontszam;
                    break;
                }
            }
            // a labda mozgatása
            Canvas.SetLeft(labda, labdaX + xSeb);
            Canvas.SetTop(labda, labdaY + ySeb);
        }

        private void startbutton_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            jatekter.Visibility = Visibility.Visible;
            jatek();
            //var ido = new DispatcherTimer();
            //ido.Interval = TimeSpan.FromMilliseconds(1);
            //ido.Tick += mozgatas;
            //ido.Start();
        }
        private void exitbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void exitgamebutton_Click(object sender, RoutedEventArgs e)
        {
            
            jatekter.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;
        }
        private void jatek()
        {
            var ido = new DispatcherTimer();
            ido.Interval = TimeSpan.FromMilliseconds(1);
            ido.Tick += mozgatas;
            ido.Start();
            int pont = Convert.ToInt32(lbPontszam.Content);


            for (int j = 0; j < 5; j++) 
            {
                for (int i = 0; i < 10; i++) 
                {
                    var tegla = new Image();
                    tegla.Source = new BitmapImage(new Uri("tegla.png", UriKind.Relative));
                    tegla.Width = 90;
                    tegla.Height = 20;
                    tegla.Stretch = Stretch.Fill;
                    Canvas.SetLeft(tegla, i * 100);
                    Canvas.SetTop(tegla, j * 30);
                    jatekter.Children.Add(tegla);
                }
            }

            if (pont < 2) 
            {
                if (pont == 0)
                {
                    MessageBox.Show("Veszettél nem maradt pontod");
                    jatekter.Visibility = Visibility.Hidden;
                    Menu.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
