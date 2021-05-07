﻿using System;
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
                Role = new Role("Manufacturer", 2, Product.Beer), Inventory = 25, Balance = 10000
            };


            for (int i = 0; i < 3; i++)
            {
                pl.IncomingOrders.Add(new Order(){ArrivalDay = 1, Volume = 10, Price = 1000}); 
            }

            pl.GetOutgoingDeliveries(1);

            pl.UpdateBalance(22);
            
            Console.WriteLine(pl.Balance);
        }
    }
}