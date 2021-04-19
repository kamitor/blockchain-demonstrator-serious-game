using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    public class BeerGameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
