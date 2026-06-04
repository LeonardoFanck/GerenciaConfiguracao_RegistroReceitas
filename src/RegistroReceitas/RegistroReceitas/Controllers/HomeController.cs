using Microsoft.AspNetCore.Mvc;
using RegistroReceitas.Helpers;
using RegistroReceitas.Models;
using System.Diagnostics;

namespace RegistroReceitas.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //if(HttpContext.Session.EstaLogado())
            if(User.EstaLogado())
                return RedirectToAction("Index", "Receitas");

            return View();
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
