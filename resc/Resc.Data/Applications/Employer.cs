using Resc.Data.Common.Interfaces;
using Resc.Data.Lists;
using System;

namespace Resc.Data.Applications
{
    public class Employer : IEntity, IAuditable, IConcurrency
    {
        public int Id { get; private set; }

        public int? EmployerListItemId {get; private set; }
        public EmployerListItem EmployerListItem { get; private set; }

        public string Representative { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }

        public DateTime CreateDate { get; set; }
        public int CreatorUserId { get; set; }
        public int Version { get; set; }

        private Employer()
        {
        }

        public Employer(int? employerListItemId, string representative, string email, string phoneNumber)
        {
            this.EmployerListItemId = employerListItemId;

            this.Representative = representative;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }

        public Employer(Employer employer)
            : this(employer.EmployerListItemId, employer.Representative, employer.Email, employer.PhoneNumber)
        {
        }

        public void Update(int? employerListItemId, string representative, string email, string phoneNumber)
        {
            this.EmployerListItemId = employerListItemId;

            this.Representative = representative;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }
    }
}
