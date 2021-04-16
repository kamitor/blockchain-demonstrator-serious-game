using Blockchain_Demonstrator_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Classes
{
    public class Game
    {
        public string Id { get; set; }
        public Phase CurrentPhase { get; set; }
        public int CurrentWeek { get; set; }
        public Dictionary<Role,Player> Players{ get; set; }
    }
}
