using Resc.Application.Common.Dtos;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Users;
using System;
using System.Linq.Expressions;

namespace Resc.Application.Nomenclatures.Dtos
{
	public class RoleNomenclatureDto : NomenclatureDto<Role>, IMapping<Role, RoleNomenclatureDto>
	{
		public string Alias { get; set; }

		public new Expression<Func<Role, RoleNomenclatureDto>> Map()
		{
			return e => new RoleNomenclatureDto {
				Id = e.Id,
				Name = e.Name,
				Alias = e.Alias
			};
		}
	}
}
