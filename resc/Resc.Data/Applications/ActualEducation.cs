using Resc.Data.Applications.Enums;
using Resc.Data.Common.Interfaces;
using Resc.Data.Nomenclatures;
using System;

namespace Resc.Data.Applications
{
	public class ActualEducation : IEntity, IAuditable, IConcurrency
	{
		public int Id { get; set; }
		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }
		public int Version { get; set; }

		public int? EducationalQualificationId { get; private set; }
		public EducationalQualification EducationalQualification { get; private set; }

		public EducationType? EducationType { get; private set; }
		public StudentStatusType? Status { get; private set; }
		public string CourseYear { get; private set; }

		public DateTime? GraduationDate { get; private set; }

		public ActualEducation(int? educationalQualificationId, StudentStatusType? status, string courseYear, DateTime? graduationDate, EducationType? educationType)
		{
			this.EducationalQualificationId = educationalQualificationId;
			this.Status = status;
			this.CourseYear = courseYear;
			this.GraduationDate = graduationDate;
			this.EducationType = educationType;
		}

		public ActualEducation(ActualEducation actualEducation)
			: this(actualEducation.EducationalQualificationId, actualEducation.Status, actualEducation.CourseYear, actualEducation.GraduationDate, actualEducation.EducationType)
		{

		}

		public void Update(int? educationalQualificationId, StudentStatusType? status, string courseYear, DateTime? graduationDate, EducationType? educationType)
		{
			this.EducationalQualificationId = educationalQualificationId;
			this.Status = status;
			this.CourseYear = courseYear;
			this.GraduationDate = graduationDate;
			this.EducationType = educationType;
		}
	}
}
