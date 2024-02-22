using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Resc.Application.Common.Configurations;
using Resc.Application.Common.Interfaces;
using Resc.Application.DomainValidation.Enums;
using Resc.Application.DomainValidations;
using Resc.Application.Emails.Interfaces;
using Resc.Application.Users.Dtos;
using Resc.Application.Users.Interfaces;
using Resc.Data.Emails;
using Resc.Data.Nomenclatures.Constants;
using Resc.Data.Users;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Users
{
    public class ActivationService : IActivationService
    {
		private readonly IAppDbContext context;
		private readonly IPasswordService passwordService;
		private readonly IEmailService emailService;
		private readonly DomainValidationService validation;
		private readonly AuthConfiguration authConfig;

		public ActivationService(
			IAppDbContext context, 
			IPasswordService passwordService, 
			IEmailService emailService, 
			DomainValidationService validation,
			IOptions<AuthConfiguration> options)
		{
			this.context = context;
			this.passwordService = passwordService;
			this.emailService = emailService;
			this.validation = validation;
			this.authConfig = options.Value;
		}

		public async Task SendActivationLink(int userId, CancellationToken cancellationToken)
        {
			var user = await this.context.Set<User>()
					.AsNoTracking()
					.Include(e => e.Role)
					.SingleOrDefaultAsync(e => e.Id == userId, cancellationToken);

			if (!user.IsLocked)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_UserAlreadyUnlocked);
			}

			var oldTokens = await this.context.Set<PasswordToken>()
				.Where(e => !e.IsUsed && e.UserId == userId)
				.ToListAsync(cancellationToken);

			foreach (var oldToken in oldTokens)
			{
				oldToken.Use();
			}

			await this.context.SaveChangesAsync(cancellationToken);

			PasswordToken passwordToken = new PasswordToken(user.Id, 20160);
			this.context.Set<PasswordToken>().Add(passwordToken);

			var payload = new {
				FullName = user.FirstName + " " + user.LastName,
				Role = user.Role.Name,
				ActivationLink = $"{authConfig.Issuer}/user/activation?token={passwordToken.Value}",
			};

			Email email = await this.emailService.ComposeEmailAsync(EmailTypeAlias.USER_ACTIVATION, payload, user.Email);
			this.context.Set<Email>().Add(email);

			await this.context.SaveChangesAsync(cancellationToken);
		}

		public async Task CheckToken(string token, CancellationToken cancellationToken)
        {
			PasswordToken passwordToken = await this.context.Set<PasswordToken>()
				.SingleOrDefaultAsync(e => e.Value == token, cancellationToken);

			if (passwordToken.IsUsed)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_ActivationTokenAlreadyUsed);
			}

			if (passwordToken.ExpirationTime < DateTime.UtcNow)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_TokenExpired);
			}
		}

		public async Task ActivateUser(UserActivationDto model, CancellationToken cancellationToken)
		{
			PasswordToken passwordToken = await this.context.Set<PasswordToken>()
				.Include(e => e.User)
				.SingleOrDefaultAsync(e => e.Value == model.Token, cancellationToken);

			if (passwordToken.IsUsed)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_ActivationTokenAlreadyUsed);
			}

			if (passwordToken.ExpirationTime < DateTime.UtcNow)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_TokenExpired);
			}

			passwordToken.Use();

			string salt = this.passwordService.GenerateSalt(128);
			string hash = this.passwordService.HashPassword(model.Password, salt);
			passwordToken.User.Activate(hash, salt);

			await this.context.SaveChangesAsync(cancellationToken);
		}
	}
}
