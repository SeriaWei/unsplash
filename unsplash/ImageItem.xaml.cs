using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ImageItem(SplashImage image)
        {
            InitializeComponent();
            this.ImageThumbnail.Source = new BitmapImage(new Uri(image.UrlThubmnail));
            LabelAuthor.Content = image.Author;
            LabelAuthor.Background = new SolidColorBrush(ToColor(image.MainColor));
        }
        public Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", "CC");
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new Color
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }
    }
}
