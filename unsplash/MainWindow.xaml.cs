using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Url =
            "https://unsplash.com/grid/filter?category[2]={0}&category[3]={1}&category[4]={2}&category[6]={3}&category[7]={4}&category[8]={5}&page={6}&scope[featured]=1&search[keyword]=&utf8=%E2%9C%93";
        public MainWindow()
        {
            InitializeComponent();
            GetData(1);
        }

        void GetData(int pageIndex)
        {
            ProgressBarStatus.Visibility = Visibility.Visible;
            Task.Factory.StartNew((index) =>
            {
                WebClient client = new WebClient { Encoding = Encoding.UTF8 };
                string html = client.DownloadString(GetBuildings((int)index));
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNodeCollection imageInfos = doc.DocumentNode.SelectNodes("//div[@class=\"js-pagination-container\"]/div");
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ProgressBarStatus.Visibility = Visibility.Hidden;
                }));
                foreach (HtmlNode item in imageInfos)
                {
                    SplashImage splashImage = new SplashImage(item);
                    this.Dispatcher.BeginInvoke(new Action<SplashImage>(m =>
                    {
                        ImageItem imageItem = new ImageItem(splashImage) { Width = 250, Height = 200 };
                        WrapPanelImages.Children.Add(imageItem);
                    }), splashImage);
                }
            }, pageIndex);
        }

        string GetBuildings(int pageIndex)
        {
            return string.Format(Url, 1, 0, 0, 0, 0, 0, pageIndex);
        }
        string GetFoodAndDrink(int pageIndex)
        {
            return string.Format(Url, 0, 1, 0, 0, 0, 0, pageIndex);
        }
        string GetNature(int pageIndex)
        {
            return string.Format(Url, 0, 0, 1, 0, 0, 0, pageIndex);
        }
        string GetObjects(int pageIndex)
        {
            return string.Format(Url, 0, 0, 0, 1, 0, 0, pageIndex);
        }
        string GetPeople(int pageIndex)
        {
            return string.Format(Url, 0, 0, 0, 0, 1, 0, pageIndex);
        }
        string GetTechnology(int pageIndex)
        {
            return string.Format(Url, 0, 0, 0, 0, 0, 1, pageIndex);
        }
    }
}
