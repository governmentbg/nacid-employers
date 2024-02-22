using Resc.Data.Nomenclatures;
using System.Collections.Generic;

namespace Resc.Data.Lists
{
    public class EmployerListItem : BaseListItem
	{
		public int EmployerListId { get; set; }
		public EmployerList EmployerList { get; set; }
		public int CityId { get; set; }
		public City City { get; set; }
		public string Name { get; set; }
		public string Bulstat { get; set; }
		public string Address { get; set; }
		public string FullAddress { get; set; }
		public ICollection<EmployerSpeciality> Specialities { get; set; } = new List<EmployerSpeciality>();
    }
}
