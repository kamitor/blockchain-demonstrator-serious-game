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

        public IActionResult Login()
        {
            ViewData["RestApiUrl"] = Config.RestApiUrl;
            return View();
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
                    bool loggedIn = (bool)data.loggedIn.value;
                    string loggedInAs = (string)data.loggedInAs.value;
                    
                    if (loggedIn)
                    {
                        SetCookie(loggedInAs + "Id", id, 480);
                        return RedirectToAction("Index", loggedInAs);
                    }
                }
            }
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
