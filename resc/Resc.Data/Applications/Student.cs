using Resc.Data.Applications.Enums;
using Resc.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Resc.Application.Applications
{
    public class Student : IEntity, IAuditable, IConcurrency
    {
		public int Id { get; private set; }

		public string FirstName { get; private set; }
		public string MiddleName { get; private set; }
		public string LastName { get; private set; }
		public string FullName { get; private set; }
		public string UIN { get; private set; }
		public string Email { get; private set; }
		public string PhoneNumber { get; private set; }


		public EducationType? EducationType { get; private set; }
		public StudentStatusType? Status { get; private set; }
		public DateTime? GraduationDate { get; private set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }
		public int Version { get; set; }

		private Student()
		{

		}

		public Student(string firstName, string middleName, string lastName, string uin, string email, string phoneNumber,
			EducationType? educationType, StudentStatusType? status, DateTime? graduationDate)
		{
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.SetFullname(firstName, middleName, lastName);
			this.UIN = uin;
			this.Email = email;
			this.PhoneNumber = phoneNumber;

			this.EducationType = educationType;
			this.Status = status;
			this.GraduationDate = graduationDate;
		}

		public Student(Student student)
			: this(student.FirstName, student.MiddleName, student.LastName, student.UIN, student.Email, student.PhoneNumber,
				  student.EducationType, student.Status, student.GraduationDate)
		{

		}

		public void Update(string firstName, string middleName, string lastName, string uin, string email, string phoneNumber,
			EducationType? educationType, StudentStatusType? status, DateTime? graduationDate)
		{
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.SetFullname(firstName, middleName, lastName);
			this.UIN = uin;
			this.Email = email;
			this.PhoneNumber = phoneNumber;

			this.EducationType = educationType;
			this.Status = status;
			this.GraduationDate = graduationDate;
		}

		private void SetFullname(string firstName, string middleName, string lastName)
		{
			this.FullName = string.Join(" ", new List<string> { firstName, middleName, lastName }
					.Where(e => !string.IsNullOrWhiteSpace(e))
					.Select(e => e));
		}
	}
}
