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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["RestApiUrl"] = Config.RestApiUrl;
            return View();
        }

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

        public IActionResult Logout()
        {
            RemoveCookie("AdminId");
            RemoveCookie("GameMasterId");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
