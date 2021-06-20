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
    /// <summary>
    /// The BeerGameController is used for all the front-end beer game related functionalities and views.
    /// </summary>
    public class BeerGameController : Controller
    {
        /// <summary>
        /// This function redirects to the index page of the beer game, also known as beer game page.
        /// </summary>
        /// <param name="gameId">ID of the joined beer game.</param>
        /// <param name="playerId">ID of the joined player.</param>
        /// <remarks>The parameters can be seen as optional, as they can also be filled in by the cookies.</remarks>
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
                        }
                        return View(player);
                    }
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// This function redirects to the choose role and name view.
        /// This view is used as the intermediary join page, where the user must insert a name and choose a role.
        /// </summary>
        /// <param name="gameId">ID of the game the player wants to join.</param>
        /// <returns></returns>
        public IActionResult ChooseRoleAndName(string gameId)
        {
            ViewData["RestApiUrl"] = Config.RestApiUrl;
            ViewData["GameId"] = gameId;
            return View();
        }

        /// <summary>
        /// This function redirects to the end game page.
        /// This view is shown after a game ends, containing statistics and other data.
        /// </summary>
        /// <param name="gameId">ID of the joined game.</param>
        /// <param name="playerId">ID of the player.</param>
        /// <remarks>When the player is redirected to this function, his cookies will be removed to make him leave the game.</remarks>
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

        /// <summary>
        /// This function is used to set a cookie.
        /// </summary>
        /// <param name="key">The name/key/id of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="expireTime">The expiry time of the cookie.</param>
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
