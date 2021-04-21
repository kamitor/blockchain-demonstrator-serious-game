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

        public DbSet<BlockchainDemonstratorApi.Models.Classes.Game> Game { get; set; }
    }
}
