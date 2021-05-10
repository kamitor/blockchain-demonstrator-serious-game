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
            Option youProvide = new Option("YouProvide", 10000, 1050, 54537, 2.5625, 100, 300);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 60000, 2100, 48972, 1.708, 350, 450);
            Option trustedParty = new Option("TrustedParty", 80000, 350, 51198, 2.05, 450, 675);
            Option dlt = new Option("DLT", 30000, 350, 44520, 1.025, 200, 600);
            Role retailer = new Role("Retailer", 1.7083333, Product.Beer);

            retailer.Options.Add(youProvide);
            retailer.Options.Add(youProvideWithHelp);
            retailer.Options.Add(trustedParty);
            retailer.Options.Add(dlt);

            return retailer;
        }

        /// <summary>
        /// Creates manufacturer role and options
        /// </summary>
        /// <returns>Manufacturer role with all options</returns>
        private static Role CreateManufacturer()
        {
            Option youProvide = new Option("YouProvide", 80000, 3500, 20874, 2, 150, 300);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 10000, 700, 18744, 1.375, 400, 375);
            Option trustedParty = new Option("TrustedParty", 60000, 350, 19596, 1.56, 250, 525);
            Option dlt = new Option("DLT", 30000, 1750, 17040, 0.825, 500, 750);
            Role manufacturer = new Role("Manufacturer", 1.375, Product.Packs);

            manufacturer.Options.Add(youProvide);
            manufacturer.Options.Add(youProvideWithHelp);
            manufacturer.Options.Add(trustedParty);
            manufacturer.Options.Add(dlt);

            return manufacturer;
        }

        /// <summary>
        /// Creates processor role and options
        /// </summary>
        /// <returns>Processor role with all options</returns>
        private static Role CreateProcessor()
        {
            Option youProvide = new Option("YouProvide", 70000, 2450, 1250, 25.75, 150, 225);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 40000, 700, 33000, 17.17, 500, 750);
            Option trustedParty = new Option("TrustedParty", 20000, 350, 34500, 20.60, 300, 375);
            Option dlt = new Option("DLT", 100000, 3500, 30000, 10.09, 250, 525);
            Role processor = new Role("Processor", 17.166667, Product.Barley);

            processor.Options.Add(youProvide);
            processor.Options.Add(youProvideWithHelp);
            processor.Options.Add(trustedParty);
            processor.Options.Add(dlt);

            return processor;
        }

        /// <summary>
        /// Creates farmer role and options
        /// </summary>
        /// <returns>Farmer role with all options</returns>
        private static Role CreateFarmer()
        {
            Option youProvide = new Option("YouProvide", 100000, 2450, 116130, 12.666667, 100, 300);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 30000, 700, 104280, 22.333333, 350, 450);
            Option trustedParty = new Option("TrustedParty", 10000, 350, 109020, 26.8, 450, 675);
            Option dlt = new Option("DLT", 80000, 3500, 94800, 13.4, 200, 600);
            Role farmer = new Role("Farmer", 22.333333, Product.Seeds);

            farmer.Options.Add(youProvide);
            farmer.Options.Add(youProvideWithHelp);
            farmer.Options.Add(trustedParty);
            farmer.Options.Add(dlt);

            return farmer;
        }
    }
}
