using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Drawing;

namespace unsplash
{
    public class SplashImage
    {
        public SplashImage() { }
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
            if (UrlBase == null) 
                return string.Empty;
            return UrlBase + string.Format("?fit=crop&fm=jpg&h={0}&q={2}&w={1}", height, width, quality);
        }
        public string DownLoadUrl { get; set; }
        public string Author { get; set; }

        public string Category { get; set; }
        public string FileName
        {
            get
            {
                if (UrlBase == null)
                    return string.Empty;
                return System.IO.Path.GetFileName(UrlBase + ".jpg");
            }
        }
        public string ThumbnailFileName
        {
            get
            {
                if (FileName == null)
                    return string.Empty;
                return "Thumbnail/" + FileName;
            }
        }
        public string DownLoadThumbnail()
        {
            string thumbnailFile = AppDomain.CurrentDomain.BaseDirectory + this.Category + "\\" + this.ThumbnailFileName;
            if (!File.Exists(thumbnailFile))
            {
                byte[] imageBuffer = GetBuffer(this.UrlThubmnail);
                using (MemoryStream ms = new MemoryStream(imageBuffer))
                {
                    Bitmap bitmap = new Bitmap(ms);
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(thumbnailFile)))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(thumbnailFile));
                    }
                    bitmap.Save(thumbnailFile);
                    bitmap.Dispose();
                }
            }
            return thumbnailFile;
        }
        public void DownLoad()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + this.Category + "\\" + this.FileName;
            if (!File.Exists(file))
            {
                byte[] imageBuffer = GetBuffer(this.GetUrlCrop(1920, 1080));
                using (MemoryStream ms = new MemoryStream(imageBuffer))
                {
                    Bitmap bitmap = new Bitmap(ms);
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(file)))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(file));
                    }
                    bitmap.Save(file);
                    bitmap.Dispose();
                }
            }
        }
        byte[] GetBuffer(string url)
        {
            try
            {
                WebClient client = new WebClient();
                return client.DownloadData(url);
            }
            catch
            {
                return GetBuffer(url);
            }
        }
    }
}
