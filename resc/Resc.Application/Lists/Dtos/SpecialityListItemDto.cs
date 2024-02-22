namespace Resc.Application.InstitutionSpecialities.Dtos
{
    public class SpecialityListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SpecialityListItemDto(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}