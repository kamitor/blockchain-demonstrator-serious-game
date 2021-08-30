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
    /// <summary>
    /// The GameHub class is used for SignalR real time communication.
    /// This class contains multiple methods which can be triggered by the client.
    /// </summary>
    public class GameHub : Hub
    {
        private readonly BeerGameContext _context;

        public GameHub(BeerGameContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to send orders and progress the game.
        /// </summary>
        /// <param name="volume">The volume of the sent order</param>
        /// <param name="gameId">The game ID of the current game</param>
        /// <param name="playerId">The player ID of the player who sent the order</param>
        /// <remarks>
        /// When all four players of the game have sent their order, the game will progress and send a UpdateGame call to the clients.
        /// When the current round is 8, the game will go into the second phase and send out the PromptOptions call to the clients.
        /// When the current round is 16, the game will go into the third phase and send out the PromptOptions call to the clients again.
        /// When the current round is 24, the game will end and send out the EndGame call to the clients.
        /// After each round the graphs of the game master will also be updated with the UpdateGraphs call to the clients.
        /// </remarks>
        public async Task SendOrder(string volume, string gameId, string playerId)
        {
            Game game = _context.Games.FirstOrDefault(x => x.Id.Equals(gameId));
            
            if (game == null) return;

            Player player = game.Players.FirstOrDefault(x => x.Id.Equals(playerId));

            if (player != null)
                player.CurrentOrder = new Order(){Volume = Convert.ToInt32(volume)};

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

                if (game.CurrentDay == Factors.RoundIncrement * 24 + 1) await EndGame(gameId);

                foreach (Player gamePlayer in game.Players)
                {
                    gamePlayer.CurrentOrder = null;
                }
            }

            await Clients.Group(game.Id).SendAsync("UpdateGraphs", game);
            _context.Games.Update(game);
            _context.SaveChanges();
        }
        public async Task EndGame(string gameId)
        {
            await Clients.Group(gameId).SendAsync("EndGame");
        }

        /// <summary>
        /// Adds player to a group
        /// </summary>
        /// <remarks>
        /// The player gets added to a group with a name corresponding to the gameId
        /// </remarks>
        /// <param name="gameId">string specifying the gameId</param>
        /// <returns></returns>
        public Task JoinGroup(string gameId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        /// <summary>
        /// This method is used to join a game with real time communication.
        /// </summary>
        /// <param name="gameId">The ID of the requested game to join.</param>
        /// <param name="role">The requested role to play as.</param>
        /// <param name="name">The requested name of the player.</param>
        /// <param name="playerId">The ID of the player</param>
        /// <returns></returns>
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
                        await Clients.Group(gameId).SendAsync("ShowGame", JsonConvert.SerializeObject(game));
                    }
                    _context.Games.Update(game);
                    _context.SaveChanges();
                }

            }
        }

        /// <summary>
        /// Updates the game page for each player in group
        /// </summary>
        /// <param name="gameId">string specifying the gameId</param>
        public async Task UpdateGame(string gameId)
        {
            await Clients.Group(gameId)
                .SendAsync("UpdateGame", JsonConvert.SerializeObject(_context.Games.FirstOrDefault(x => x.Id.Equals(gameId))) );
        }

        /// <summary>
        /// Show the choose option prompt to each player in group
        /// </summary>
        /// <param name="gameId">string specifying the gameId</param>
        public async Task PromptOptions(string gameId)
        {
            await Clients.Group(gameId).SendAsync("PromptOptions");
        }

        /// <summary>
        /// This method is used by a player to choose a supply chain option.
        /// </summary>
        /// <param name="playerId">The ID of the player.</param>
        /// <param name="option">The option the player has selected.</param>
        /// <returns></returns>
        public async Task<bool> ChooseOption(string playerId, string option)
        {
            Game game = _context.Games.FirstOrDefault(g => g.Retailer.Id == playerId ||
                                                           g.Manufacturer.Id == playerId ||
                                                           g.Processor.Id == playerId ||
                                                           g.Farmer.Id == playerId);
            Player player = game.Players.FirstOrDefault(p => p.Id == playerId);
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

            _context.Games.Update(game);
            _context.SaveChanges();
            return !thirdPhase;
        }

        /// <summary>
        /// This method is used to check avaialable roles.
        /// </summary>
        /// <param name="gameId">The ID of the game to check.</param>
        /// <returns>Returns a list of available roles</returns>
        /// <remarks>This method could be reworked as it does not fully apply real time communication</remarks>
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

        /// <summary>
        /// This method is used to calculate the most chosen option. 
        /// This method is mainly used to decide which option will be chosen 
        /// in the third phase when the players must collectively make a decission.
        /// </summary>
        /// <param name="players">The players of the given game</param>
        /// <returns>Returns the name of the most chosen option.</returns>
        /// <remarks>If their are multiple most chosen options, a random one of the most chosen will be picked.</remarks>
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
