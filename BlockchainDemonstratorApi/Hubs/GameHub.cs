using System;
using System.Collections.Generic;
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

            if (player != null)
                player.CurrentOrder = new Order(){Volume = Convert.ToInt32(volume)};
            
            game.Manufacturer.CurrentOrder = new Order() { Volume = 15 };
            game.Processor.CurrentOrder = new Order() { Volume = 15 };
            game.Farmer.CurrentOrder = new Order() { Volume = 15 };

            if (game.Players.All(x => x.CurrentOrder != null))
            {
                game.Progress();
                await Clients.Group(gameId).SendAsync("UpdateGame", JsonConvert.SerializeObject(game));
                if (game.CurrentDay == Factors.RoundIncrement * 8 + 1) await PromptOptions(gameId);
                if (game.CurrentDay == Factors.RoundIncrement * 16 + 1)
                {
                    foreach(Player gamePlayer in game.Players)
                    {
                        gamePlayer.ChosenOption = null;
                    }
                    await PromptOptions(gameId);
                }

                foreach (Player gamePlayer in game.Players)
                {
                    gamePlayer.CurrentOrder = null;
                }
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

        public async Task<bool> ChooseOption(string playerId, string option)
        {
            var player = _context.Players.FirstOrDefault(x => x.Id == playerId);
            Game game = _context.Games.FirstOrDefault(g => g.Retailer.Id == playerId ||
                                                           g.Manufacturer.Id == playerId ||
                                                           g.Processor.Id == playerId ||
                                                           g.Farmer.Id == playerId);
            bool thirdPhase = game.CurrentDay == Factors.RoundIncrement * 16 + 1;
            player.ChosenOption = _context.Options.FirstOrDefault(x => x.RoleId == player.Role.Id && x.Name == option);
            player.ChosenOption.Name += ""; //Do not remove this line, otherwise this function will no longer work
            if (!thirdPhase)
            {
                player.Payments.Add(new Payment
                {
                    Amount = player.ChosenOption.CostOfStartUp * -1,
                    DueDay = Factors.RoundIncrement * 8 + 1,
                    FromPlayer = false,
                    PlayerId = player.Id,
                    Topic = "Setup " + player.ChosenOption.Name
                });

                await Clients.Group(game.Id).SendAsync("UpdateGame", JsonConvert.SerializeObject(_context.Games.FirstOrDefault(x => x.Id.Equals(game.Id))));
            }
            else
            {
                await Clients.Group(game.Id).SendAsync("UpdatePromptOptions", JsonConvert.SerializeObject(player));
                if (game.Players.All(p => p.ChosenOption != null))
                {
                    string mostChosenOption = CalculateMostChosen(game.Players);
                    foreach (Player playerGame in game.Players)
                    {
                        player.ChosenOption = _context.Options.FirstOrDefault(x => x.RoleId == player.Role.Id && x.Name == mostChosenOption);
                        player.ChosenOption.Name += "";
                        playerGame.Payments.Add(new Payment
                        {
                            Amount = playerGame.ChosenOption.CostOfStartUp * -1,
                            DueDay = Factors.RoundIncrement * 16 + 1,
                            FromPlayer = false,
                            PlayerId = playerGame.Id,
                            Topic = "Setup " + playerGame.ChosenOption.Name
                        });
                    }
                    await Clients.Group(game.Id).SendAsync("ClosePromptOptions", mostChosenOption);
                }
            }

            _context.Players.Update(player);
            _context.SaveChanges();
            return !thirdPhase;
        }

        public List<string> CheckAvailableRoles(string gameId)
        {
            List<string> availableRoles = new List<string>();
            var game = _context.Games.FirstOrDefault(g => g.Id == gameId);
            if (game == null) return new List<string>();
            if(game.Retailer == null) availableRoles.Add("Retailer"); 
            if(game.Manufacturer == null) availableRoles.Add("Manufacturer"); 
            if(game.Processor == null) availableRoles.Add("Processor"); 
            if(game.Farmer == null) availableRoles.Add("Farmer");
            return availableRoles;
        }

        private string CalculateMostChosen(List<Player> players)
        {
            Dictionary<string, int> options = new Dictionary<string, int>()
                        { { "YouProvide", 0 },{ "YouProvideWithHelp", 0 },{ "TrustedParty", 0 },{ "DLT", 0 }  };
            foreach (Player player in players)
            {
                options[player.ChosenOption.Name] += 1;
            }
            int maxChosen = options.Max(kp => kp.Value);
            List<string> mostChosen = options.Where(kp => kp.Value == maxChosen).Select(kp => kp.Key).ToList();
            return mostChosen[new Random().Next(0, mostChosen.Count)];
        }
    }
}