using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Resc.Application.Common.Interfaces;
using Resc.Hosting.Infrastructure.Auth;

namespace Resc.Application.Common.Extensions
{
    public static class ServicesDIExtension
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			services
				.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
			;
			
			services
				.AddScoped<IUserContext, UserContext>()
				//.AddScoped<CaptchaService>()
				;

			return services;
		}
	}
}
