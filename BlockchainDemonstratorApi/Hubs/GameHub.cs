using System;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlockchainDemonstratorApi.Hubs
{
    public class GameHub : Hub
    {
        //TODO: setup heartbeat
        private readonly BeerGameContext _context;

        public GameHub(BeerGameContext context)
        {
            _context = context;
        }

        public async Task SendOrder(string volume, string gameId, string playerId)
        {
            Game game = _context.Games.FirstOrDefault(x => x.Id.Equals(gameId));
            
            if (game == null) return;

            Player player = game.Players.FirstOrDefault(x => x.Id.Equals(playerId));
            
            //TODO: for testing purposes
            /*if (game.Retailer == null) game.Retailer = new Player("Rtest");
            if (game.Manufacturer == null) game.Manufacturer = new Player("Mtest");
            if (game.Processor == null) game.Processor = new Player("Ptest");*/
            
            /*game.Retailer.CurrentOrder = new Order() {Volume = 12};
            game.Manufacturer.CurrentOrder = new Order() {Volume = 12};
            game.Processor.CurrentOrder = new Order() {Volume = 12};*/
            
            if (player != null)
                player.CurrentOrder = new Order(){Volume = Convert.ToInt32(volume)};
            
            if (game.Players.All(x => x.CurrentOrder != null))
            {
                game.Progress();
                await Clients.Group(gameId).SendAsync("UpdateGame", JsonConvert.SerializeObject(game));
                if (game.CurrentDay == Factors.RoundIncrement * 8 + 1) await PromptOptions(gameId);
            }
            
            _context.Games.Update(game);
            _context.SaveChanges();
        }

        public Task JoinGroup(string gameId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task JoinGame(string gameId, string role, string name, string playerId)
        {
            Game game = _context.Games.Find(gameId);
            if (game != null)
            {
                bool joined = false;
                try
                {
                    if (role == "Retailer")
                    {
                        Player player = new Player(name, playerId);
                        player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Retailer");
                        player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Retailer");
                        game.Retailer = player;
                        joined = true;
                    }
                    else if (role == "Manufacturer")
                    {
                        Player player = new Player(name, playerId);
                        player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Manufacturer");
                        player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Manufacturer");
                        game.Manufacturer = player;
                        joined = true;
                    }
                    else if (role == "Processor")
                    {
                        Player player = new Player(name, playerId);
                        player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Processor");
                        player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Processor");
                        game.Processor = player;
                        joined = true;
                    }
                    else if (role == "Farmer")
                    {
                        Player player = new Player(name, playerId);
                        player.Role = _context.Roles.FirstOrDefault(r => r.Id == "Farmer");
                        player.ChosenOption = _context.Options.FirstOrDefault(o => o.Name == "Basic" && o.RoleId == "Farmer");
                        game.Farmer = player;
                        joined = true;
                    }
                }
                catch (ArgumentException){ }

                if (joined)
                {
                    if(game.Players.Count == 4)
                    {
                        game.SetupGame();
                        await Clients.Group(gameId).SendAsync("ShowGame", game);
                    }
                    _context.Games.Update(game);
                    _context.SaveChanges();
                }

            }
        }

        public async Task UpdateGame(string gameId)
        {
            await Clients.Group(gameId)
                .SendAsync("UpdateGame", JsonConvert.SerializeObject(_context.Games.FirstOrDefault(x => x.Id.Equals(gameId))) );
        }

        public async Task PromptOptions(string gameId)
        {
            await Clients.Group(gameId).SendAsync("PromptOptions");
        }

        public async Task ChooseOption(string playerId, string option)
        {
            var player = _context.Players.FirstOrDefault(x => x.Id == playerId);
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
            string gameId = _context.Games.FirstOrDefault(g => g.Retailer.Id == playerId ||
                                                            g.Manufacturer.Id == playerId ||
                                                            g.Processor.Id == playerId ||
                                                            g.Farmer.Id == playerId).Id;
            await Clients.Group(gameId).SendAsync("UpdateGame", JsonConvert.SerializeObject(_context.Games.FirstOrDefault(x => x.Id.Equals(gameId))));
        }
    }
}