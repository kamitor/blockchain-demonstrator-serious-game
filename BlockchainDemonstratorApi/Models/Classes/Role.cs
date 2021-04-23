using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Role
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public double LeadTime { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public virtual List<Option> Options { get; set; } = new List<Option>(4);

        public Role(string id, double leadTime, Product product)
        {
            Id = id;
            LeadTime = leadTime;
            Product = product;
        }
    }

}
