using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrapingDemo.Amazon;

namespace ScrapingDemo.Models
{
    public class IndexViewModel
    {
        public IEnumerable<AmazonItem> Items { get; set; }
        public string SearchTerm { get; set; }
    }
}