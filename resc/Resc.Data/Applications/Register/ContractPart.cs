using Resc.Data.Common.Models;

namespace Resc.Data.Applications.Register
{
    public class ContractPart : Part<Contract>
    {
        public ContractPart()
            : base()
        {

        }

        public ContractPart(ContractPart part)
            : base(part)
        {

        }
    }
}
