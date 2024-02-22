using EnumsNET;
using Resc.Application.Applications.Dtos.Search;
using Resc.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace Resc.Application.Common.Services
{
	public class GeneratePdfService
	{
		private readonly ITemplateService templateService;
		private readonly IPdfService pdfService;

		public GeneratePdfService(ITemplateService templateService, IPdfService pdfService)
		{
			this.templateService = templateService;
			this.pdfService = pdfService;
		}

		public async Task<byte[]> GenerateReportPdf(string templateAlias, object items, SearchReportFilter filter)
		{
			var template = await this.templateService.GetTemplateAsync(templateAlias);
			var stream = await this.pdfService.GeneratePdfFile(new {
				Items = items,
				ReportTypeName = filter.ReportType.AsString(EnumFormat.Description),
				InstitutionName = filter.InstitutionName ?? "Всички",
				SchoolYearName = filter.SchoolYearName,
				ResearchAreaName = filter.ResearchAreaName ?? "Всички",
				SpecialityName = filter.SpecialityName ?? "Всички",
				EducationalQualificationName = filter.EducationalQualificationName ?? "Всички",
				ReportDate = filter.CreatedReportDate.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
			}, template);

			return stream.ToArray();
		}
	}
}
