using FileStorageNetCore.Models;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Nomenclatures;
using System;

namespace Resc.Application.Applications.Dtos.Modification
{
	public class ApplicationTerminationDto
	{
		public DateTime TerminationDate { get; set; }

		public NomenclatureDto<TerminationReason> TerminationReason { get; set; }

		public AttachedFile AnnexFile { get; set; }
	}
}
