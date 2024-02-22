using Resc.Data.Applications.Enums;

namespace Resc.Application.Applications.Dtos.Create
{
    public class ContactPersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ContactType Type { get; set; }
    }
}
