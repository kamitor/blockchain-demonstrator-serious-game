﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    public class GameMasterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
