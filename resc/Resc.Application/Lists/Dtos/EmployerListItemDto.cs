namespace Resc.Application.InstitutionSpecialities.Dtos
{
    public class EmployerListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public EmployerListItemDto(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}