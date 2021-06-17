using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blockchain_Demonstrator_Web_App.Models;
using BlockchainDemonstratorApi.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    [AuthorityCookie("GameMaster")]
    public class GameMasterController : Controller
    {
        private string _gameMasterId;

        public IActionResult Index()
        {
            _gameMasterId = Request.Cookies["GameMasterId"];
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(_gameMasterId), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/GameMaster/GetGames", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        return View(JsonConvert.DeserializeObject<List<Game>>(responseString));
                    }
                }
            }
            return StatusCode(500);
        }

        public IActionResult BeerGame(string gameId)
        {
            if (gameId == null) return StatusCode(400);
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(gameId), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/GetGame", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        return View(JsonConvert.DeserializeObject<Game>(responseString));
                    }
                }
            }
            return StatusCode(500);
        }
    }
}
