using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resc.Application.Users.Dtos;
using Resc.Application.Users.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Users
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ActivationController : ControllerBase
    {
        private readonly IActivationService activationService;

        public ActivationController(IActivationService activationService)
        {
            this.activationService = activationService;
        }

        [HttpGet("userActivation")]
        public async Task SendActivationLink([FromQuery] int userId, CancellationToken cancellationToken)
            => await this.activationService.SendActivationLink(userId, cancellationToken);

        [HttpGet]
        public async Task CheckToken([FromQuery] string token, CancellationToken cancellationToken)
            => await this.activationService.CheckToken(token, cancellationToken);

        [HttpPost]
        public async Task ActivateUser([FromBody] UserActivationDto model, CancellationToken cancellationToken)
           => await this.activationService.ActivateUser(model, cancellationToken);
    }
}
