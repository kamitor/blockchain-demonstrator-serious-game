using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blockchain_Demonstrator_Web_App.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using BlockchainDemonstratorApi.Models.Classes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
    /// <summary>
    /// The HomeController is the default controller of the web application.
    /// This controller is used for the index page of the site, the login page and the error page.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This function redirects the user to the index page of the site, where the user can join game.
        /// </summary>
        public IActionResult Index()
        {
            ViewData["RestApiUrl"] = Config.RestApiUrl;
            return View();
        }

        /// <summary>
        /// This function redirects the user to the login view.
        /// </summary>
        /// <param name="loginFailed">The loginFailed parameter is used to indicate whether the previous login attempt failed or not.</param>
        /// <returns></returns>
        public IActionResult Login(bool loginFailed = false)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(Config.RestApiUrl + "/api/Admin/AdminExists").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null)
                    {
                        ViewData["AdminExists"] = JsonConvert.DeserializeObject<bool>(responseString);
                        ViewData["LoginFailed"] = loginFailed;
                        return View();
                    }
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// This function is used to redirect a login request to the REST API.
        /// </summary>
        /// <param name="id">The ID of the account(game master or admin).</param>
        /// <param name="password">The optional password that is used by the admin</param>
        /// <returns></returns>
        public IActionResult LoginPost(string id, string password)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(new { id, password }), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/BeerGame/Login", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(responseString);
                    bool loggedIn = (bool)data.loggedIn.Value;
                    string loggedInAs = (string)data.loggedInAs.Value;

                    if (loggedIn)
                    {
                        SetCookie(loggedInAs + "Id", id, 480);
                        return RedirectToAction("Index", loggedInAs);
                    }
                }
            }
            return RedirectToAction("Login", new { loginFailed = true });
        }

        /// <summary>
        /// This method redirects the request to create a admin account to the REST API.
        /// </summary>
        /// <param name="id">The inserted ID of the new admin.</param>
        /// <param name="password">The inserted password of the new admin.</param>
        public IActionResult CreateAdmin(string id, string password)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(new Admin() { Id = id, Password = password }), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(Config.RestApiUrl + "/api/Admin/Create", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    SetCookie("AdminId", id, 480);
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Login", true);
        }

        /// <summary>
        /// This function logs the user out of their account, by removing all the potential account cookies.
        /// It then redirects the user to the index page.
        /// </summary>
        public IActionResult Logout()
        {
            RemoveCookie("AdminId");
            RemoveCookie("GameMasterId");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// This function is used in the release environment when a error occurs.
        /// It redirects the user to a new view with a small bit of information about the error.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
