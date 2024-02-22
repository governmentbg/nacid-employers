using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Applications;
using Resc.Data.Applications.Enums;
using Resc.Data.Nomenclatures;
using System;

namespace Resc.Application.Applications.Dtos.Create
{
	public class ActualEducationDto
	{
		public NomenclatureDto<EducationalQualification> EducationalQualification { get; set; }
		public StudentStatusType? Status { get; set; }
		public string CourseYear { get; set; }
		public DateTime? GraduationDate { get; set; }
		public EducationType? EducationType { get; set; }

		public ActualEducation ToModel()
			=> new ActualEducation(this.EducationalQualification.Id, this.Status, this.CourseYear, this.GraduationDate, this.EducationType);
	}
}
