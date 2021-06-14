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
        public IActionResult Index(string gameId, string playerId)
        {
            if (gameId == null)
            {
                if (Request.Cookies["JoinedGame"] != null) gameId = Request.Cookies["JoinedGame"];
                else return RedirectToAction("Index","Home");
            }
            else SetCookie("JoinedGame", gameId, 480);

            if(playerId == null)
            {
                if (Request.Cookies["PlayerId"] != null) playerId = Request.Cookies["PlayerId"];
                else return RedirectToAction("Index", "Home");
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
                        ViewData["ThirdPhaseNotReady"] = false;
                        ViewData["Players"] = game.Players;
                        if (game.Players.Any(p => p.ChosenOption == null) && game.CurrentDay == Factors.RoundIncrement * 16 + 1){
                            ViewData["ThirdPhaseNotReady"] = true;
                            Dictionary<string, string> chosenOptionsPlayers = new Dictionary<string, string>();
                            //TODO: use dictionary in cshtml page ?
                        }
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

        public IActionResult ChooseRoleAndName(string gameId)
        {
            ViewData["RestApiUrl"] = Config.RestApiUrl;
            ViewData["GameId"] = gameId;
            return View();
        }

        public IActionResult EndGame(string gameId, string playerId)
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
                        RemoveCookie("PlayerId");
                        RemoveCookie("JoinedGame");
                        Game game = JsonConvert.DeserializeObject<Game>(responseString);
                        ViewData["Player"] = game.Players.FirstOrDefault(p => p.Id == playerId);
                        ViewData["RestApiUrl"] = Config.RestApiUrl;
                        ViewData["GameId"] = gameId;
                        return View(game);
                    }
                }
            }
            return BadRequest();
        }

        private void SetCookie(string key, string value, int? expireTime)

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

        private void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
