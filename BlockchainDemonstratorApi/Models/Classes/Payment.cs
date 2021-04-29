using System.ComponentModel.DataAnnotations.Schema;

namespace BlockchainDemonstratorApi.Models.Classes
{
    [NotMapped]
    public class Payment
    {
        public double Amount { get; set; }
        /**
         * <summary>Day when payment has to be made</summary>
         */
        public double DueDay { get; set; }

        public bool toPlayer;

    }
}