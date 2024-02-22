using FileStorageNetCore.Models;
using Resc.Data.Common.Interfaces;
using System;

namespace Resc.Data.Applications
{
    public class ContractFile : AttachedFile, IEntity
    {
		public int Id { get; private set; }

		public int ContractId { get; private set; }
        public Contract Contract { get; private set; }

        private ContractFile()
		{

		}

		public ContractFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{
			//this.ContractId = contractId;
		}

		public void Update(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			this.Key = key;
			this.Hash = hash;
			this.Size = size;
			this.Name = name;
			this.MimeType = mimeType;
			this.DbId = dbId;
			//this.ContractId = contractId;
		}
	}
}
