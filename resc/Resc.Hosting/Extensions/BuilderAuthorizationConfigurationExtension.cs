using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Resc.Application.Common.Constants;
using System.Security.Claims;

namespace Resc.Hosting.Extensions
{
	public static class BuilderAuthorizationConfigurationExtension
    {
		public static void ConfigureAuthorization(this IServiceCollection services)
		{
			services.AddAuthorization(options => {
				options.DefaultPolicy =
					new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();

				options.AddPolicy("default",
					p => p.RequireAuthenticatedUser());

				options.AddPolicy("RequireAdministratorRole", p => {
					p.RequireClaim(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR);
				});
			});
		}
	}
}
