using Resc.Data.Applications.Register;
using Resc.Data.Common.Enums;
using System;
using System.Linq.Expressions;

namespace Resc.Application.Applications.Dtos.Search
{
    public class ApplicationSearchResultDto
	{
		public int LotId { get; set; }
		public int CommitId { get; set; }
		public CommitState State { get; set; }
		public string RegisterNumber { get; set; }
		public string ContractNumber { get; set; }
		public DateTime? SigningDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Institution { get; set; }
		public string Speciality { get; set; }
		public string EmployerName { get; set; }
		public string Bulstat { get; set; }
		public string StudentName { get; set; }
		public string StudentEmail { get; set; }
		public string StudentUIN { get; set; }

		public static Expression<Func<ApplicationCommit, ApplicationSearchResultDto>> SelectExpression
			=> commit => new ApplicationSearchResultDto {
				LotId = commit.LotId,
				CommitId = commit.Id,
				State = commit.State,
				RegisterNumber = commit.Lot.RegisterNumber,
				ContractNumber = commit.ContractPart.Entity.Number,
				SigningDate = commit.ContractPart.Entity.SigningDate,
				EndDate = commit.ContractPart.Entity.EndDate,
				Institution = commit.UniversityPart.Entity.Institution.Name,
				Speciality = commit.UniversityPart.Entity.SpecialityListItem.Speciality.Name,
				EmployerName = commit.EmployerPart.Entity.EmployerListItem.Name,
				Bulstat = commit.EmployerPart.Entity.EmployerListItem.Bulstat,
				StudentName = $"{commit.StudentPart.Entity.FirstName} {commit.StudentPart.Entity.LastName}",
				StudentEmail = commit.StudentPart.Entity.Email,
				StudentUIN = commit.StudentPart.Entity.UIN
			};
	}
}
