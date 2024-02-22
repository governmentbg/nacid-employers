using Resc.Data.Applications.Enums;
using Resc.Data.Common.Interfaces;

namespace Resc.Data.Applications
{
    public class ContactPerson : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public ContactType Type { get; private set; }

        public int ContractId { get; private set; }
        public Contract Contract { get; private set; }

        private ContactPerson()
        {

        }

        public ContactPerson(string name, string email, string phoneNumber, ContactType type)
        {
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Type = type;
        }

        public void UpdateContactPerson(string name, string phoneNumber, string email, ContactType type)
		{
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Type = type;
        }
    }
}
