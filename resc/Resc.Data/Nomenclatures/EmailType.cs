using Resc.Data.Nomenclatures.Models;

namespace Resc.Data.Nomenclatures
{
    public class EmailType : Nomenclature
	{
		public string Subject { get; set; }

		public string Body { get; set; }

		public string Alias { get; set; }
	}
}
