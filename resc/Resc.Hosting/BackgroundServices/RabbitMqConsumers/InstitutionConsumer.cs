using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.InstitutionSpecialities.Services;
using Resc.Infrastructure.RabbitMq;
using Resc.Hosting.BackgroundServices.RabbitMqConsumers.Configurations;

namespace Resc.Hosting.BackgroundServices.RabbitMqConsumers
{
    public class InstitutionConsumer : BaseConsumerService<InstitutionDto>
    {
        private readonly IServiceProvider serviceProvider;

        public InstitutionConsumer(
            RndConsumerConnectionService consumer,
            IOptions<RndConsumerConfiguration> configuration,
            IServiceProvider serviceProvider
        )
            : base(consumer, configuration.Value.RndOrganizationUpdateExchange)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override Action<InstitutionDto> OnReceive => SaveInstitution;

        private async void SaveInstitution(InstitutionDto model)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var institutionSpecialitiesService = scope.ServiceProvider.GetRequiredService<InstitutionSpecialitiesService>();

                await institutionSpecialitiesService.SaveInstitution(model);
            }
        }
    }
}
