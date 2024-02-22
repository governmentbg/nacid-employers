using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.Nomenclatures.Services;
using Resc.Data.Nomenclatures;
using System;
using Resc.Hosting.BackgroundServices.RabbitMqConsumers.Configurations;
using Resc.Infrastructure.RabbitMq;

namespace Resc.Hosting.BackgroundServices.RabbitMqConsumers
{
    public class SpecialityConsumer : BaseConsumerService<SpecialityDto>
    {
        private readonly IServiceProvider serviceProvider;

        public SpecialityConsumer(
            RndConsumerConnectionService consumer,
            IOptions<RndConsumerConfiguration> configuration,
            IServiceProvider serviceProvider
        )
            : base(consumer, configuration.Value.RndSpecialityUpdateExchange)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override Action<SpecialityDto> OnReceive => SaveSpeciality;

        private async void SaveSpeciality(SpecialityDto model)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var nomenclatureService = scope.ServiceProvider.GetRequiredService<INomenclatureService<Speciality>>();

                var nomenclature = new Speciality(model.Name, model.Id, model.EducationalQualificationId, model.IsActive);

                var existingNomenclature = await nomenclatureService.GetSingleOrDefaultNomenclatureAsync(e => e.ExternalId == model.Id);
                if (existingNomenclature != null)
                {
                    await nomenclatureService.UpdateNomenclatureAsync(existingNomenclature.Id, nomenclature);
                }
                else
                {
                    await nomenclatureService.InsertNomenclatureAsync(nomenclature);
                }
            }
        }
    }
}
