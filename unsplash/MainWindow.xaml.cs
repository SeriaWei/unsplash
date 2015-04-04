using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
        int pageIndex = 1;
        string currentCategory;
        private const string Url =
            "https://unsplash.com/grid/filter?category[2]={0}&category[3]={1}&category[4]={2}&category[6]={3}&category[7]={4}&category[8]={5}&page={6}&scope[featured]=1&search[keyword]=&utf8=%E2%9C%93";
        List<SplashImage> _allImages;
        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement item in Canvas_ButtonGroup.Children)
            {
                (item as Button).Click += (e, s) =>
                {
                    DisableButtons();
                    MethodInfo method = this.GetType().GetMethod((e as Button).Tag.ToString());
                    method.Invoke(this, new object[] { pageIndex });
                };
            }
        }
        void EnableButtons()
        {
            foreach (UIElement btn in Canvas_ButtonGroup.Children)
            {
                btn.IsEnabled = true;
            }
        }
        void DisableButtons()
        {
            foreach (UIElement btn in Canvas_ButtonGroup.Children)
            {
                btn.IsEnabled = false;
            }
        }

        void GetData(string url)
        {
            this.Title = string.Format("{0} ({1})", currentCategory, pageIndex);
            ProgressBarStatus.Visibility = Visibility.Visible;
            WrapPanelImages.Children.Clear();
            Task.Factory.StartNew((index) =>
            {
                _allImages = GetAllImageInfo();
                if (_allImages == null)
                {
                    _allImages = new List<SplashImage>();
                }
                WebClient client = new WebClient { Encoding = Encoding.UTF8 };
                string html = client.DownloadString(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNodeCollection imageInfos = doc.DocumentNode.SelectNodes("//div[@class=\"js-pagination-container\"]/div[@class=\"sheet photo-container js-packery-image-container\"]");
                bool hasChange = false;
                if (imageInfos != null)
                {
                    foreach (HtmlNode item in imageInfos)
                    {
                        SplashImage splashImage = new SplashImage(item);
                        splashImage.Category = currentCategory;
                        if (_allImages.All(m => m.UrlBase != splashImage.UrlBase))
                        {
                            //_allImages.Add(splashImage);
                            _allImages.Insert(0, splashImage);
                            hasChange = true;
                        }
                        splashImage.DownLoadThumbnail();
                        this.Dispatcher.BeginInvoke(new Action<SplashImage>(m =>
                        {
                            ImageItem imageItem = new ImageItem(splashImage) { Width = 250, Height = 200 };
                            WrapPanelImages.Children.Add(imageItem);
                        }), splashImage);
                    }
                }
                if (hasChange)
                {
                    SaveAllImageInfo(_allImages);
                }
                
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ProgressBarStatus.Visibility = Visibility.Hidden;
                    EnableButtons();
                }));
            }, pageIndex);
        }
        private int InitPageIndex(int index, string category)
        {
            if (category == currentCategory)
            {
                this.pageIndex = ++index;
            }
            else
            {
                this.pageIndex = index = 1;
            }
            return index;
        }
        public void GetBuildings(int index)
        {
            const string category = "Buildings";
            index = InitPageIndex(index, category);
            currentCategory = category;
            GetData(string.Format(Url, 1, 0, 0, 0, 0, 0, index));
        }
        public void GetFoodAndDrink(int index)
        {
            const string category = "FoodAndDrink";
            index = InitPageIndex(index, category);
            currentCategory = category;
            GetData(string.Format(Url, 0, 1, 0, 0, 0, 0, index));
        }
        public void GetNature(int index)
        {
            const string category = "Nature";
            index = InitPageIndex(index, category);
            currentCategory = category;
            GetData(string.Format(Url, 0, 0, 1, 0, 0, 0, index));
        }
        public void GetObjects(int index)
        {
            const string category = "Objects";
            index = InitPageIndex(index, category);
            currentCategory = category;
            GetData(string.Format(Url, 0, 0, 0, 1, 0, 0, index));
        }


        public void GetPeople(int index)
        {
            const string category = "People";
            index = InitPageIndex(index, category);
            currentCategory = category;
            GetData(string.Format(Url, 0, 0, 0, 0, 1, 0, index));
        }
        public void GetTechnology(int index)
        {
            const string category = "Technology";
            index = InitPageIndex(index, category);
            currentCategory = category;
            GetData(string.Format(Url, 0, 0, 0, 0, 0, 1, index));
        }

        public List<SplashImage> GetAllImageInfo()
        {
            string file = string.Format("{0}\\All.json", currentCategory);
            if (!System.IO.File.Exists(file))
            {
                System.IO.File.Create(file).Dispose();
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<SplashImage>>(System.IO.File.ReadAllText(file, Encoding.UTF8));
        }
        public void SaveAllImageInfo(List<SplashImage> objs)
        {
            string file = string.Format("{0}\\All.json", currentCategory);
            if (!System.IO.File.Exists(file))
            {
                System.IO.File.Create(file).Dispose();
            }
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(objs);
            System.IO.File.WriteAllText(file, value, Encoding.UTF8);
        }
    }
}
