using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrapingDemo.Amazon;
using ScrapingDemo.Models;

namespace ScrapingDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string searchTerm)
        {
            var viewModel = new IndexViewModel();
            viewModel.SearchTerm = searchTerm;
            if (!String.IsNullOrEmpty(searchTerm))
            {
                viewModel.Items = AmazonScraper.Search(searchTerm);
            }
            return View(viewModel);
        }
    }
}