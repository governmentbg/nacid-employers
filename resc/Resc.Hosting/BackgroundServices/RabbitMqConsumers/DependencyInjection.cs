using Microsoft.Extensions.DependencyInjection;
using Resc.Hosting.BackgroundServices.RabbitMqConsumers;
using Resc.Hosting.BackgroundServices.RabbitMqConsumers.Configurations;

namespace Resc.Hosting.BackgroundServices
{
	public static class DependencyInjection
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services
                .AddSingleton<RndConsumerConnectionService>()
                .AddHostedService<InstitutionConsumer>()
                .AddHostedService<SpecialityConsumer>()
            ;

            return services;
        }
    }
}
