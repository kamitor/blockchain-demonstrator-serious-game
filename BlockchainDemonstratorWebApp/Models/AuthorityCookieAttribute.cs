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
    /// <summary>
    /// This attribute is used to check the cookie of a request.
    /// With this attribute, a view or controller can be closed off to only a certain authority, such as the Admin or Game master.
    /// Use this attribute by calling the constructor with the authority that matches the cookie ID.
    /// </summary>
    public class AuthorityCookieAttribute : ActionFilterAttribute
    {
        private string _authority;

        public AuthorityCookieAttribute(string authority)
        {
            _authority = authority;
        }
        /// <summary>
        /// This function executes before every action, allowing each requests to be checked before being handled further.
        /// This function checks whether the request has the expected and correct cookie for the action.
        /// </summary>
        /// <param name="filterContext">The context of the action, contains information such as the request.</param>
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
