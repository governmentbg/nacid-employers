using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Applications.Enums;
using Resc.Data.Nomenclatures;
using System;

namespace Resc.Application.Applications.Dtos.Create
{
    public class StudentDto
    {
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string UIN { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public EducationType? EducationType { get; set; }
        public StudentStatusType? Status { get; set; }
		public DateTime? GraduationDate { get; set; }

		public Student ToModel()
			=> new Student(this.FirstName, this.MiddleName, this.LastName, this.UIN, this.Email, this.PhoneNumber,
				this.EducationType, this.Status, this.GraduationDate);
	}
}
