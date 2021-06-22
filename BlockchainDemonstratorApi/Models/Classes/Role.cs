using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
	/// <summary>
	/// The Role class is used to represent the four roles in the game: Retailer, Manufacturer, Processor and Farmer.
	/// </summary>
	/// <remarks>There are four pre-made objects of this class in the database, 
	/// containing game data of the roles. The default values of these objects can be found in the SeedData class</remarks>
	public class Role
	{
		[Key]
		public string Id { get; set; }

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

		public Role(string id, Product product)
		{
			Id = id;
			Product = product;
		}
	}
}