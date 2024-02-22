using FileStorageNetCore.Models;
using Resc.Data.Common.Interfaces;
using System;

namespace Resc.Data.Applications
{
	public class AnnexFile : AttachedFile, IEntity
	{
		public int Id { get; private set; }

		public int? ApplicationModificationId { get; private set; }
		public ApplicationModification ApplicationModification { get; private set; }

		public int? ApplicationTerminationId { get; private set; }
		public ApplicationTermination ApplicationTermination { get; private set; }

		private AnnexFile()
		{

		}

		public AnnexFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{
		}

		public void Update(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			this.Key = key;
			this.Hash = hash;
			this.Size = size;
			this.Name = name;
			this.MimeType = mimeType;
			this.DbId = dbId;
		}
	}
}
