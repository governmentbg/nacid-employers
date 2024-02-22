namespace Resc.Application.Applications.Dtos.Create
{
    public class ApplicationDto
    {
        public StudentDto Student { get; set; }
        public UniversityDto University { get; set; }
        public EmployerDto Employer { get; set; }
        public ContractDto Contract { get; set; }
        public ActualEducationDto ActualEducation { get; set; }
        public bool? IsDraft { get; set; }
    }
}
