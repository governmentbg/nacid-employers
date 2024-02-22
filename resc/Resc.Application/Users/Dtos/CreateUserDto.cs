using Resc.Data.Nomenclatures;

namespace Resc.Application.Users.Dtos
{
    public class CreateUserDto
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public string RoleAlias { get; set; }
        public Institution Institution { get; set; }
        public string Position { get; set; }
    }
}
