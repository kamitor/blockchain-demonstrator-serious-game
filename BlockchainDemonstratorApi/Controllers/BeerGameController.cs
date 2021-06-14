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
            Game game = new Game(GetUniqueId());
            _context.Games.Add(game);
            _context.SaveChanges();
            return Ok();
        }

        /// <summary>Creates a unique id using six numbers</summary>
        /// <returns>Unique id as string</returns>
        /// <remarks>For now it returns a string later on, we might need to change that to an integer</remarks>
        private string GetUniqueId()
        {
            List<string> usedIds = _context.Games.Select(g => g.Id).ToList();

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

        [HttpPost("JoinGame")]
        public ActionResult JoinGame([FromBody] dynamic data)
        {
            if (data.gameId.Value == "" || data.role.Value == "" || data.name.Value == "") return BadRequest();
            string gameId = (string) data.gameId;
            RoleType role = (RoleType) data.role;
            string name = (string) data.name;
            string playerId = (string) data.playerId;
            
            Game game = _context.Games.Find(gameId);
            if (game == null) return NotFound();

            bool joined = false;
            try
            {
                if (role == RoleType.Retailer)
                {
                    Player player = new Player(name, playerId);
                    player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Retailer");
                    player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Retailer");
                    game.Retailer = player;
                    joined = true;
                }
                else if (role == RoleType.Manufacturer)
                {
                    Player player = new Player(name, playerId);
                    player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Manufacturer");
                    player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Manufacturer");
                    game.Manufacturer = player;
                    joined = true;
                }
                else if (role == RoleType.Processor)
                {
                    Player player = new Player(name, playerId);
                    player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Processor");
                    player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Processor");
                    game.Processor = player;
                    joined = true;
                }
                else if (role == RoleType.Farmer)
                {
                    Player player = new Player(name, playerId);
                    player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Farmer");
                    player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Farmer");
                    game.Farmer = player;
                    joined = true;
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            if (joined)
            {
                _context.Games.Update(game);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("ChooseOption")]
        public ActionResult ChooseOption([FromBody] dynamic data)
        {
            if (data.option.Value == "" || data.playerId.Value == "") return BadRequest();

            string option = (string) data.option;
            string playerId = (string) data.playerId;

            var player = _context.Players.Include(x => x.Role).FirstOrDefault(x => x.Id == playerId);
            player.ChosenOption = _context.Options.FirstOrDefault(x => x.RoleId == player.Role.Id && x.Name == option);
            player.Payments.Add(new Payment
            {
                Amount = player.ChosenOption.CostOfStartUp * -1,
                DueDay = Factors.RoundIncrement * 8 + 1,
                FromPlayer = false,
                PlayerId = player.Id,
                Topic = "Setup " + player.ChosenOption.Name
            });
            _context.Players.Update(player);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("LeaveGame")]
        public void LeaveGame()
        {
            //TODO: add leave game method
        }

        // GET: api/BeerGame
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            return await _context.Games
                .Include(g => g.Retailer).ThenInclude(p => p.Role)
                .Include(g => g.Manufacturer).ThenInclude(p => p.Role)
                .Include(g => g.Processor).ThenInclude(p => p.Role)
                .Include(g => g.Farmer).ThenInclude(p => p.Role)
                .ToListAsync();
        }

        // POST: api/BeerGame/GetGame
        [HttpPost("GetGame")]
        public ActionResult<Game> GetGame([FromBody] string gameId)
        {
            if (gameId == "") return BadRequest();

            var game = _context.Games.FirstOrDefault(game => game.Id == gameId);

            if (game == null)
            {
                return NotFound();
            }
            return game;
        }

        // POST: api/BeerGame/SendOrders
        [HttpPost("SendOrders")]
        public ActionResult<Game> SendOrders([FromBody] dynamic data)
        {
            if (data.gameId.Value == "") return BadRequest();

            string gameId = data.gameId;
            var game = GetGameFromContext(gameId);

            game.Retailer.CurrentOrder = new Order() {Volume = (data.retailerOrder.Value != "") ? Int32.Parse((string)data.retailerOrder) : 0 };
            game.Manufacturer.CurrentOrder = new Order() {Volume = (data.manufacturerOrder.Value != "") ? Int32.Parse((string)data.manufacturerOrder) : 0 };
            game.Processor.CurrentOrder = new Order() {Volume = (data.processorOrder.Value != "") ? Int32.Parse((string)data.processorOrder) : 0 };
            game.Farmer.CurrentOrder = new Order() {Volume = (data.farmerOrder.Value != "") ? Int32.Parse((string)data.farmerOrder) : 0 };

            game.Progress();
            _context.Games.Update(game);
            _context.SaveChanges();
            return game;
        }

        // POST: api/BeerGame/CheckInGame
        [HttpPost("CheckInGame")]
        public ActionResult<string> CheckInGame([FromBody] string playerId)
        {
            if (playerId == null) return BadRequest();
            return _context.Games.AsEnumerable()
                .FirstOrDefault(g => g.Players
                     .Any(p => p.Id.Equals(playerId))).Id;
        }

        // POST: api/BeerGame/Login
        [HttpPost("Login")]
        public ActionResult<dynamic> Login([FromBody] dynamic data)
        {
            if (data.id.Value == "") return BadRequest();
            string id = (string)data.id;
            string password = (string)data.password;
            bool anyGameMaster = _context.GameMasters.Any(gm => gm.Id == id);
            bool anyAdmin = _context.Admins.AsEnumerable().Any(a => a.Id == id && Cryptography.HashCompare(password, a.Password, a.Salt));
            string loggedInAs = "";
            if (anyGameMaster) loggedInAs = "GameMaster";
            if (anyAdmin) loggedInAs = "Admin";
            return new {
                loggedIn = anyGameMaster || anyAdmin,
                loggedInAs = loggedInAs
            };
        }

        [HttpPost("EditGame")]
        public ActionResult EditGame([FromBody] dynamic data)
        {
            if (data.Id.Value == "") return BadRequest();
            string id = (string)data.Id.Value;
            int currentDay = (int)data.CurrentDay.Value;
            bool gameStarted = (bool)data.GameStarted.Value;
            bool removeRetailer = (bool)data.remove_Retailer.Value;
            bool removeManufacturer = (bool)data.remove_Manufacturer.Value;
            bool removeProcessor = (bool)data.remove_Processor.Value;
            bool removeFarmer = (bool)data.remove_Farmer.Value;
            string gameMasterId = (string)data.GameMasterId.Value;

            Game game = _context.Games.FirstOrDefault(g => g.Id == id);
            game.CurrentDay = currentDay;
            game.GameStarted = gameStarted;
            game.GameMasterId = gameMasterId;
            if (removeRetailer)
            {
                _context.Orders.RemoveRange(game.Retailer.OutgoingOrders);
                _context.Orders.RemoveRange(game.Retailer.IncomingOrders);
                if (game.Retailer.CurrentOrder != null) _context.Orders.Remove(game.Retailer.CurrentOrder);
                _context.Payments.RemoveRange(game.Retailer.Payments);
                _context.Players.Remove(game.Retailer);
            }
            if (removeManufacturer)
            {
                _context.Orders.RemoveRange(game.Manufacturer.OutgoingOrders);
                _context.Orders.RemoveRange(game.Manufacturer.IncomingOrders);
                if (game.Manufacturer.CurrentOrder != null) _context.Orders.Remove(game.Manufacturer.CurrentOrder);
                _context.Payments.RemoveRange(game.Manufacturer.Payments);
                _context.Players.Remove(game.Manufacturer);
            }
            if (removeProcessor)
            {
                _context.Orders.RemoveRange(game.Processor.OutgoingOrders);
                _context.Orders.RemoveRange(game.Processor.IncomingOrders);
                if(game.Processor.CurrentOrder != null) _context.Orders.Remove(game.Processor.CurrentOrder);
                _context.Payments.RemoveRange(game.Processor.Payments);
                _context.Players.Remove(game.Processor);
            }
            if (removeFarmer)
            {
                _context.Orders.RemoveRange(game.Farmer.OutgoingOrders);
                _context.Orders.RemoveRange(game.Farmer.IncomingOrders);
                if (game.Farmer.CurrentOrder != null) _context.Orders.Remove(game.Farmer.CurrentOrder);
                _context.Payments.RemoveRange(game.Farmer.Payments);
                _context.Players.Remove(game.Farmer);
            }

            _context.Games.Update(game);
            _context.SaveChanges();
            return Ok();
        }
        
        // POST: api/BeerGame
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
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
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            foreach (Player player in game.Players) //TODO: see if deliveries get removed as well
            {
                _context.Orders.RemoveRange(player.OutgoingOrders);
                _context.Orders.RemoveRange(player.IncomingOrders);
                if (player.CurrentOrder != null) _context.Orders.Remove(player.CurrentOrder);
                _context.Payments.RemoveRange(player.Payments);
                _context.Players.Remove(player);
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        private bool GameExists(string id)
        {
            return _context.Games.Any(e => e.Id == id);
        }

        private Game GetGameFromContext(string gameId)
        {
            Game game = _context.Games.FirstOrDefault(game => game.Id == gameId);
            /*Seperated into chunks to reduce load time
            game.Retailer = _context.Games
                .Include(g => g.Retailer).ThenInclude(p => p.Role)
                .Include(g => g.Retailer).ThenInclude(p => p.CurrentOrder)
                .Include(g => g.Retailer).ThenInclude(p => p.IncomingOrders).ThenInclude(o => o.Deliveries)
                .Include(g => g.Retailer).ThenInclude(p => p.OutgoingOrders).ThenInclude(o => o.Deliveries)
                .FirstOrDefault(game => game.Id == gameId).Retailer;
            game.Manufacturer = _context.Games
                .Include(g => g.Manufacturer).ThenInclude(p => p.Role)
                .Include(g => g.Manufacturer).ThenInclude(p => p.CurrentOrder)
                .Include(g => g.Manufacturer).ThenInclude(p => p.IncomingOrders).ThenInclude(o => o.Deliveries)
                .Include(g => g.Manufacturer).ThenInclude(p => p.OutgoingOrders).ThenInclude(o => o.Deliveries)
                .FirstOrDefault(game => game.Id == gameId).Manufacturer;
            game.Processor = _context.Games
                .Include(g => g.Processor).ThenInclude(p => p.Role)
                .Include(g => g.Processor).ThenInclude(p => p.CurrentOrder)
                .Include(g => g.Processor).ThenInclude(p => p.IncomingOrders).ThenInclude(o => o.Deliveries)
                .Include(g => g.Processor).ThenInclude(p => p.OutgoingOrders).ThenInclude(o => o.Deliveries)
                .FirstOrDefault(game => game.Id == gameId).Processor;
            game.Farmer = _context.Games
                .Include(g => g.Farmer).ThenInclude(p => p.Role)
                .Include(g => g.Farmer).ThenInclude(p => p.CurrentOrder)
                .Include(g => g.Farmer).ThenInclude(p => p.IncomingOrders).ThenInclude(o => o.Deliveries)
                .Include(g => g.Farmer).ThenInclude(p => p.OutgoingOrders).ThenInclude(o => o.Deliveries)
                .FirstOrDefault(game => game.Id == gameId).Farmer;*/
            return game;
        }
    }
}
