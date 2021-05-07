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
            Option youProvide = new Option("YouProvide", 0, 0, 0, 0, 0);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 0, 0, 0, 0, 0);
            Option trustedParty = new Option("TrustedParty", 0, 0, 0, 0, 0);
            Option dlt = new Option("DLT", 0, 0, 0, 0, 0);
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
            Option youProvide = new Option("YouProvide", 0, 0, 0, 0, 0);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 0, 0, 0, 0, 0);
            Option trustedParty = new Option("TrustedParty", 0, 0, 0, 0, 0);
            Option dlt = new Option("DLT", 0, 0, 0, 0, 0);
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
            Option youProvide = new Option("YouProvide", 0, 0, 0, 0, 0);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 0, 0, 0, 0, 0);
            Option trustedParty = new Option("TrustedParty", 0, 0, 0, 0, 0);
            Option dlt = new Option("DLT", 0, 0, 0, 0, 0);
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
            Option youProvide = new Option("YouProvide", 0, 0, 0, 0, 0);
            Option youProvideWithHelp = new Option("YouProvideWithHelp", 0, 0, 0, 0, 0);
            Option trustedParty = new Option("TrustedParty", 0, 0, 0, 0, 0);
            Option dlt = new Option("DLT", 0, 0, 0, 0, 0);
            Role farmer = new Role("Farmer", 22.333333, Product.Seeds);

            farmer.Options.Add(youProvide);
            farmer.Options.Add(youProvideWithHelp);
            farmer.Options.Add(trustedParty);
            farmer.Options.Add(dlt);

            return farmer;
        }
    }
}
