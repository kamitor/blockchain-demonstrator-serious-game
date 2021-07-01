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
    /// The AdminController is used for all the front-end functionalities and views for the web application.
    /// </summary>
    [AuthorityCookie("Admin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// This function redirects to the index view of the Admin, also known as the game list view.
        /// This view can be filtered by a given game master ID.
        /// </summary>
        /// <param name="gameMasterId">Filter by optional game master ID.</param>
        public IActionResult Index(string gameMasterId = null)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = null;
                if (gameMasterId != null)
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(gameMasterId), System.Text.Encoding.UTF8, "application/json");
                    response = client.PostAsync(Config.RestApiUrl + "/api/GameMaster/GetGames", stringContent).Result;
                }
                else
                {
                    response = client.GetAsync(Config.RestApiUrl + "/api/BeerGame").Result;
                }


                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        ViewData["GameMasterId"] = (gameMasterId != null) ? gameMasterId : "";
                        return View(JsonConvert.DeserializeObject<List<Game>>(responseString));
                    }
                }
            }
            return StatusCode(500);
        }

        /// <summary>
        /// This function redirects to the old beer game view, which can be used to play games by yourself.
        /// </summary>
        /// <param name="gameId">ID of the given game.</param>
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

        /// <summary>
        /// This function redirects tot the statical beer game view which the game master also sees.
        /// </summary>
        /// <param name="gameId">ID of the given game.</param>
        /// <returns></returns>
        public IActionResult ViewGame(string gameId)
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

        /// <summary>
        /// This function redirects to the edit game view.
        /// </summary>
        /// <param name="gameId">ID of the given game.</param>
        public IActionResult EditGame(string gameId)
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

        /// <summary>
        /// This function sends the edited game through to the REST API.
        /// </summary>
        /// <param name="Id">ID of the edited game.</param>
        /// <param name="CurrentDay">Current day of the game.</param>
        /// <param name="GameMasterId">Game master ID coupled with the game.</param>
        /// <param name="GameStarted">Boolean whether the game has started.</param>
        /// <param name="remove_Retailer">Boolean whether to kick the retailer from the game.</param>
        /// <param name="remove_Manufacturer">Boolean whether to kick the manufacturer from the game.</param>
        /// <param name="remove_Processor">Boolean whether to kick the processor from the game.</param>
        /// <param name="remove_Farmer">Boolean whether to kick the farmer from the game.</param>
        public IActionResult PutGame(string Id, int CurrentDay, string GameMasterId, bool GameStarted, bool remove_Retailer, bool remove_Manufacturer, bool remove_Processor, bool remove_Farmer)
        {
            if (Id == null) return StatusCode(400);
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(new { Id = Id, CurrentDay = CurrentDay, GameMasterId = GameMasterId, GameStarted = GameStarted, remove_Retailer = remove_Retailer, remove_Manufacturer = remove_Manufacturer, remove_Processor = remove_Processor, remove_Farmer = remove_Farmer }), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/EditGame", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            return StatusCode(500);
        }

        /// <summary>
        /// This function redirects to the delete game view.
        /// </summary>
        /// <param name="gameId">ID of the given game.</param>
        public IActionResult DeleteGame(string gameId)
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

        /// <summary>
        /// This function sends the confirmation to the REST API to delete the game.
        /// </summary>
        /// <param name="gameId">ID of the deleted game.</param>
        public IActionResult ConfirmDeleteGame(string gameId)
        {
            if (gameId == null) return StatusCode(400);
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync(Config.RestApiUrl + "/api/BeerGame/" + gameId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            return StatusCode(500);
        }

        /// <summary>
        /// This function redirects to the game master list view.
        /// </summary>
        public IActionResult GameMaster()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(Config.RestApiUrl + "/api/GameMaster").Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        return View(JsonConvert.DeserializeObject<List<GameMaster>>(responseString));
                    }
                }
            }
            return StatusCode(500);
        }

        /// <summary>
        /// This function redirects to the delete game master view.
        /// </summary>
        /// <param name="gameMasterId"></param>
        /// <returns></returns>
        public IActionResult DeleteGameMaster(string gameMasterId)
        {
            if (gameMasterId == null) return StatusCode(400);
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(gameMasterId), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/GameMaster/GetGameMaster", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        return View(JsonConvert.DeserializeObject<GameMaster>(responseString));
                    }
                }
            }
            return StatusCode(500);
        }

        /// <summary>
        /// This function sends the confirmation to the REST API to delete the game master.
        /// </summary>
        /// <param name="gameMasterId">ID of the deleted game master</param>
        /// <returns></returns>
        public IActionResult ConfirmDeleteGameMaster(string gameMasterId)
        {
            if (gameMasterId == null) return StatusCode(400);
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync(Config.RestApiUrl + "/api/GameMaster/" + gameMasterId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        return RedirectToAction("GameMaster", "Admin");
                    }
                }
            }
            return StatusCode(500);
        }
    }
}
