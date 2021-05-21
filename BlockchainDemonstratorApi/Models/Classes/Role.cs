using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
		public double ProductPrice
		{
			get
			{
				double price = 2000;
				if (string.Equals(Id, "Retailer"))
				{
					price = Convert.ToDouble(Factors.RetailProductPrice);
				}
				else if (string.Equals(Id, "Processor"))
				{
					price = Convert.ToDouble(Factors.ProcProductPrice);
				}
				else if (string.Equals(Id, "Manufacturer"))
				{
					price = Convert.ToDouble(Factors.ManuProductPrice);
				}
				else if (string.Equals(Id, "Farmer"))
				{
					price = Convert.ToDouble(Factors.FarmerProductPrice);
				}

				return price;
			}
		}

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