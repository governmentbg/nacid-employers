using Resc.Application.Applications.Dtos.Reports;
using Resc.Application.Applications.Dtos.Search;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Interfaces
{
	public interface IReportService
	{
		Task<ReportDto> GetReport(SearchReportFilter filter);
	}
}
