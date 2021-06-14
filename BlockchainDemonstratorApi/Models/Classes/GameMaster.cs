using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class GameMaster
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("GameMasterId")]
        public virtual List<Game> Games { get; set; }
    }
}
