using FileStorageNetCore.Models;
using Resc.Data.Applications.Register;
using Resc.Data.Common.Interfaces;
using System;

namespace Resc.Data.Applications
{
	public class ApplicationModification : IEntity
	{
		public int Id { get; private set; }

		public DateTime ModificationDate { get; private set; }

		public string Reason { get; private set; }

		public AnnexFile AnnexFile { get; private set; }

		public int? ApplicationCommitId { get; set; }
		public ApplicationCommit ApplicationCommit { get; set; }

		private ApplicationModification()
		{

		}

		public ApplicationModification(DateTime modificationDate, string reason, AttachedFile annexFile)
		{
			this.ModificationDate = modificationDate;
			this.Reason = reason;

			this.AnnexFile = new AnnexFile(annexFile.Key, annexFile.Hash, annexFile.Size, annexFile.Name, annexFile.MimeType, annexFile.DbId);
		}
	}
}
