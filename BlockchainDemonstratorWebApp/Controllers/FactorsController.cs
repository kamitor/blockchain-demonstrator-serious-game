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
				"Id,test,holdingFactor,roundIncrement,retailProductPrice,manuProductPrice,procProductPrice,farmerProductPrice,harvesterProductPrice,setupCost,initialCapital,ratioALeadtime,ratioBLeadtime,ratioCLeadtime,ratioAChance,ratioBChance,ratioCChance,flushInventoryPrice")]
			Factors factors)
		{
			using (var client = new HttpClient())
			{
				var stringContent = new StringContent(JsonConvert.SerializeObject(factors), System.Text.Encoding.UTF8,
					"application/json");
				var response = client.PutAsync(Config.RestApiUrl + "/api/Factors/" + id, stringContent).Result;

				if (response.IsSuccessStatusCode)
				{
					//this is a reason why uncommitted changes are discarded when changing tab
					return RedirectToAction("Index", "Factors");
				}
			}

			return BadRequest();
		}

		/// <summary>
		/// This function sends the edited factors to the REST API.
		/// </summary>
		/// <param name="id">ID of the edited factors</param>
		/// <param name="option">Binded parameters of the option</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditOption(string id,
			[Bind(
				"Id,Name,CostOfStartUp,CostOfMaintenance,TransportCostOneTrip,TransportCostPerDay,LeadTime,Flexibility,GuaranteedCapacityPenalty,RoleId,MinimumGuaranteedCapacity")]
			Option option)
		{
			using (var client = new HttpClient())
			{
				var stringContent = new StringContent(JsonConvert.SerializeObject(option), System.Text.Encoding.UTF8,
					"application/json");
				var response = client.PutAsync(Config.RestApiUrl + "/api/Factors/Option/" + id, stringContent).Result;

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index", "Factors");
				}
			}

			return BadRequest();
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditAll(string id,
			[Bind(
				"Id,Name,CostOfStartUp,CostOfMaintenance,TransportCostOneTrip,TransportCostPerDay,LeadTime,Flexibility,GuaranteedCapacityPenalty,RoleId,MinimumGuaranteedCapacity")]
			List<Option> option)
		{
			using (var client = new HttpClient())
			{
				var stringContent = new StringContent(JsonConvert.SerializeObject(option), System.Text.Encoding.UTF8,
					"application/json");
				var response = client.PutAsync(Config.RestApiUrl + "/api/Factors/Option/" + id, stringContent).Result;

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index", "Factors");
				}
			}

			return BadRequest();
		}

		[HttpGet]
		public IActionResult Simulation(string chosenOption)
		{
			if (chosenOption != "")
			{
				using (var client = new HttpClient())
				{
					var response = client.GetAsync(Config.RestApiUrl + $"/api/Factors/{chosenOption}")
						.Result;

					if (response.IsSuccessStatusCode)
					{
						var responseContent = response.Content;
						string responseString = responseContent.ReadAsStringAsync().Result;
						if (responseString != null)
						{
							return View(JsonConvert.DeserializeObject<Game>(responseString));
						}
					}
				}
			}

			return View();
		}
	}
}