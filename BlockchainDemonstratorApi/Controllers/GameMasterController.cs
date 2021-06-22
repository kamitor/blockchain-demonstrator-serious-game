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
    /// <summary>
    /// This controller is used to handle back-end game master functionalities, such as creating game master or getting the games of a certain game mastser.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GameMasterController : ControllerBase
    {
        private readonly BeerGameContext _context;

        public GameMasterController(BeerGameContext context)
        {
            _context = context;
        }

        /// <summary>
        /// POST: api/GameMaster/GetGames
        /// </summary>
        [HttpPost("GetGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames([FromBody] string gameMasterId)
        {
            return await _context.Games.Where(g => g.GameMasterId == gameMasterId).ToListAsync();
        }

        /// <summary>
        /// POST: api/GameMaster/CreateGameMaster
        /// </summary>
        [HttpPost("CreateGameMaster")]
        public ActionResult<GameMaster> CreateGameMaster()
        {
            GameMaster gameMaster = new GameMaster() { Id = GetUniqueId() };
            _context.GameMasters.Add(gameMaster);
            _context.SaveChanges();
            return gameMaster;
        }


        /// <summary>
        /// GET: api/GameMaster
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameMaster>>> GetGameMasters()
        {
            return await _context.GameMasters.ToListAsync();
        }

        /// <summary>
        /// POST: api/BeerGame/GetGameMaster
        /// </summary>
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

        /// <summary>
        /// DELETE: api/GameMaster/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameMaster>> DeleteGameMaster(string id)
        {
            var gameMaster = await _context.GameMasters.FindAsync(id);
            if (gameMaster == null)
            {
                return NotFound();
            }

            for (int i = 0; i < gameMaster.Games.Count; i++)
            {
                BeerGameController.RemoveGame(gameMaster.Games[i], _context);
            }
            _context.GameMasters.Remove(gameMaster);
            _context.SaveChanges();

            return gameMaster;
        }

        /// <summary>
        /// POST: api/GameMaster/GameMasterExists
        /// </summary>
        [HttpPost("GameMasterExists")]
        public ActionResult<bool> GameMasterExists([FromBody] string id)
        {
            return GameMasterExistsFunc(id);
        }

        /// <summary>
        /// This method is used to generate a unique ID for a game master.
        /// </summary>
        /// <returns>The returned ID ranges between 100000 - 999999</returns>
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

        private bool GameMasterExistsFunc(string id)
        {
            return _context.GameMasters.Any(e => e.Id == id);
        }
    }
}
