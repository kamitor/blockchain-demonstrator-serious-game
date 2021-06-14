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
                        return View(JsonConvert.DeserializeObject<List<Game>>(responseString));
                    }
                }
            }
            return StatusCode(500);
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

        public IActionResult ConfirmDeleteGameMaster(string gameId)
        {
            if (gameId == null) return StatusCode(400);
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync(Config.RestApiUrl + "/api/GameMaster/" + gameId).Result;

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
