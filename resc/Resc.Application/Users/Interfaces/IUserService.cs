using Resc.Application.Users.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;
using Resc.Data.Users.Enums;

namespace Resc.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<SearchResultItemDto<UserSearchResultDto>> SearchUsers(UserSearchFilterDto filter, CancellationToken cancellationToken);
        Task<int> CreateUser(CreateUserDto model, CancellationToken cancellationToken);
        Task<UserEditDto> GetUserById(int userId, CancellationToken cancellationToken);
        Task EditUserData(UserEditDto dto, CancellationToken cancellationToken);
        Task ChangeUserPassword(ChangePasswordDto dto, CancellationToken cancellationToken);
        Task<UserStatus> ChangeUserStatus(int id, CancellationToken cancellationToken);
    }
}
