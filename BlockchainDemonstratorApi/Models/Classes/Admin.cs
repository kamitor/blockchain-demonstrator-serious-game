using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Admin
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
        [JsonIgnoreAttribute]
        [Required]
        public string Salt { get; set; }
    }
}
