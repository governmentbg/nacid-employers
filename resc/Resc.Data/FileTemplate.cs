using FileStorageNetCore.Models;
using Resc.Data.Common.Interfaces;

namespace Resc.Data
{
	public class FileTemplate : AttachedFile, IEntity
	{
		public int Id { get; set; }
		public string Alias { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
	}
}
