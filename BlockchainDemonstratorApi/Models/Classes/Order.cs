using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Order
    {
        [Key] 
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Incoming order for player with the following id
        /// </summary>
        public string OutgoingOrderForPlayerId { get; set; }

        /// <summary>
        /// Incoming delivery for player with the following id
        /// </summary>
        public string IncomingOrderForPlayerId { get; set; }

        /// <summary>
        /// Order history of player with the following id
        /// </summary>
        public int OrderNumber { get; set; }
        
        public int OrderDay { get; set; }
        
        //public double ArrivalDay { get; set; }
        
        private int _volume;

        [Range(0, double.MaxValue)]
        public int Volume
        {
            get { return _volume; }
            set
            {
                if (value >= 0)
                {
                    _volume = value;
                }
                else
                {
                    _volume = 0;
                }
            }
        }

        public double Price { get; set; }

        [ForeignKey("OrderId")]
        public List<Delivery> Deliveries { get; set; }

        public void GetDeliveries() //TODO: cycle through all deliveries to process to inventory
        {
            foreach (Delivery delivery in Deliveries)
            {

            }
        }
    }
}