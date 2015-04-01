using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace unsplash
{
    public class SplashImage
    {
        public SplashImage(HtmlNode node)
        {
            MainColor =
                node.SelectSingleNode("div[@class=\"photo\"]")
                    .GetAttributeValue("style", "")
                    .Replace("background-color: ", "").Replace(";", "").Trim();
            UrlThubmnail = node.SelectSingleNode("div[@class=\"photo\"]/a/img").GetAttributeValue("src", "").Replace("&amp;", "&");
            UrlBase = UrlThubmnail.Split('?')[0];
            DownLoadUrl = "https://unsplash.com" + node.SelectSingleNode("div[@class=\"photo\"]/a").GetAttributeValue("href", "");
            Author = node.SelectSingleNode("div[@class=\"text-center photo-description\"]/h2/a[2]").InnerText.Trim();
        }
        public string MainColor { get; set; }
        public string UrlBase { get; set; }
        public string UrlThubmnail { get; set; }

        public string GetUrlCrop(int width, int height, int quality = 75)
        {
            return UrlBase + string.Format("?fit=crop&fm=jpg&h={0}&q={2}&w={1}", height, width, quality);
        }
        public string DownLoadUrl { get; set; }
        public string Author { get; set; }

        public string Category { get; set; }
    }
}
