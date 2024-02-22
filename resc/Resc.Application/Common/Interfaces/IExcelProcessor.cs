using Resc.Application.Applications.Dtos.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace Resc.Application.Common.Interfaces
{
	public interface IExcelProcessor
	{
		MemoryStream ExportReports<T, TResult>(SearchReportFilter filter, IEnumerable<T> list, params Expression<Func<T, TResult>>[] expr);
	}
}
