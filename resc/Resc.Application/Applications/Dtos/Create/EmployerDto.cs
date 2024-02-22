using Resc.Data.Applications;
using Resc.Data.Lists;

namespace Resc.Application.Applications.Dtos.Create
{
    public class  EmployerDto
    {
        public EmployerListItem EmployerListItem { get; set; }
        public string Representative { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Employer ToModel()
            => new Employer(this.EmployerListItem?.Id, this.Representative, this.Email, this.PhoneNumber);
    }
}
