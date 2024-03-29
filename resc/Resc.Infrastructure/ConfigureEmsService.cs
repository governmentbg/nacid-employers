﻿using Microsoft.Extensions.DependencyInjection;
using System;
using Resc.Infrastructure.Ems;

namespace Resc.Infrastructure
{
	public static class ConfigureEmsService
	{
		public static IServiceCollection AddEmsService(this IServiceCollection services, string emsUri)
		{
			services.AddHttpClient<EmsService>((provider, c) => {
				c.BaseAddress = new Uri(emsUri);
				c.DefaultRequestHeaders.Add("Accept", "application/json");
			});

			return services;
		}
	}
}
