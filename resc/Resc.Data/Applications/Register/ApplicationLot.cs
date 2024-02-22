using System;
using Resc.Data.Common.Interfaces;
using Resc.Data.Common.Models;

namespace Resc.Data.Applications.Register
{
	public class ApplicationLot : Lot<ApplicationCommit>, IAuditable
	{
		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }
		public string RegisterNumber { get; set; }
	}
}
