using Resc.Data.Common.Interfaces;
using Resc.Data.Nomenclatures;

namespace Resc.Data.Lists
{
	public class EmployerSpeciality : IEntity
	{
		public int Id { get; set; }

		public int SpecialityId { get; set; }
		public Speciality Speciality { get; set; }

		public int ItemId { get; set; }
		public EmployerListItem Item { get; set; }

		public int StudentsCount { get; set; }

	}
}
