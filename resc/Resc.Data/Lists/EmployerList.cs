using System.Collections.Generic;

namespace Resc.Data.Lists
{
	public class EmployerList : BaseList
	{
		public IEnumerable<EmployerListItem> Items { get; set; } = new List<EmployerListItem>();

		public EmployerList(int schoolYearId, bool isPublished)
			: base(schoolYearId, isPublished)
		{
		}
	}
}
