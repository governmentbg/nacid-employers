using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using System.Collections.Generic;

namespace Resc.Application.Lists.Public.Dtos
{
    public class SpecialityListItemPublicDto
	{
		public int Id { get; set; }
		public int StudentsCount { get; set; }
		public int FreeSpotsCount { get; set; }

		public Speciality Speciality { get; set; }

		public ResearchArea ResearchArea { get; set; }

		public Institution Institution { get; set; }

		public EducationalQualification EducationalQualification { get; set; }

		public EducationFormType EducationFormType { get; set; }

		public List<SpecialityMinister> SpecialityMinisters { get; set; } = new List<SpecialityMinister>();
	}
}
