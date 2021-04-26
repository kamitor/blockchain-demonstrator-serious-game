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
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public int OrderDay { get; set; }
        public double ArrivalDay { get; set; }
        [NotMapped] private int _volume;

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
    }
}