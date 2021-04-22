using BlockchainDemonstratorApi.Models.Enums;
using BlockchainDemonstratorApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Manufacturer : IRole
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public double LeadTime { get; set; } = 1;
        [Required]
        public IOption YouProvide { get; set; }
        [Required]
        public IOption YouProvideWithHelp { get; set; }
        [Required]
        public IOption TrustedParty { get; set; }
        [Required]
        public IOption DLT { get; set; }
        [Required]
        public Product Product { get; set; } = Product.Beer;
    }
}
