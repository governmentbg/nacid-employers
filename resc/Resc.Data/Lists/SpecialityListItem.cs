using Resc.Data.Nomenclatures;
using System.Collections.Generic;

namespace Resc.Data.Lists
{
	public class SpecialityListItem : BaseListItem
	{
		public int StudentsCount { get; set; }

		public int SpecialityListId { get; set; }
		public SpecialityList SpecialityList { get; set; }

		public int SpecialityId { get; set; }
		public Speciality Speciality { get; set; }

		public int ResearchAreaId { get; set; }
		public ResearchArea ResearchArea { get; set; }

		public int InstitutionId { get; set; }
		public Institution Institution { get; set; }

		public int EducationalQualificationId { get; set; }
		public EducationalQualification EducationalQualification { get; set; }

		public int EducationFormTypeId { get; set; }
		public EducationFormType EducationFormType { get; set; }

		public List<SpecialityMinister> SpecialityMinisters { get; set; } = new List<SpecialityMinister>();

		public void Update(int educationalQualificationId, int educationFormTypeId, int institutionId, int specialityId, int researchAreaId)
		{
			this.EducationalQualificationId = educationalQualificationId;
			this.EducationFormTypeId = educationFormTypeId;
			this.InstitutionId = institutionId;
			this.SpecialityId = specialityId;
			this.ResearchAreaId = researchAreaId;
		}
	}
}
