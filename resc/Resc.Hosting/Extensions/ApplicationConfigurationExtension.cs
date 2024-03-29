﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resc.Application.Common.Configurations;
using Resc.Hosting.Infrastructure.Configurations;

namespace Resc.Hosting.Extensions
{
    static public class ApplicationConfigurationExtension
    {
		public static IConfiguration ConfigureApplicationConfiguration(this IServiceCollection services, IWebHostEnvironment environment)
		{
			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

			IConfiguration configuration = configurationBuilder.Build();
            services
                .Configure<AuthConfiguration>(config => configuration.GetSection("AuthConfiguration").Bind(config))
                .Configure<EmailConfiguration>(config => configuration.GetSection("EmailConfiguration").Bind(config));
            //	.Configure<ApplicationFileConfiguration>(configuration.GetSection("ApplicationFileConfiguration"))
            //	.Configure<RndConsumerConfiguration>(configuration.GetSection("RndConsumerConfiguration"))
            //	.Configure<ReCaptchaConfiguration>(configuration.GetSection("ReCaptchaConfiguration"))
            //	.Configure<GeneralConfiguration>(configuration.GetSection("GeneralConfiguration"))
            //	.AddOptions();

            return configuration;
		}
	}
}
