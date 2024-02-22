using Microsoft.AspNetCore.Mvc;
using Resc.Application.Applications.Dtos.Reports;
using Resc.Application.Applications.Dtos.Search;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Common.Constants;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Interfaces;
using Resc.Application.Common.Services;
using Resc.Application.Utils;
using Resc.Data.Applications.Enums;
using System.IO;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Applications
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly IReportService reportService;
		private readonly IExcelProcessor excelProcessor;
		private readonly GeneratePdfService generatePdfService;

		public ReportController(IReportService reportService, IExcelProcessor excelProcessor, GeneratePdfService generatePdfService)
		{
			this.reportService = reportService;
			this.excelProcessor = excelProcessor;
			this.generatePdfService = generatePdfService;
		}

		[HttpGet]
		public async Task<ReportDto> GetReports([FromQuery] SearchReportFilter filter)
			=> await this.reportService.GetReport(filter);

		[HttpPost("Excel")]
		public async Task<FileStreamResult> ExportApplicationsFiltered([FromBody] SearchReportFilter filter)
		{
			filter.Limit = int.MaxValue;
			filter.Offset = 0;

			var report = await this.reportService.GetReport(filter);

			if (report.ReportType == ReportType.ReportBySpecialty)
			{

				var excelStream = this.excelProcessor.ExportReports(filter, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Speciality, ColumnName = "Специалност" },
					e => new ExcelTableTuple { CellItem = e.StudentsCount, ColumnName = "Списък" },
					e => new ExcelTableTuple { CellItem = e.EnteredCommitsCount, ColumnName = "Вписани" },
					e => new ExcelTableTuple { CellItem = e.FreeSpotsCounts, ColumnName = "Свободни" },
					e => new ExcelTableTuple { CellItem = e.ChangedCommitsCount, ColumnName = "Изменени" },
					e => new ExcelTableTuple { CellItem = e.TerminatedCommitsCount, ColumnName = "Прекратени" },
					e => new ExcelTableTuple { CellItem = e.ExpiredCommitsCount, ColumnName = "Приключили" }
					);

				return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
			}

			if (report.ReportType == ReportType.ReportByInstitution)
			{
				var excelStream = this.excelProcessor.ExportReports(filter, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Institution, ColumnName = "Висше училище" },
					e => new ExcelTableTuple { CellItem = e.StudentsCount, ColumnName = "Списък" },
					e => new ExcelTableTuple { CellItem = e.EnteredCommitsCount, ColumnName = "Вписани" },
					e => new ExcelTableTuple { CellItem = e.FreeSpotsCounts, ColumnName = "Свободни" },
					e => new ExcelTableTuple { CellItem = e.ChangedCommitsCount, ColumnName = "Изменени" },
					e => new ExcelTableTuple { CellItem = e.TerminatedCommitsCount, ColumnName = "Прекратени" },
					e => new ExcelTableTuple { CellItem = e.ExpiredCommitsCount, ColumnName = "Приключили" }
					);

				return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
			}

			if (report.ReportType == ReportType.ReportByResearchArea)
			{
				var excelStream = this.excelProcessor.ExportReports(filter, report.Reports,
					e => new ExcelTableTuple { CellItem = e.ResearchArea, ColumnName = "Направление" },
					e => new ExcelTableTuple { CellItem = e.StudentsCount, ColumnName = "Списък" },
					e => new ExcelTableTuple { CellItem = e.EnteredCommitsCount, ColumnName = "Вписани" },
					e => new ExcelTableTuple { CellItem = e.FreeSpotsCounts, ColumnName = "Свободни" },
					e => new ExcelTableTuple { CellItem = e.ChangedCommitsCount, ColumnName = "Изменени" },
					e => new ExcelTableTuple { CellItem = e.TerminatedCommitsCount, ColumnName = "Прекратени" },
					e => new ExcelTableTuple { CellItem = e.ExpiredCommitsCount, ColumnName = "Приключили" }
					);

				return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
			}

			if (report.ReportType == ReportType.ReportByResearchAreaAndSpecialty)
			{
				var excelStream = this.excelProcessor.ExportReports(filter, report.Reports,
					e => new ExcelTableTuple { CellItem = e.ResearchArea, ColumnName = "Направление" },
					e => new ExcelTableTuple { CellItem = e.Speciality, ColumnName = "Специалност" },
					e => new ExcelTableTuple { CellItem = e.StudentsCount, ColumnName = "Списък" },
					e => new ExcelTableTuple { CellItem = e.EnteredCommitsCount, ColumnName = "Вписани" },
					e => new ExcelTableTuple { CellItem = e.FreeSpotsCounts, ColumnName = "Свободни" },
					e => new ExcelTableTuple { CellItem = e.ChangedCommitsCount, ColumnName = "Изменени" },
					e => new ExcelTableTuple { CellItem = e.TerminatedCommitsCount, ColumnName = "Прекратени" },
					e => new ExcelTableTuple { CellItem = e.ExpiredCommitsCount, ColumnName = "Приключили" }
					);

				return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
			}

			if (report.ReportType == ReportType.ReportByResearchAreaAndSpecialityAndInstitution)
			{
				var excelStream = this.excelProcessor.ExportReports(filter, report.Reports,
					e => new ExcelTableTuple { CellItem = e.ResearchArea, ColumnName = "Направление" },
					e => new ExcelTableTuple { CellItem = e.Speciality, ColumnName = "Специалност" },
					e => new ExcelTableTuple { CellItem = e.Institution, ColumnName = "Висше училище" },
					e => new ExcelTableTuple { CellItem = e.StudentsCount, ColumnName = "Списък" },
					e => new ExcelTableTuple { CellItem = e.EnteredCommitsCount, ColumnName = "Вписани" },
					e => new ExcelTableTuple { CellItem = e.FreeSpotsCounts, ColumnName = "Свободни" },
					e => new ExcelTableTuple { CellItem = e.ChangedCommitsCount, ColumnName = "Изменени" },
					e => new ExcelTableTuple { CellItem = e.TerminatedCommitsCount, ColumnName = "Прекратени" },
					e => new ExcelTableTuple { CellItem = e.ExpiredCommitsCount, ColumnName = "Приключили" }
					);

				return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
			}

			if (report.ReportType == ReportType.DefaultReport)
			{
				var excelStream = this.excelProcessor.ExportReports(filter, report.Reports,
					e => new ExcelTableTuple { CellItem = e.StudentsCount, ColumnName = "Списък" },
					e => new ExcelTableTuple { CellItem = e.EnteredCommitsCount, ColumnName = "Вписани" },
					e => new ExcelTableTuple { CellItem = e.FreeSpotsCounts, ColumnName = "Свободни" },
					e => new ExcelTableTuple { CellItem = e.ChangedCommitsCount, ColumnName = "Изменени" },
					e => new ExcelTableTuple { CellItem = e.TerminatedCommitsCount, ColumnName = "Прекратени" },
					e => new ExcelTableTuple { CellItem = e.ExpiredCommitsCount, ColumnName = "Приключили" }
					);

				return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
			}

			return null;
		}

		[HttpPost("PDF")]
		public async Task<FileContentResult> ExportApplicationsFilteredPdf([FromBody] SearchReportFilter filter)
		{
			filter.Limit = int.MaxValue;
			filter.Offset = 0;

			var report = await this.reportService.GetReport(filter);

			if (report.ReportType == ReportType.DefaultReport)
			{
				var bytes = await this.generatePdfService.GenerateReportPdf(FileTemplateAliases.DEFAULT_REPORT_EXPORT, report.Reports, filter);
				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByInstitution)
			{
				var bytes = await this.generatePdfService.GenerateReportPdf(FileTemplateAliases.REPORT_BY_INSTITUTION, report.Reports, filter);
				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportBySpecialty)
			{
				var bytes = await this.generatePdfService.GenerateReportPdf(FileTemplateAliases.REPORT_BY_SPECIALITY, report.Reports, filter);
				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByResearchArea)
			{
				var bytes = await this.generatePdfService.GenerateReportPdf(FileTemplateAliases.REPORT_BY_RESEARCHAREA, report.Reports, filter);
				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByResearchAreaAndSpecialty)
			{
				var bytes = await this.generatePdfService.GenerateReportPdf(FileTemplateAliases.REPORT_BY_RESEARCHAREA_AND_SPECIALITY, report.Reports, filter);
				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByResearchAreaAndSpecialityAndInstitution)
			{
				var bytes = await this.generatePdfService.GenerateReportPdf(FileTemplateAliases.REPORT_BY_RESEARCHAREA_AND_SPECIALITY_AND_INSTITUTION, report.Reports, filter);
				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}

			return null;
		}
	}
}
