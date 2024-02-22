using Microsoft.Extensions.Options;
using Resc.Infrastructure.RabbitMq;

namespace Resc.Hosting.BackgroundServices.RabbitMqConsumers.Configurations
{
	public class RndConsumerConnectionService : BaseConsumerConnectionService
    {
        public RndConsumerConnectionService(IOptions<RndConsumerConfiguration> configuration)
            : base(configuration.Value)
        {

        }
    }
}
