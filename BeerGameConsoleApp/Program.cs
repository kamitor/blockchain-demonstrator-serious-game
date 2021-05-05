using System;
using System.Collections.Concurrent;
using System.Threading.Channels;
using System.Xml.Serialization;
using Blockchain_Demonstrator_Web_App.Models;
using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;

namespace BeerGameConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Game gm = new Game();
            // gm.Players.Add(Role.Farmer, new Player("Farmer", new Farmer()));
            // gm.Players.Add(Role.Retailer, new Player("Retailer", new Retailer()));
            // gm.Players.Add(Role.Manufacturer, new Player("Manufacturer", new Manufacturer()));
            // gm.Players.Add(Role.Processor, new Player("Processor", new Processor()));

            // while (true)
            // {
            //     foreach (var player in gm.Players)
            //     {
            //         Console.WriteLine("-----------------------------");
            //         Console.WriteLine(
            //             $"role: {player.Key} inventory: {player.Value.Inventory} backorder: {player.Value.Backorder}");
            //         if (player.Value.IncomingOrder != null)
            //         {
            //             Console.WriteLine(
            //                 $"incoming order: {player.Value.IncomingOrder.OrderDay}, {player.Value.IncomingOrder.ArrivalDay}, {player.Value.IncomingOrder.Volume}");
            //         }
            //
            //         foreach (var item in player.Value.IncomingDelivery)
            //         {
            //             Console.WriteLine(
            //                 $"orderday: {item.OrderDay} arrivalday: {item.ArrivalDay} volume: {item.Volume}");
            //         }
            //
            //         Console.WriteLine("-----------------------------");
            //     }
            //
            //     foreach (var player in gm.Players)
            //     {
            //         Console.WriteLine($"input for {player.Key}");
            //         var input = Console.ReadLine();
            //         player.Value.CurrentOrder = new Order() {Volume = int.Parse(input)};
            //     }
            //
            //     gm.Progress();
            // }

            Player retail = new Player("retail") {Role = new Role("Retailer", 1.7083333, Product.Packs)};
            Player manu = new Player("manufacturer") {Role = new Role("Manufacturer", 1.375, Product.Packs)};
            Player processor = new Player("processor") {Role = new Role("Processor", 17.166667, Product.Packs)};
            Player farmer = new Player("farmer") {Role = new Role("Farmer", 22.333333, Product.Packs)};

            Game gm = new Game()
            {
                Retailer = retail,
                Manufacturer = manu,
                Processor = processor,
                Farmer = farmer
            };

            var x = 0;
            for (int i = 0; i < 11; i++)
            {
                gm.Progress();
                foreach (Player gmPlayer in gm.Players)
                {
                    Console.WriteLine(gmPlayer.Name + " " + gmPlayer.Balance);
                    Console.WriteLine("Profit: " + gmPlayer.Profit);
                    gmPlayer.CurrentOrder = new Order() {Volume = i};
                }
                Console.WriteLine("------------------------");
            }
        }
    }
}