using Resc.Data.Common.Interfaces;
using Resc.Data.Nomenclatures;

namespace Resc.Data.Lists
{
	public class BaseList: IEntity
	{
		public int Id { get; set; }
		public int SchoolYearId { get; set; }
		public SchoolYear SchoolYear { get; set; }
		public bool IsPublished { get; set; }

		public BaseList()
        {

        }

		public BaseList(int schoolYearId, bool isPublished)
        {
			this.SchoolYearId = schoolYearId;
			this.IsPublished = isPublished;
        }
	}
}
