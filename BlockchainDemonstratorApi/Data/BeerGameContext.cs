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
    }
}
