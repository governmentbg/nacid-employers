using System.Collections.Generic;
using Resc.Infrastructure.Ems.Enums;

namespace Resc.Infrastructure.Ems.Models
{
    public class EmsCorrespondent
    {
        public CorrespondentType Type { get; } = CorrespondentType.BulgarianCitizen;

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        //Organization name?!?
        public string Name { get; set; }

        public IList<EmsCorrespondentContact> CorrespondentContacts { get; set; }
    }
}
