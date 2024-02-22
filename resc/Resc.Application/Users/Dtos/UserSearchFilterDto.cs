using Resc.Data.Users.Enums;

namespace Resc.Application.Users.Dtos
{
    public class UserSearchFilterDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public int? InstitutionId { get; set; }
        public UserStatus? Status { get; set; }

        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;
    }
}
