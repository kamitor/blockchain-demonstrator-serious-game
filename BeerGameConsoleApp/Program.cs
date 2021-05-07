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
            Player pl = new Player("Manufacturer") {Role = new Role("Manufacturer", 2, Product.Beer)};

            pl.Inventory = 1000;
            pl.Balance = 10000;
            
            pl.IncomingDeliveries.Add(new Order(){ArrivalDay = 1, Volume = 10, Price = 1000});
            
            pl.ProcessDeliveries(1);
            
            pl.UpdateBalance(8);

            Console.WriteLine(pl.Balance);
        }
    }
}