using System;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;
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
            
            game.Retailer.CurrentOrder = new Order() {Volume = 12};
            game.Manufacturer.CurrentOrder = new Order() {Volume = 12};
            game.Processor.CurrentOrder = new Order() {Volume = 12};
            
            if (player != null)
                player.CurrentOrder = new Order(){Volume = Convert.ToInt32(volume)};
            
            if (game.Players.All(x => x.CurrentOrder != null))
            {
                game.Progress();
                await Clients.Group(gameId).SendAsync("UpdateGame", JsonConvert.SerializeObject(game));
                if (game.CurrentDay == Factors.RoundIncrement * 10 + 1) await PromptOptions(gameId);
            }
            
            _context.Games.Update(game);
            _context.SaveChanges();
        }

        public Task JoinGroup(string gameId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId);
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
    }
}