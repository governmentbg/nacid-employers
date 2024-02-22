using Microsoft.AspNetCore.Mvc;
using Resc.Application.Common.Constants;
using Resc.Application.Users.Dtos;
using Resc.Application.Users.Interfaces;
using Resc.Hosting.Infrastructure.Auth;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;
using Resc.Data.Users.Enums;

namespace Resc.Hosting.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public async Task<SearchResultItemDto<UserSearchResultDto>> SearchUsers([FromQuery] UserSearchFilterDto filter, CancellationToken cancellationToken)
            => await this.userService.SearchUsers(filter, cancellationToken);

        [HttpPost]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public async Task<int> CreateUser([FromBody] CreateUserDto model, CancellationToken cancellationToken)
            => await this.userService.CreateUser(model, cancellationToken);

        [HttpGet("{id:int}")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public Task<UserEditDto> GetUserById([FromRoute] int id, CancellationToken cancellationToken)
           => this.userService.GetUserById(id, cancellationToken);

        [HttpPut]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public Task UpdateUserData([FromBody] UserEditDto model, CancellationToken cancellationToken)
            => this.userService.EditUserData(model, cancellationToken);

        [HttpPost("NewPassword")]
        public Task ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
           => this.userService.ChangeUserPassword(dto, cancellationToken);

        [HttpPut("changeStatus")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public async Task<UserStatus> ChangeUserStatus([FromBody] int id, CancellationToken cancellationToken)
           => await this.userService.ChangeUserStatus(id, cancellationToken);
    }
}
