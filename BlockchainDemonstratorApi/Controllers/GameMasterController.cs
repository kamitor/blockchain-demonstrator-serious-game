using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;

namespace BlockchainDemonstratorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameMasterController : ControllerBase
    {
        private readonly BeerGameContext _context;

        public GameMasterController(BeerGameContext context)
        {
            _context = context;
        }

        // POST: api/GameMaster/GetGames
        [HttpPost("GetGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames([FromBody] string gameMasterId)
        {
            return await _context.Games.Where(g => g.GameMasterId == gameMasterId).ToListAsync();
        }

        [HttpPost("CreateGameMaster")]
        public ActionResult<GameMaster> CreateGameMaster()
        {
            GameMaster gameMaster = new GameMaster() { Id = GetUniqueId() };
            _context.GameMasters.Add(gameMaster);
            _context.SaveChanges();
            return gameMaster;
        }

        private string GetUniqueId()
        {
            List<string> usedIds = _context.GameMasters.Select(g => g.Id).ToList();

            Random r = new Random();
            while (true)
            {
                int id = r.Next(100000, 1000000);

                if (!usedIds.Contains(id.ToString()))
                {
                    return id.ToString();
                }
            }
        }


        // GET: api/GameMaster
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameMaster>>> GetGameMasters()
        {
            return await _context.GameMasters.ToListAsync();
        }

        // POST: api/BeerGame/GetGameMaster
        [HttpPost("GetGameMaster")]
        public ActionResult<GameMaster> GetGameMaster([FromBody] string gameMasterId)
        {
            if (gameMasterId == "") return BadRequest();

            var gameMaster = _context.GameMasters.FirstOrDefault(g => g.Id == gameMasterId);

            if (gameMaster == null)
            {
                return NotFound();
            }
            return gameMaster;
        }

        // DELETE: api/GameMaster/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameMaster>> DeleteGameMaster(string id)
        {
            var gameMaster = await _context.GameMasters.FindAsync(id);
            if (gameMaster == null)
            {
                return NotFound();
            }

            _context.GameMasters.Remove(gameMaster);
            await _context.SaveChangesAsync();

            return gameMaster;
        }

        [HttpPost("GameMasterExists")]
        public ActionResult<bool> GameMasterExists([FromBody] string id)
        {
            return GameMasterExistsFunc(id);
        }

        private bool GameMasterExistsFunc(string id)
        {
            return _context.GameMasters.Any(e => e.Id == id);
        }
    }
}
