using Resc.Application.Applications.Dtos.Create;

namespace Resc.Application.Applications.Dtos.Modification
{
	public class ApplicationUpdateDto
	{
		public StudentDto Student { get; set; }
		public UniversityDto University { get; set; }
		public EmployerDto Employer { get; set; }
		public ContractDto Contract { get; set; }
		public ActualEducationDto ActualEducation { get; set; }

		public bool ChangeStatus { get; set; }
	}
}
