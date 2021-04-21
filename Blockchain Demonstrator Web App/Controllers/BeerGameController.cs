using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using Newtonsoft.Json;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    public class BeerGameController : Controller
    {
        public Game gm = new Game();
        public void Init()
        {
           /* gm.Players.Add(Role.Farmer, new Player("Farmer", new Farmer()));
            gm.Players.Add(Role.Retailer, new Player("Retailer", new Retailer()));
            gm.Players.Add(Role.Manufacturer, new Player("Manufacturer", new Manufacturer()));
            gm.Players.Add(Role.Processor, new Player("Processor", new Processor()));*/
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult GameView()
        {
            Init();
            return View(gm);
        }

        public string GetGame()
        {
            Init();
            string blabla = JsonConvert.SerializeObject(gm);
            return blabla;
        }
    }
}
