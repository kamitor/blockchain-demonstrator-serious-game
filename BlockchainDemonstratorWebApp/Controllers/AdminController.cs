using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blockchain_Demonstrator_Web_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    [AuthorityCookie("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
