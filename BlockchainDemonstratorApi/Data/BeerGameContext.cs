using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Models.Classes;

namespace BlockchainDemonstratorApi.Data
{
    /// <summary>
    /// This class represents the BeerGameContext database that will be produced.
    /// The BeerGameContext contains multiple tables which are represented with the DbSet properties.
    /// </summary>
    public class BeerGameContext : DbContext
    {
        public BeerGameContext (DbContextOptions<BeerGameContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Factors> Factors { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<GameMaster> GameMasters { get; set; }
        public DbSet<Admin> Admins { get; set; }

        /// <summary>
        /// This method is used to override the use of Lazy Loading, enforcing it to be used.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
