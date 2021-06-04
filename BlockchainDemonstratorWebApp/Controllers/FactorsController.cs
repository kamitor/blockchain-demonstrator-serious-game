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
using Newtonsoft.Json;

namespace Blockchain_Demonstrator_Web_App.Controllers
{
	public class FactorsController : Controller
	{
		public FactorsController()
		{
		}

		// GET: Factors
		public async Task<IActionResult> Index()
		{
			using (var client = new HttpClient())
			{
				//ello
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

		// GET: Factors/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync(Config.RestApiUrl + "/api/Factors/" + id).Result;

				if (response.IsSuccessStatusCode)
				{
					var responseContent = response.Content;
					var responseString = responseContent.ReadAsStringAsync().Result;
					if (responseString != null) return View(JsonConvert.DeserializeObject<Factors>(responseString));
				}
			}

			return BadRequest();
		}

		// POST: Factors/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id,
			[Bind(
				"Id,test,retailTransport,manuTransport,procTransport,farmerTransport,holdingFactor,roundIncrement,retailProductPrice,manuProductPrice,procProductPrice,farmerProductPrice,harvesterProductPrice,setupCost,initialCapital")]
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
				
				var response = client.PostAsync(Config.RestApiUrl + "/api/Factors/EditOption", stringContent).Result;
				
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index", "Factors");
				}
			}

			return BadRequest();
		}
	}
}