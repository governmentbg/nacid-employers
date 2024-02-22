using System.Collections.Generic;

namespace Resc.Data.Lists
{
	public class SpecialityList: BaseList
	{
		public IEnumerable<SpecialityListItem> Items { get; set; } = new List<SpecialityListItem>();

		public SpecialityList() : base()
        {

        }

		public SpecialityList(int schoolYearId, bool isPublished)
			: base(schoolYearId, isPublished)
		{
		}
	}
}
