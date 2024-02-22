using FileStorageNetCore.Models;
using Resc.Data.Applications;
using Resc.Data.Applications.Enums;
using System;
using System.Collections.Generic;

namespace Resc.Application.Applications.Dtos.Create
{
    public class ContractDto
    {
        public DateTime? SigningDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Number { get; set; }
        public string Term { get; set; }
        public string EmploymentTerm { get; set; }

        public IEnumerable<ContactPersonDto> Contacts = new List<ContactPersonDto>();

        public AttachedFile AttachedFile { get; set; }

        public TaxType? TaxType { get; set; }

        public Contract ToModel()
        {
            var contract = new Contract(this.SigningDate, this.EndDate, this.Number, this.Term, this.EmploymentTerm,
              this.TaxType, this.AttachedFile);

            foreach (var contact in this.Contacts)
            {
                contract.Contacts.Add(new ContactPerson(contact.Name, contact.Email, contact.PhoneNumber, contact.Type));
            }

            return contract;
        }
    }
}
