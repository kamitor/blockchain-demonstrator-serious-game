using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;
using System.Net.Http;
using Blockchain_Demonstrator_Web_App.Models;
using BlockchainDemonstratorApi.Models;
using Newtonsoft.Json;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
	/// <summary>
	/// The FactorsController is used for all the front-end factors related functionalities and views
	/// </summary>
	[AuthorityCookie("Admin")]
	public class FactorsController : Controller
	{
		/// <summary>
		/// This function redirects the admin to the index page of the factors, also known as the factors list view.
		/// </summary>
		public async Task<IActionResult> Index()
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync(Config.RestApiUrl + "/api/Factors/").Result;
				if (response.IsSuccessStatusCode)
				{
					var responseContent = response.Content;
					var responseString = responseContent.ReadAsStringAsync().Result;
					if (responseString != null)
					{
						dynamic responseObject = JsonConvert.DeserializeObject<object>(responseString);
						ViewData["Options"] = responseObject.options.ToObject<List<Option>>();
						ViewData["Roles"] = responseObject.roles.ToObject<List<Role>>();
						return View(responseObject.defaultFactors.ToObject<Factors>());
					}
				}
			}

			return BadRequest();
		}

		/// <summary>
		/// This function sends the edited factors to the REST API.
		/// </summary>
		/// <param name="id">ID of the edited factors</param>
		/// <param name="factors">Binded parameters of the factors</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id,
			[Bind(
				"Id,test,holdingFactor,roundIncrement,retailProductPrice,manuProductPrice,procProductPrice,farmerProductPrice,harvesterProductPrice,setupCost,initialCapital")]
			Factors factors)
		{
			using (var client = new HttpClient())
			{
				var stringContent = new StringContent(JsonConvert.SerializeObject(factors), System.Text.Encoding.UTF8,
					"application/json");
				var response = client.PutAsync(Config.RestApiUrl + "/api/Factors/" + id, stringContent).Result;

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index", "Factors");
				}
			}

			return BadRequest();
		}

		/// <summary>
		/// This method edits a single option.
		/// </summary>
		/// <param name="roleId">ID of the option's correlating role.</param>
		/// <param name="optionName">Name of the option.</param>
		/// <param name="costStartup">Value of the (edited) cost of startup.</param>
		/// <param name="costMaintenance">Value of the (edited) cost of maintenance.</param>
		/// <param name="transportOneTrip">Value of the (edited) transport cost of one trip.</param>
		/// <param name="transportPerDay">Value of the (edited) transport cost per day.</param>
		/// <param name="leadTime">Value of the (edited) lead time.</param>
		/// <param name="flexibility">Value of the (edited) flexibility.</param>
		/// <param name="penalty">Value of the (edited) penalty.</param>
		[HttpPost]
		public async Task<IActionResult> EditOption(string roleId, string optionName, string costStartup,
			string costMaintenance, string transportOneTrip, string transportPerDay, string leadTime,
			string flexibility, string penalty)
		{
			using (var client = new HttpClient())
			{
				var stringContent = new StringContent(
					JsonConvert.SerializeObject(new
					{
						roleId, optionName, costStartup,
						costMaintenance, transportOneTrip,
						transportPerDay, leadTime, flexibility,
						penalty
					}), System.Text.Encoding.UTF8, "application/json");
				
				var response = client.PostAsync(Config.RestApiUrl + "/api/NewFactors/EditOption", stringContent).Result;
				
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index", "Factors");
				}
			}

			return BadRequest();
		}
	}
}