using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using Newtonsoft.Json;

namespace BlockchainDemonstratorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerGameController : ControllerBase
    {
        private readonly BeerGameContext _context;

        public BeerGameController(BeerGameContext context)
        {
            _context = context;
        }

        [HttpPost("CreateGame")]
        public ActionResult CreateGame()
        {
            Game game = new Game(); //TODO: Make try catch
            _context.Game.Add(game);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("JoinGame")]
        public ActionResult JoinGame([FromBody] dynamic data)
        {
            if (data.gameId == null || data.role == null || data.name == null) return BadRequest();
            string gameId = (string) data.gameId;
            Role role = (Role) data.role;
            string name = (string) data.name;
            
            Game game = _context.Game.Find(gameId);
            if (game == null) return NotFound();

            bool joined = false;
            if (role == Role.Retailer)
            {
                game.Retailer = new Player(name);
                joined = true;
            }
            else if (role == Role.Manufacturer)
            {
                game.Manufacturer = new Player(name);
                joined = true;
            }
            else if (role == Role.Processor)
            {
                game.Processor = new Player(name);
                joined = true;
            }
            else if (role == Role.Farmer)
            {
                game.Farmer = new Player(name);
                joined = true;
            }

            if (joined)
            {
                _context.Game.Update(game);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        // GET: api/BeerGame
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            return await _context.Game
                .Include(g => g.Retailer)
                .Include(g => g.Manufacturer)
                .Include(g => g.Processor)
                .Include(g => g.Farmer)
                .ToListAsync();
        }

        // POST: api/BeerGame/GetGame
        [HttpPost("GetGame")]
        public ActionResult<Game> GetGame([FromBody] string gameId)
        {
            //string gameId = (string) data.gameId;
            var game =  _context.Game
                .Include(g => g.Retailer)
                .Include(g => g.Manufacturer)
                .Include(g => g.Processor)
                .Include(g => g.Farmer)
                .FirstOrDefault(game => game.Id == gameId);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/BeerGame/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(string id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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

        // POST: api/BeerGame
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Game.Add(game);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GameExists(game.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/BeerGame/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(string id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        private bool GameExists(string id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}
