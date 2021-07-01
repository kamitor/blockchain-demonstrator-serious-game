using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blockchain_Demonstrator_Web_App.Models;
using BlockchainDemonstratorApi.Models;
using BlockchainDemonstratorApi.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    /// <summary>
    /// The GameMasterController is used for all of the front-end game master functionalities and views.
    /// </summary>
    [AuthorityCookie("GameMaster")]
    public class GameMasterController : Controller
    {
        private string _gameMasterId;

        /// <summary>
        /// The index function of the GameMasterController is also known as the game master games list.
        /// This list contains all the games of the given game master.
        /// </summary>
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

        /// <summary>
        /// The BeerGame view of this controller is used to show statistical data of the (ongoing) game.
        /// </summary>
        /// <param name="gameId">ID of the wanted game.</param>
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
