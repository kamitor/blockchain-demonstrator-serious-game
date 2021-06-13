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
    [AuthorityCookie("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BeerGame(string gameId)
        {
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
                        Game game = JsonConvert.DeserializeObject<Game>(responseString);
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        return View(game);
                    }
                }
            }

            return BadRequest();
        }
    }
}
