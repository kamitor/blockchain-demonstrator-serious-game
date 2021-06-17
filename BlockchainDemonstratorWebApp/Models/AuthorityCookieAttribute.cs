using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models
{
    public class AuthorityCookieAttribute : ActionFilterAttribute
    {
        private string _authority;

        public AuthorityCookieAttribute(string authority)
        {
            _authority = authority;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string authorityId = filterContext.HttpContext.Request.Cookies[_authority + "Id"];
            if (authorityId != null)
            {
                using (var client = new HttpClient())
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(authorityId), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(Config.RestApiUrl + "/api/" + _authority + "/" + _authority + "Exists", stringContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        if (responseString != null)
                        {
                            if (JsonConvert.DeserializeObject<bool>(responseString)) return;
                        }
                    }
                }
            }
            filterContext.Result = new RedirectResult("/");
            return;
        }
    }
}
