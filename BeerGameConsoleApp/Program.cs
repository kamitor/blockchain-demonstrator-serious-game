using System;
using System.Collections.Concurrent;
using System.Threading.Channels;
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
        }
    }
}