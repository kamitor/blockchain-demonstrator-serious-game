using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Delivery
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        /// The amount of which the suplier was able to send.
        /// </summary>
        [Required]
        public int Volume { get; set; }

       /// <summary>
       /// The day on which the delivery was sent.
       /// </summary>
        [Required]
        public int SendDeliveryDay { get; set; }

        /// <summary>
        /// The day on which the delivery will arrive. 
        /// </summary>
        /// <remarks>This value is predetermined when the delivery is made.</remarks>
        [Required]
        public double ArrivalDay { get; set; }

        [Required]
        public bool Processed { get; set; } = false;
        
        /// <summary>
        /// The price of the delivery. This is used to both pay the supplier and subtract money from the customer.
        /// </summary>
        [Required]
        public double Price { get; set; }
    }
}
