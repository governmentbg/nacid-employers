using Resc.Data.Applications;
using Resc.Data.Applications.Register;
using Resc.Infrastructure.Ems.Models;

namespace Resc.Application.Ems.Converters
{
	public interface IEmsApplicationConverter
    {
        EmsApplication ToEmsApplication(string electornicServiceUri, UniversityPart model, string regNumber, ContractFile file, bool hasParent);
    }
}
