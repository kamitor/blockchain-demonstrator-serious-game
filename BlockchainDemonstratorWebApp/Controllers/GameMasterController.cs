using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blockchain_Demonstrator_Web_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    //TODO: uncomment this when you are able to login as a game master [AuthorityCookie("GameMaster")]
    public class GameMasterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GameMasterView()
        {
            return View();
        }
    }
}
