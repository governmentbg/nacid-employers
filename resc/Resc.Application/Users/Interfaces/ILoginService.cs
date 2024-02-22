using Resc.Application.Users.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Users.Interfaces
{
    public interface ILoginService
    {
        Task<UserLoginInfoDto> Login(UserCredentialsDto model, CancellationToken cancellationToken);
    }
}
