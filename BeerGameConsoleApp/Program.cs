using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            Player pl = new Player("Manufacturer")
            {
                Role = new Role("Manufacturer", 2, Product.Beer, 2000), Inventory = 25, Balance = 10000
            };

            pl.Inventory = 1000;
            pl.Balance = 10000;

            pl.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1.375, 0, 750);
            
            pl.AddTransportCost(1, 0);
            
            pl.AddTransportCost(1, 1);
            
            pl.AddTransportCost(1, 2);
            
            pl.AddTransportCost(1, 3);
            
           
        }
    }
}