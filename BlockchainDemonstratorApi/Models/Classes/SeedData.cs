using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    /// <summary>
    /// This class is used to create and update the database as well as store default data in the database.
    /// </summary>
    public static class SeedData
    {
        private static BeerGameContext _beerGameContext;
        /// <summary>
        /// Initialize private variable, migrates database and adds default data 
        /// </summary>
        /// <param name="beerGameContext">Context for BeerGame database</param>
        public static void Initialize(BeerGameContext beerGameContext)
        {
            _beerGameContext = beerGameContext;
            _beerGameContext.Database.Migrate();
            CreateRolesAndOptions();
            AddFactors();
            _beerGameContext.SaveChanges();
            
        }

        /// <summary>
        /// Adds all default factors to database if not found, default factors can be found in the Factors class
        /// </summary>
        private static void AddFactors()
        {
            if(!_beerGameContext.Factors.Any(r => r.Id == "DefaultFactors"))
            {
                _beerGameContext.Factors.Add(new Factors());
            }
        }

        /// <summary>
        /// Creates and adds all roles and options
        /// </summary>
        private static void CreateRolesAndOptions()
        {
            if (!RoleExists("Retailer")) _beerGameContext.Roles.Add(CreateRetailer());
            if (!RoleExists("Manufacturer")) _beerGameContext.Roles.Add(CreateManufacturer());
            if (!RoleExists("Processor")) _beerGameContext.Roles.Add(CreateProcessor());
            if (!RoleExists("Farmer")) _beerGameContext.Roles.Add(CreateFarmer());
        }

        /// <summary>
        /// Checks whether the given role exists in the database 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private static bool RoleExists(string roleId)
        {
            return _beerGameContext.Roles.Any(r => r.Id == roleId);
        }

        /// <summary>
        /// Creates retailer role and options
        /// </summary>
        /// <returns>Retailer role with all options</returns>
        private static Role CreateRetailer() 
        {
            Option youProvide = new Option("YouProvide", 10000, 1050, 1818, 709, 2.5625,100, 300 );
            
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 60000, 2100, 1632, 956,1.708, 350, 450 );
            
            Option trustedParty = new Option("TrustedParty", 80000, 350, 1707, 832,2.05, 450, 675 );
            
            Option dlt = new Option("DLT", 30000, 350, 1484, 1448,1.025, 200, 600 );
            
            Option basic = new Option("Basic", 75000, 3500, 1855, 1086, 1.708333, 0, 750);
            Role retailer = new Role("Retailer", 1.7083333, Product.Beer, Factors.RetailProductPrice);

            retailer.Options.Add(youProvide);
            retailer.Options.Add(youProvideWithHelp);
            retailer.Options.Add(trustedParty);
            retailer.Options.Add(dlt);
            retailer.Options.Add(basic);

            return retailer;
        }

        /// <summary>
        /// Creates manufacturer role and options
        /// </summary>
        /// <returns>Manufacturer role with all options</returns>
        private static Role CreateManufacturer()
        {
            Option youProvide = new Option("YouProvide", 80000, 3500, 696, 348, 2, 150, 300);
            
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 10000, 700, 625, 454, 1.375, 400, 375);
            
            Option trustedParty = new Option("TrustedParty", 60000, 350, 653, 419, 1.56, 250, 525);
            
            Option dlt = new Option("DLT", 30000, 1750, 568, 688, 0.825, 500, 750);
            
            Option basic = new Option("Basic", 75000, 3500, 710, 516, 1.375, 0, 750);
            Role manufacturer = new Role("Manufacturer", 1.375, Product.Packs, Factors.ManuProductPrice);

            manufacturer.Options.Add(youProvide);
            manufacturer.Options.Add(youProvideWithHelp);
            manufacturer.Options.Add(trustedParty);
            manufacturer.Options.Add(dlt);
            manufacturer.Options.Add(basic);

            return manufacturer;
        }

        /// <summary>
        /// Creates processor role and options
        /// </summary>
        /// <returns>Processor role with all options</returns>
        private static Role CreateProcessor()
        {
            Option youProvide = new Option("YouProvide", 70000, 2450, 1225,48, 25.75, 150, 225);
            
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 40000, 700, 1100, 64, 17.17, 500, 750);
            
            Option trustedParty = new Option("TrustedParty", 20000, 350, 1150, 56, 20.60, 300, 375);
            
            Option dlt = new Option("DLT", 100000, 3500, 1000, 99, 10.09, 250, 525);
            
            Option basic = new Option("Basic", 75000, 3500, 1250, 73, 17.16667, 0, 750);
            Role processor = new Role("Processor", 17.166667, Product.Barley, Factors.ProcProductPrice);

            processor.Options.Add(youProvide);
            processor.Options.Add(youProvideWithHelp);
            processor.Options.Add(trustedParty);
            processor.Options.Add(dlt);
            processor.Options.Add(basic);

            return processor;
        }

        /// <summary>
        /// Creates farmer role and options
        /// </summary>
        /// <returns>Farmer role with all options</returns>
        private static Role CreateFarmer()
        {
            Option youProvide = new Option("YouProvide", 100000, 2450, 3871, 306, 12.666667, 100, 300);
            
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 30000, 700, 3476, 156, 22.333333, 350, 450);
            
            Option trustedParty = new Option("TrustedParty", 10000, 350, 3634, 136, 26.8, 450, 675);
            
            Option dlt = new Option("DLT", 80000, 3500, 3160, 236, 13.4, 200, 600);
            
            Option basic = new Option("Basic", 75000, 3500, 3950, 177, 22.333333, 0, 750);
            Role farmer = new Role("Farmer", 22.333333, Product.Seeds, Factors.FarmerProductPrice);

            farmer.Options.Add(youProvide);
            farmer.Options.Add(youProvideWithHelp);
            farmer.Options.Add(trustedParty);
            farmer.Options.Add(dlt);
            farmer.Options.Add(basic);

            return farmer;
        }
    }
}
