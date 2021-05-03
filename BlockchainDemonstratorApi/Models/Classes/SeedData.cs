using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public static class SeedData
    {
        private static BeerGameContext _beerGameContext;

        public static void Initialize(BeerGameContext beerGameContext)
        {
            _beerGameContext = beerGameContext;
            _beerGameContext.Database.Migrate();
            CreateRolesAndOptions();
            AddFactors();
            _beerGameContext.SaveChanges();
            
        }

        private static void AddFactors()
        {
            if(!_beerGameContext.Factors.Any(r => r.Id == "DefaultFactors"))
            {
                _beerGameContext.Factors.Add(new Factors());
            }
        }

        private static void CreateRolesAndOptions()
        {
            if (!RoleExists("Retailer")) _beerGameContext.Roles.Add(CreateRetailer());
            if (!RoleExists("Manufacturer")) _beerGameContext.Roles.Add(CreateManufacturer());
            if (!RoleExists("Processor")) _beerGameContext.Roles.Add(CreateProcessor());
            if (!RoleExists("Farmer")) _beerGameContext.Roles.Add(CreateFarmer());
        }

        private static bool RoleExists(string roleId)
        {
            return _beerGameContext.Roles.FirstOrDefault(r => r.Id == roleId) != null;
        }

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
