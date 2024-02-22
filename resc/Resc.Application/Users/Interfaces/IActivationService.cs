using Resc.Application.Users.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Users.Interfaces
{
    public interface IActivationService
    {
        Task SendActivationLink(int userId, CancellationToken cancellationToken);
        Task CheckToken(string token, CancellationToken cancellationToken);
        Task ActivateUser(UserActivationDto model, CancellationToken cancellationToken);
    }
}
