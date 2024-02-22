using Resc.Data.Nomenclatures.Models;
using System.Collections.Generic;

namespace Resc.Data.Nomenclatures
{
	public class RegisterIndex : Nomenclature
	{
		public string Alias { get; set; }

		public string Format { get; set; }

		public ICollection<RegisterIndexCounter> Counters { get; set; }
	}
}
