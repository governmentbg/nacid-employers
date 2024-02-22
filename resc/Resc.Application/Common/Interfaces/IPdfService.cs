using System.IO;
using System.Threading.Tasks;

namespace Resc.Application.Common.Interfaces
{
	public interface IPdfService
	{
		Task<MemoryStream> GeneratePdfFile<T>(T payload, byte[] content, bool closeStream = true);
	}
}
