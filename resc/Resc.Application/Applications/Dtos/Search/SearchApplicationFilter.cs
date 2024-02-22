using Resc.Data.Common.Enums;
using System;

namespace Resc.Application.Applications.Dtos.Search
{
    public class SearchApplicationFilter
    {
		public string RegisterNumber { get; set; }
		public string ContractNumber { get; set; }
		public DateTime? SigningDateFrom { get; set; }
		public DateTime? SigningDateTo { get; set; }

		public int? InstitutionId { get; set; }
		public string Institution { get; set; }

		public int? SpecialityId { get; set; }

		public int? EmployerListItemId { get; set; }
		public string Bulstat { get; set; }

		public string StudentName { get; set; }
		public string StudentUIN { get; set; }

		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public CommitState? State { get; set; }
	}
}
