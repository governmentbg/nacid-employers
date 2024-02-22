using FileStorageNetCore.Models;
using System;

namespace Resc.Application.Applications.Dtos.Modification
{
	public class ApplicationModificationDto
	{
		public DateTime ModificationDate { get; set; }

		public string Reason { get; set; }

		public AttachedFile AnnexFile { get; set; }
	}
}
