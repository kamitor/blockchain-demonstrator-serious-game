using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Models.Classes;

namespace BlockchainDemonstratorApi.Data
{
    public class BeerGameContext : DbContext
    {
        public BeerGameContext (DbContextOptions<BeerGameContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; } //TODO: check if every usage of this dbset is now Games instead of Game
        public DbSet<Role> Roles { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Factors> Factors { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.RequestForPlayer)
                .WithMany(p => p.IncomingOrders);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryToPlayer)
                .WithMany(p => p.IncomingDeliveries);
            modelBuilder.Entity<Order>()
               .HasOne(o => o.HistoryOfPlayer)
               .WithMany(p => p.OrderHistory);
        }*/
    }
}
