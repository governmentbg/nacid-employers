using Newtonsoft.Json;
using Resc.Infrastructure.Ems.Enums;

namespace Resc.Infrastructure.Ems.Models
{
	public class EmsDocStatusResponse
	{
		public EmsIncomingDocStatus Status { get; set; }
		public string ReceiptElectronicDocument { get; set; }

		public EmsReceiptAcknowledgedDoc ReceiptAcknowledgedDoc =>
			JsonConvert.DeserializeObject<EmsReceiptAcknowledgedDoc>(this.ReceiptElectronicDocument);
	}
}
