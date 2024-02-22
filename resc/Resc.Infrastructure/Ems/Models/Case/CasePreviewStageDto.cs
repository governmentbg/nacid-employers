using System;
using Resc.Infrastructure.Ems.Common;

namespace Resc.Infrastructure.Ems.Models.Case
{
	public class CasePreviewStageDto
	{
		public NameObject ElectronicServiceStage { get; set; }
		public NameObject ExecutorUnit { get; set; }
		public DateTime? EndingDate { get; set; }
	}
}
