using Resc.Application.Users.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Users.Interfaces
{
    public interface IForgottenPasswordService
    {
        Task SendMail(EmailForgottenPasswordDto model, CancellationToken cancellationToken);
        Task RecoverPassword(ForgottenPasswordDto model, CancellationToken cancellationToken);
    }
}
