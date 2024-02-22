using EnumsNET;
using Resc.Data.Users;
using Resc.Data.Users.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Resc.Application.Users.Dtos
{
    public class UserSearchResultDto
    {
		public int? Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string FullName => string.Join(' ', new List<string> { FirstName, MiddleName, LastName }.Where(e => !string.IsNullOrWhiteSpace(e)));
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Role { get; set; }
		public string InstitutionName { get; set; }
		public UserStatus Status { get; set; }

		//ForPdfExport
		public string StatusDescription { get; set; }

		public static Expression<Func<User, UserSearchResultDto>> SelectExpression =>
			user => new UserSearchResultDto {
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				Phone = user.Phone,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Role = user.Role.Name,
				InstitutionName = user.Institution.Name,
				Status = user.Status,
				StatusDescription = user.Status.AsString(EnumFormat.Description)
			};
	}
}
