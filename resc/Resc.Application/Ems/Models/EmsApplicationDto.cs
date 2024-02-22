using Resc.Application.Applications.Dtos.Create;

namespace Resc.Application.Ems.Models
{
    public class EmsApplicationDto
    {
        public ApplicationDto StructuredData { get; set; }
        public byte[] Signature { get; set; }
    }
}
