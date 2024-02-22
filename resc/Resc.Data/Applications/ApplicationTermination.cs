using FileStorageNetCore.Models;
using Resc.Data.Common.Interfaces;
using Resc.Data.Nomenclatures;
using System;

namespace Resc.Data.Applications
{
	public class ApplicationTermination : IEntity
	{
		public int Id { get; private set; }

		public DateTime TerminationDate { get; private set; }

		public int TerminationReasonId { get; private set; }
		public TerminationReason TerminationReason { get; private set; }

		public AnnexFile AnnexFile { get; private set; }

		private ApplicationTermination()
		{

		}

		public ApplicationTermination(DateTime terminationDate, int terminationReasonId, AttachedFile annexFile)
		{
			this.TerminationDate = terminationDate;
			this.TerminationReasonId = terminationReasonId;

			if (annexFile != null)
			{
				this.AnnexFile = new AnnexFile(annexFile.Key, annexFile.Hash, annexFile.Size, annexFile.Name, annexFile.MimeType, annexFile.DbId);
			}
		}
	}
}
