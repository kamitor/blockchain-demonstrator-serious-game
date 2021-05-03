using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Payment
    {
        [Key] 
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string PlayerId { get; set; }
        public double Amount { get; set; }
        /**
         * <summary>Day when payment has to be made</summary>
         */
        public double DueDay { get; set; }
        public bool FromPlayer { get; set; }
    }
}