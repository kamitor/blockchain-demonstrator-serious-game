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
            
            //pl.OutgoingOrders.Add(new Order(){ArrivalDay = 1, Volume = 10, Price = 1000}); No longer works because order does not have price
            
            pl.ProcessDeliveries(1);
            
            pl.UpdateBalance(8);


            for (int i = 0; i < 3; i++)
            {
                //pl.IncomingOrders.Add(new Order(){ArrivalDay = 1, Volume = 10, Price = 1000}); 
            }

            pl.GetOutgoingDeliveries(1);

            pl.UpdateBalance(22);
            
            Console.WriteLine(pl.Balance);
        }
    }
}