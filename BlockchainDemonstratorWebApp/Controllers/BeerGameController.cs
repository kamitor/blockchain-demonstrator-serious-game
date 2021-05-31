using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Blockchain_Demonstrator_Web_App.Models;
using Microsoft.AspNetCore.Http;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    public class BeerGameController : Controller
    {
        public IActionResult Index()
        {
            ViewData["RestApiUrl"] = Config.RestApiUrl;
            return View();
        }
        
        public IActionResult GameView(string gameId)
        {
            SetCookie("JoinedGame", gameId, 480);
            
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(gameId), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/GetGame",stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null) 
                    {
                        Game game = JsonConvert.DeserializeObject<Game>(responseString);
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        /*if (game.Retailer != null) game.Retailer.OrderHistory = GetOrdersFromPlayer(game.Retailer.Id);
                        if (game.Manufacturer != null) game.Manufacturer.OrderHistory = GetOrdersFromPlayer(game.Manufacturer.Id);
                        if (game.Processor != null) game.Processor.OrderHistory = GetOrdersFromPlayer(game.Processor.Id);
                        if (game.Farmer != null) game.Farmer.OrderHistory = GetOrdersFromPlayer(game.Farmer.Id);*/
                        return View(game);
                    }
                }
            }

            return BadRequest();
        }

        public IActionResult BeerGame(string gameId, string playerId)
        {
            if (gameId == null)
            {
                if (Request.Cookies["JoinedGame"] != null) gameId = Request.Cookies["JoinedGame"];
                else throw new ArgumentNullException("gameId as well as JoinedGame cookie is null");
            }
            else SetCookie("JoinedGame", gameId, 480);

            if(playerId == null)
            {
                if (Request.Cookies["PlayerId"] != null) playerId = Request.Cookies["PlayerId"];
                else throw new ArgumentNullException("playerId as well as PlayerId cookie is null");
            }
            else SetCookie("PlayerId", playerId, 480);
            
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(gameId), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/GetGame",stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null) 
                    {
                        Game game = JsonConvert.DeserializeObject<Game>(responseString);

                        Player player = game.Players.Find(x => string.Equals(x.Id, playerId));
                        ViewData["CurrentDay"] = game.CurrentDay;
                        ViewData["GameId"] = game.Id;
                        ViewData["GameReady"] = game.Players.Count == 4;
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        return View(player);
                    }
                }
            }
            return BadRequest();
        }

        public IActionResult CreateGame()
        {
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/CreateGame", null).Result;

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        public IActionResult JoinGame(string gameId, RoleType role, string name)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(new { gameId, role, name }), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/JoinGame", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GameView", "BeerGame", new { gameid = gameId });
                }
            }
            return BadRequest();
        }

        public IActionResult GamePinView()
        {
            ViewData["RestApiUrl"] = Config.RestApiUrl;
            return View();
        }

        public IActionResult GameMaster()
        {
            return View();
        }

        public IActionResult Graphs()
        {
            return View();
        }

        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTimeOffset.Now.AddMinutes(expireTime.Value);
            }
            else
            {
                option.Expires = DateTimeOffset.Now.AddMilliseconds(10);
            }
            
            Response.Cookies.Append(key, value, option);
        }

        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

        public List<Order> GetOrdersFromPlayer(string playerId)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(playerId), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/GetOrders", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null) return JsonConvert.DeserializeObject<List<Order>>(responseString);
                }
            }
            return null;
        } 
    }
}
