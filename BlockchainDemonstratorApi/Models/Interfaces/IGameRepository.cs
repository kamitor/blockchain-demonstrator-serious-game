using BlockchainDemonstratorApi.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Interfaces
{
    public interface IGameRepository
    {
        public List<Game> CurrentGames { get; set; }
    }
}
