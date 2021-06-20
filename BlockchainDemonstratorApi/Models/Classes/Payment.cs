using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockchainDemonstratorApi.Models.Classes
{
    /// <summary>
    /// The Payment class represents the payments sent and recieved in the beer game.
    /// </summary>
    public class Payment
    {
        [Key] 
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string PlayerId { get; set; }
        /// <remarks>
        /// The payment amount can be either positive or negative, 
        /// which also decides whether the player has to pay or gets to recieve the amount.
        /// </remarks>
        public double Amount { get; set; }
        ///<summary>
        ///Day when the payment has to be made
        ///</summary>
        public double DueDay { get; set; }
        public bool FromPlayer { get; set; }
        /// <summary>
        /// The topic of the payment, such as maintenance cost, flexibility, etc.
        /// </summary>
        public string Topic { get; set; }
    }
}