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
                var response = client.GetAsync(Config.RestApiUrl + "/api/Factors").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != null) return View(JsonConvert.DeserializeObject<List<Factors>>(responseString));
                }
            }
            return BadRequest();
        }

     //  // GET: Factors/Details/5
     //   public async Task<IActionResult> Details(string id)
     //   {
     //       if (id == null)
     //       {
     //           return NotFound();
     //       }
     //
     //       var factors = await _context.Factors
     //           .FirstOrDefaultAsync(m => m.Id == id);
     //       if (factors == null)
     //       {
     //           return NotFound();
     //       }
     //
     //       return View(factors);
     //   }
     //
     //   // GET: Factors/Create
     //   public IActionResult Create()
     //   {
     //       return View();
     //   }
     //
     //   // POST: Factors/Create
     //   // To protect from overposting attacks, enable the specific properties you want to bind to, for 
     //   // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
     //   [HttpPost]
     //   [ValidateAntiForgeryToken]
     //   public async Task<IActionResult> Create([Bind("Id,retailTransport,manuTransport,procTransport,farmerTransport,holdingFactor,roundIncrement,retailProductPrice,manuProductPrice,procProductPrice,farmerProductPrice,harvesterProductPrice,setupCost,initialCapital")] Factors factors)
     //   {
     //       if (ModelState.IsValid)
     //       {
     //           _context.Add(factors);
     //           await _context.SaveChangesAsync();
     //           return RedirectToAction(nameof(Index));
     //       }
     //       return View(factors);
     //   }

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
        public async Task<IActionResult> Edit(string id, [Bind("Id,retailTransport,manuTransport,procTransport,farmerTransport,holdingFactor,roundIncrement,retailProductPrice,manuProductPrice,procProductPrice,farmerProductPrice,harvesterProductPrice,setupCost,initialCapital")] Factors factors)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(factors), System.Text.Encoding.UTF8, "application/json");
                var response = client.PutAsync(Config.RestApiUrl + "/api/Factors/" + id, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Factors");
                }
            }
            return BadRequest();
           
        }

     //   // GET: Factors/Delete/5
     //   public async Task<IActionResult> Delete(string id)
     //   {
     //       if (id == null)
     //       {
     //           return NotFound();
     //       }
     //
     //       var factors = await _context.Factors
     //           .FirstOrDefaultAsync(m => m.Id == id);
     //       if (factors == null)
     //       {
     //           return NotFound();
     //       }
     //
     //       return View(factors);
     //   }
     //
     //   // POST: Factors/Delete/5
     //   [HttpPost, ActionName("Delete")]
     //   [ValidateAntiForgeryToken]
     //   public async Task<IActionResult> DeleteConfirmed(string id)
     //   {
     //       var factors = await _context.Factors.FindAsync(id);
     //       _context.Factors.Remove(factors);
     //       await _context.SaveChangesAsync();
     //       return RedirectToAction(nameof(Index));
     //   }
     //
     //   private bool FactorsExists(string id)
     //   {
     //       return _context.Factors.Any(e => e.Id == id);
     //   }
    }
}
