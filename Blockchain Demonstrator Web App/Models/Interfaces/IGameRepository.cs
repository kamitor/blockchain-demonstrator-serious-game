using Blockchain_Demonstrator_Web_App.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Interfaces
{
    public interface IGameRepository
    {
        public List<Game> CurrentGames { get; set; }
    }
}
