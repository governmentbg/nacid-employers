using System.Threading.Tasks;

namespace Resc.Application.Common.Interfaces
{
	public interface ITemplateService
	{
		Task<byte[]> GetTemplateAsync(string alias);
	}
}
