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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;

namespace Resc.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IAppDbContext context;
        private readonly DomainValidationService validation;
        private readonly IEmailService emailService;
		private readonly IUserContext userContext;
		private readonly IPasswordService passwordService;
		private readonly AuthConfiguration authConfig;

        public UserService(
            IAppDbContext context,
            DomainValidationService validation,
            IEmailService emailService,
            IOptions<AuthConfiguration> options,
            IUserContext userContext,
            IPasswordService passwordService
            )
        {
            this.context = context;
            this.validation = validation;
            this.emailService = emailService;
			this.userContext = userContext;
			this.passwordService = passwordService;
			this.authConfig = options.Value;
        }

        public async Task<int> CreateUser(CreateUserDto model, CancellationToken cancellationToken)
        {
            bool isEmailTaken = await context.Set<User>()
                    .AnyAsync(e => e.Email.Trim().ToLower() == model.Email.Trim().ToLower() /*&& e.Status == UserStatus.Active*/, cancellationToken);

            if (isEmailTaken)
            {
                this.validation.ThrowErrorMessage(UserErrorCode.User_EmailTaken);
            }

            model.Username = model.Email;

            var user = new User(model.Username, model.FirstName, model.MiddleName, model.LastName, model.Email, model.Phone, model.RoleId, model.Position, model.Institution?.Id);

            this.context.Set<User>().Add(user);
            await this.context.SaveChangesAsync(cancellationToken);

            await this.SendUserActivationLink(user.Id, cancellationToken);

            return user.Id;
        }

		public async Task EditUserData(UserEditDto dto, CancellationToken cancellationToken)
		{
            var user = await this.context.Set<User>()
                    .SingleOrDefaultAsync(e => e.Id == dto.Id, cancellationToken);

            if (user == null)
            {
                this.validation.ThrowErrorMessage(UserErrorCode.User_NotFound);
            }

            bool isEmailTaken = await context.Set<User>()
                .AnyAsync(e => e.Email.Trim().ToLower() == dto.Email.Trim().ToLower() && e.Id != dto.Id, cancellationToken);

            if (isEmailTaken)
            {
                this.validation.ThrowErrorMessage(UserErrorCode.User_EmailTaken);
            }

            user.Update(dto.Email, dto.Email, dto.Phone, dto.FirstName, dto.MiddleName, dto.LastName,
                dto.Role.Id, dto.Position, dto.Institution?.Id);
            await this.context.SaveChangesAsync(cancellationToken);
        }

		public async Task<UserEditDto> GetUserById(int userId, CancellationToken cancellationToken)
		{
            var user = await this.context.Set<User>()
                    .Where(e => e.Id == userId)
                    .Select(UserEditDto.SelectExpression)
                    .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                this.validation.ThrowErrorMessage(UserErrorCode.User_NotFound);
            }

            return user;
        }

		public async Task<SearchResultItemDto<UserSearchResultDto>> SearchUsers(UserSearchFilterDto filter, CancellationToken cancellationToken)
        {
            IQueryable<User> query = context.Set<User>();

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                query = query.Where(e => e.FirstName.Trim().ToLower().Contains(filter.FirstName.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.MiddleName))
            {
                query = query.Where(e => e.MiddleName.Trim().ToLower().Contains(filter.MiddleName.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                query = query.Where(e => e.LastName.Trim().ToLower().Contains(filter.LastName.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                query = query.Where(e => e.Username.Trim().ToLower().Contains(filter.Username.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(e => e.Email.Trim().ToLower().Contains(filter.Email.Trim().ToLower()));
            }

            if (filter.RoleId.HasValue)
            {
                query = query.Where(e => e.RoleId == filter.RoleId.Value);
            }

            if (filter.InstitutionId.HasValue)
            {
                query = query.Where(e => e.InstitutionId == filter.InstitutionId.Value);
            }

            if (filter.Status.HasValue)
            {
                switch (filter.Status)
                {
                    case UserStatus.Active:
                        query = query.Where(e => e.Status == UserStatus.Active);
                        break;
                    case UserStatus.Deactivated:
                        query = query.Where(e => e.Status == UserStatus.Deactivated);
                        break;
                    case UserStatus.Inactive:
                        query = query.Where(e => e.Status == UserStatus.Inactive);
                        break;
                }
            }

            var items = await query
                .Select(UserSearchResultDto.SelectExpression)
                .OrderByDescending(e => e.Id)
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .ToListAsync(cancellationToken);

            var result = new SearchResultItemDto<UserSearchResultDto> {
                Items = items,
                TotalCount = await query.CountAsync()
            };

            return result;
        }

        private async Task SendUserActivationLink(int userId, CancellationToken cancellationToken)
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
                ActivationLink = $"{authConfig.Issuer}/user/activation?token={passwordToken.Value}"
            };

            Email email = await this.emailService.ComposeEmailAsync(EmailTypeAlias.USER_ACTIVATION, payload, user.Email);
            this.context.Set<Email>().Add(email);

            await this.context.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangeUserPassword(ChangePasswordDto dto, CancellationToken cancellationToken)
		{
            if (dto.NewPassword != dto.NewPasswordAgain)
            {
                this.validation.ThrowErrorMessage(UserErrorCode.User_ChangePasswordNewPasswordMismatch);
            }

            var user = await this.context.Set<User>()
                    .SingleAsync(e => e.Id == this.userContext.UserId, cancellationToken);

            if (!this.passwordService.VerifyHashedPassword(user.Password, dto.OldPassword, user.PasswordSalt))
            {
                this.validation.ThrowErrorMessage(UserErrorCode.User_ChangePasswordOldPasswordMismatch);
            }

            string newPasswordSalt = this.passwordService.GenerateSalt(128);
            string newPasswordHash = this.passwordService.HashPassword(dto.NewPassword, newPasswordSalt);
            user.ChangePassword(newPasswordHash, newPasswordSalt);

            await this.context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserStatus> ChangeUserStatus(int id, CancellationToken cancellationToken)
        {
            var user = await this.context.Set<User>()
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (user.Status == UserStatus.Active)
            {
                user.Status = UserStatus.Deactivated;
            }
            else if (user.Status == UserStatus.Deactivated)
            {
                user.Status = UserStatus.Active;
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return user.Status;
        }
    }
}
