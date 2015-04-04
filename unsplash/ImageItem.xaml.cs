using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

namespace unsplash
{
    /// <summary>
    /// Interaction logic for ImageItem.xaml
    /// </summary>
    public partial class ImageItem : UserControl
    {
        readonly SplashImage _image;
        public ImageItem(SplashImage image)
        {
            _image = image;
            InitializeComponent();
            this.ImageThumbnail.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + _image.Category + "\\" + _image.ThumbnailFileName, UriKind.Absolute));
            LabelAuthor.Content = image.Author;
            LabelAuthor.Background = new SolidColorBrush(ToColor(image.MainColor));
            Task.Factory.StartNew(() =>
            {
                _image.DownLoad();
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ProgressBarStatus.Visibility = System.Windows.Visibility.Hidden;
                }));
            });
        }
        public System.Windows.Media.Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", "CC");
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new System.Windows.Media.Color
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }
    }
}
