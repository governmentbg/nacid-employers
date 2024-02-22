using System.Collections.Generic;
using Resc.Infrastructure.Ems.Enums;

namespace Resc.Infrastructure.Ems.Models
{
	public class EmsDocImportResult
    {
        public EmsDocImportStatus Status { get; set; } = EmsDocImportStatus.HasTechnicalIssue;
        public IList<string> Errors { get; set; } = new List<string>();
    }
}
