using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Resc.Application.Common.Configurations;
using Resc.Application.Common.Interfaces;
using Resc.Application.DomainValidation.Enums;
using Resc.Application.DomainValidations;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Application.Users.Dtos;
using Resc.Application.Users.Interfaces;
using Resc.Data.Nomenclatures;
using Resc.Data.Users;
using Resc.Data.Users.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Users
{
    public class LoginService : ILoginService
    {
		private readonly IAppDbContext context;
		private readonly IPasswordService passwordService;
		private readonly DomainValidationService validation;
		private readonly AuthConfiguration authConfiguration;

		public LoginService(
			IAppDbContext context,
			IPasswordService passwordService,
			DomainValidationService validation,
			IOptions<AuthConfiguration> options
			)
		{
			this.context = context;
			this.passwordService = passwordService;
			this.validation = validation;
			this.authConfiguration = options.Value;
		}

		public async Task<UserLoginInfoDto> Login(UserCredentialsDto model, CancellationToken cancellationToken)
        {
			var user = await this.context.Set<User>()
					.AsNoTracking()
					.Include(u => u.Role)
					.Include(u => u.Institution)
					.SingleOrDefaultAsync(u => u.Username.Trim() == model.Username.Trim(), cancellationToken);

			if (user == null || user.Status == UserStatus.Deactivated || user.IsLocked)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
			}

			bool isSamePassword = this.passwordService.VerifyHashedPassword(user.Password, model.Password, user.PasswordSalt);
			if (!isSamePassword)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
			}

			var result = new UserLoginInfoDto {
				Fullname = user.FirstName + " " + user.LastName,
				RoleAlias = user.Role.Alias,
				Token = this.CreateToken(user.Id, user.Username, user.Role.Alias, user.Institution?.Name),
				Institution = user.Institution != null
							? new NomenclatureDto<Institution> {
								Id = user.Institution.Id,
								Name = user.Institution.Name
							}
							: null,
			};

			return result;
		}

		private string CreateToken(int userId, string username, string role, string institutionName)
        {
			var claims = new List<Claim> {
					new Claim("username", username),
					new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()),
					new Claim("institutionName", institutionName ?? string.Empty),
					new Claim("role", role)
				};

			var expires = DateTime.Now.AddHours(authConfiguration.ValidHours);
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				authConfiguration.Issuer,
				authConfiguration.Audience,
				claims,
				expires: expires,
				signingCredentials: creds
			);

			string tokenString = new JwtSecurityTokenHandler()
				.WriteToken(token);
			
			return tokenString;
		}
    }
}
