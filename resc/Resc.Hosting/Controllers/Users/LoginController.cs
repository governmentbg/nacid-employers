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
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost]
        public async Task<UserLoginInfoDto> LoginUser([FromBody] UserCredentialsDto model, CancellationToken cancellationToken)
            => await this.loginService.Login(model, cancellationToken);
    }
}
