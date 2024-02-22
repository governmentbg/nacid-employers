using Resc.Application.Applications.Dtos.Modification;
using Resc.Application.Common.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Interfaces
{
    public interface IApplicationModificationService
    {
        Task<CommitInfoDto> StartModification(int lotId, string changeStateDescription, CancellationToken cancellationToken);
        Task<CommitInfoDto> FinishCommitModification(int lotId, CancellationToken cancellationToken);
        Task<CommitInfoDto> CancelModification(int lotId, CancellationToken cancellationToken);
        Task<CommitInfoDto> EraseApplication(int lotId, string changeStateDescription, CancellationToken cancellationToken);
        Task<CommitInfoDto> RevertErasedApplication(int lotId, CancellationToken cancellationToken);
        Task<CommitInfoDto> EnterApplication(int lotId, CancellationToken cancellationToken);
        Task<CommitInfoDto> StartModificationEnteredApplication(int lotId, string changeStateDescription, CancellationToken cancellationToken);
        Task<CommitInfoDto> FinishEnteredModification(int lotId, CancellationToken cancellationToken);
        Task<CommitInfoDto> ChangeEnteredContract(int lotId, ApplicationModificationDto dto, CancellationToken cancellationToken);
        Task<CommitInfoDto> TerminateContract(int lotId, ApplicationTerminationDto dto, CancellationToken cancellationToken);
        Task DeleteDraft(int lotId, CancellationToken cancellationToken);
        Task SendEmail(string alias, object payload, int userId, CancellationToken cancellationToken);
    }
}
