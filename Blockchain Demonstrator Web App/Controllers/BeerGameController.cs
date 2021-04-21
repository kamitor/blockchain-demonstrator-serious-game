using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using Newtonsoft.Json;
using System.Net.Http;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    public class BeerGameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult GameView(string gameId)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(gameId), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync("http://localhost:56404/api/BeerGame/GetGame",stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString =  responseContent.ReadAsStringAsync().Result;
                    if (responseString != null) return View(JsonConvert.DeserializeObject<Game>(responseString));
                }
            }

            return BadRequest();
        }

        public IActionResult CreateGame()
        {
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"http://localhost:56404/api/BeerGame/CreateGame", null).Result; //TODO: Make static baseURL later

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        public IActionResult JoinGame(string gameId, Role role, string name)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(new { gameId, role, name }), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync($"http://localhost:56404/api/BeerGame/JoinGame", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GameView", "BeerGame", new { gameid = gameId });
                }
            }
            return BadRequest();
        }
    }
}
