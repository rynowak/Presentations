using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TheFuture.Models;

namespace TheFuture.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Endpoints([FromServices] EndpointDataSource dataSource)
        {
            var endpoints = dataSource.Endpoints;
            return View(endpoints);
        }

        public IActionResult ThrowHtml()
        {
            throw new Exception("Heyoooo");
        }

        [JsonExceptionMetadata]
        public IActionResult ThrowJson()
        {
            throw new Exception("Heyoooo");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
