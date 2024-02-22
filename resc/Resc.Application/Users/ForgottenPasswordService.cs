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
using Resc.Data.Users.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Users
{
    public class ForgottenPasswordService : IForgottenPasswordService
    {
		private readonly IAppDbContext context;
		private readonly IEmailService emailService;
		private readonly IPasswordService passwordService;
		private readonly DomainValidationService validation;
		private readonly AuthConfiguration authConfig;

		public ForgottenPasswordService(
			IAppDbContext context,
			IEmailService emailService,
			IPasswordService passwordService,
			DomainValidationService validation,
			IOptions<AuthConfiguration> options)
		{
			this.context = context;
			this.emailService = emailService;
			this.passwordService = passwordService;
			this.validation = validation;
			this.authConfig = options.Value;
		}

        public async Task SendMail(EmailForgottenPasswordDto model, CancellationToken cancellationToken)
        {
			var user = await this.context.Set<User>()
					.AsNoTracking()
					.SingleOrDefaultAsync(e => e.Email.Trim().ToLower() == model.Mail.Trim().ToLower());

			if (user == null)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
			}

			if (user.IsLocked || user.Status == UserStatus.Deactivated)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_CannotRestoreUserPassword);
			}

			PasswordToken passwordToken = new PasswordToken(user.Id, 20160);
			this.context.Set<PasswordToken>().Add(passwordToken);

			var payload = new {
				Username = user.Username,
				ForgottenPasswordLink = $"{authConfig.Issuer}/passwordRecovery?token={passwordToken.Value}"
			};

			Email email = await this.emailService.ComposeEmailAsync(EmailTypeAlias.FORGOTTEN_PASSWORD, payload, user.Email);
			this.context.Set<Email>().Add(email);
			await this.context.SaveChangesAsync(cancellationToken);
		}

		public async Task RecoverPassword(ForgottenPasswordDto model, CancellationToken cancellationToken)
		{
			if (model.NewPassword != model.NewPasswordAgain)
			{
				this.validation.ThrowErrorMessage(UserErrorCode.User_ChangePasswordNewPasswordMismatch);
			}

			if (string.IsNullOrWhiteSpace(model.Token) || string.IsNullOrWhiteSpace(model.NewPassword))
			{
				this.validation.ThrowErrorMessage(SystemErrorCode.System_IncorrectParameters);
			}

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
			string hash = this.passwordService.HashPassword(model.NewPassword, salt);
			passwordToken.User.ChangePassword(hash, salt);
			await this.context.SaveChangesAsync(cancellationToken);
		}
	}
}
