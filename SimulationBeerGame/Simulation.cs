using System;
using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;

namespace SimulationBeerGame
{
	public class Simulation
	{
		private Game _game;

		public Simulation()
		{
			_game = new Game(Guid.NewGuid().ToString());
			
			Player retailer = new Player("Retailer-Sim");
			retailer.Role = new Role("Retailer", Product.Beer);
			retailer.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1, 0, 750);
			_game.Retailer = retailer;

			Player manufacturer = new Player("Manufacturer-Sim");
			manufacturer.Role = new Role("Manufacturer", Product.Packs);
			manufacturer.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1, 0, 750);
			_game.Manufacturer = manufacturer;

			Player processor = new Player("Processor-Sim");
			processor.Role = new Role("Processor", Product.Barley);
			processor.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1, 0, 750);
			_game.Processor = processor;

			Player farmer = new Player("Farmer-Sim");
			farmer.Role = new Role("Farmer", Product.Seeds);
			farmer.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1, 0, 750);
			_game.Farmer = farmer;
			
			_game.SetupGame();
		}

		public void RunSimulation()
		{
			while (_game.CurrentDay != Factors.RoundIncrement * 24 + 1)
			{
				foreach (Player player in _game.Players)
				{
					//player.SimulateCurrentOrder();
				}
				_game.Progress();
				Console.WriteLine("-----------------------------------------------------");
				Console.WriteLine(_game);
			}
		}
	}
}