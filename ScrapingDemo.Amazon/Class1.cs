using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;

namespace ScrapingDemo.Amazon
{

    public class AmazonItem
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public double? Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrime { get; set; }
    }


    public static class AmazonScraper
    {
        public static IEnumerable<AmazonItem> Search(string searchTerm)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.UserAgent] = "Amazon is terrible";
                string html = client.DownloadString($"https://www.amazon.com/s/?field-keywords={searchTerm}");
                return ParseAmazonHtml(html);
            }
        }

        private static IEnumerable<AmazonItem> ParseAmazonHtml(string html)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            IEnumerable<IElement> elements =
                document.QuerySelectorAll("li.s-result-item");

            List<AmazonItem> items = new List<AmazonItem>();
            foreach (var li in elements)
            {
                var item = new AmazonItem();
                var anchorTag = li.QuerySelector("a.a-link-normal.s-access-detail-page.s-color-twister-title-link.a-text-normal");
                item.Title = anchorTag.TextContent;
                item.Link = anchorTag.GetAttribute("href");

                var priceSpan = li.QuerySelector("span.sx-price.sx-price-large");
                if (priceSpan != null)
                {
                     var priceString =  
                        priceSpan.TextContent.Replace("\n", "")
                        .Replace("\t", "").Replace(" ", "").Replace("$", "");
                    double price = double.Parse(priceString) * .01;
                    item.Price = price;
                }


                var image = li.QuerySelector("img.s-access-image");
                item.ImageUrl = image.GetAttribute("src");
                var primeIcon = li.QuerySelector("i.a-icon-prime");
                item.IsPrime = primeIcon != null;
                items.Add(item);
            }

            return items;
        }
    }
}
