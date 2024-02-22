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
    public class ForgottenPasswordController : ControllerBase
    {
        private readonly IForgottenPasswordService forgottenPasswordService;

        public ForgottenPasswordController(IForgottenPasswordService forgottenPasswordService)
        {
            this.forgottenPasswordService = forgottenPasswordService;
        }

        [HttpPost]
        public async Task SendForgottenPasswordMail([FromBody] EmailForgottenPasswordDto model, CancellationToken cancellationToken)
            => await this.forgottenPasswordService.SendMail(model, cancellationToken);

        [HttpPost("Recovery")]
        public async Task RecoverPassword([FromBody] ForgottenPasswordDto model, CancellationToken cancellationToken)
            => await this.forgottenPasswordService.RecoverPassword(model, cancellationToken);
    }
}
