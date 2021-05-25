using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;
using Newtonsoft.Json;

namespace BlockchainDemonstratorApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FactorsController : ControllerBase
	{
		private readonly BeerGameContext _context;

		public FactorsController(BeerGameContext context)
		{
			_context = context;
		}

		// GET: api/Factors
		[HttpGet]
		public async Task<ActionResult<object>> GetFactors()
		{
			return new
			{
				defaultFactors = await _context.Factors.FirstAsync(f => f.Id == "DefaultFactors"),
				options = await _context.Options.ToListAsync(),
				roles = await _context.Roles.ToListAsync()
			};
		}

		// GET: api/Factors/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Factors>> GetFactors(string id)
		{
			var factors = await _context.Factors.FindAsync(id);

			if (factors == null)
			{
				return NotFound();
			}

			return factors;
		}

		// PUT: api/Factors/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFactors(string id, Factors factors)
		{
			if (id != factors.Id)
			{
				return BadRequest();
			}

			_context.Entry(factors).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FactorsExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Factors
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Factors>> PostFactors(Factors factors)
		{
			_context.Factors.Add(factors);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				if (FactorsExists(factors.Id))
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtAction("GetFactors", new {id = factors.Id}, factors);
		}

		// DELETE: api/Factors/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Factors>> DeleteFactors(string id)
		{
			var factors = await _context.Factors.FindAsync(id);
			if (factors == null)
			{
				return NotFound();
			}

			_context.Factors.Remove(factors);
			await _context.SaveChangesAsync();

			return factors;
		}

		[HttpPost("EditOption")]
		public async Task<IActionResult> EditOptions(dynamic data)
		{
			string roleId = Convert.ToString(data.roleId);
			string optionName = Convert.ToString(data.optionName);
			double costStartup = Convert.ToDouble(data.costStartup);
			double costMaintenance = Convert.ToDouble(data.costMaintenance);
			double transportOneTrip = Convert.ToDouble(data.transportOneTrip);
			double transportPerDay = Convert.ToDouble(data.transportPerDay);
			double leadTime = Convert.ToDouble(data.leadTime);
			double flexibility = Convert.ToDouble(data.flexibility);
			double penalty = Convert.ToDouble(data.penalty);

			if ( string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(optionName)) return BadRequest();
			var option = _context.Options.FirstOrDefault(x => x.RoleId == roleId && x.Name == optionName);
			if (option == null)
			{
				return NotFound();
			}

			option.CostOfStartUp = costStartup;
			option.CostOfMaintenance = costMaintenance;
			option.TransportCostOneTrip = transportOneTrip;
			option.TransportCostPerDay = transportPerDay;
			option.LeadTime = leadTime;
			option.Flexibility = flexibility;
			option.GuaranteedCapacityPenalty = penalty;

			_context.Options.Update(option);
			_context.SaveChanges();
			return Ok();
		}

		private bool FactorsExists(string id)
		{
			return _context.Factors.Any(e => e.Id == id);
		}
	}
}