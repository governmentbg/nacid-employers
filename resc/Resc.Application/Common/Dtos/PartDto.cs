using Resc.Data.Common.Enums;

namespace Resc.Application.Common.Dtos
{
	public class PartDto<TDto>
		where TDto : class
	{
		public int Id { get; set; }

		public TDto Entity { get; set; }

		public PartState State { get; set; }
	}
}
