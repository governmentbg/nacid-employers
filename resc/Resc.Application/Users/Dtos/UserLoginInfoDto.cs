using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Nomenclatures;

namespace Resc.Application.Users.Dtos
{
    public class UserLoginInfoDto
    {
		public string Fullname { get; set; }
		public string Token { get; set; }
		public string RoleAlias { get; set; }
		public NomenclatureDto<Institution> Institution { get; set; }

	}
}
