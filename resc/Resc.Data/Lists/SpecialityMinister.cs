using Resc.Data.Common.Interfaces;
using Resc.Data.Nomenclatures;

namespace Resc.Data.Lists
{
	public class SpecialityMinister : IEntity
	{
		public int Id { get; set; }

		public int SpecialityListItemId { get; set; }
		public SpecialityListItem SpecialityListItem { get; set; }

		public int MinisterId { get; set; }
		public Minister Minister { get; set; }
	}
}
