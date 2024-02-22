using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Applications.Dtos.History;
using Resc.Application.Applications.Dtos.Modification;
using Resc.Application.Applications.Dtos.Search;
using Resc.Application.Common.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Interfaces
{
    public interface IApplicationService
    {
        Task<CommitInfoDto> CreateApplication(ApplicationDto model, CancellationToken cancellationToken);
        Task<ApplicationCommitDto> GetApplicationCommit(int lotId, int commitId, CancellationToken cancellationToken);
        Task<SearchResultItemDto<ApplicationSearchResultDto>> GetApplicationsFiltered(SearchApplicationFilter filter, CancellationToken cancellationToken);
        Task<ApplicationLotHistoryDto> GetApplicationLotHistory(int lotId, CancellationToken cancellationToken);
        Task UpdateApplication(int commitId, ApplicationUpdateDto model, CancellationToken cancellationToken);
    }
}
