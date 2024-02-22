using FileStorageNetCore.Models;
using Resc.Data.Applications.Enums;
using Resc.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Resc.Data.Applications
{
    public class Contract : IEntity, IAuditable, IConcurrency
    {
        public int Id { get; private set; }

        public DateTime? SigningDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string Number { get; private set; }
        public string Term { get; private set; }
        public string EmploymentTerm { get; private set; }

        public TaxType? TaxType { get; private set; }

        public ContractFile ContractFile {get; private set; }
        public List<ContactPerson> Contacts { get; set; } = new List<ContactPerson>();

        public DateTime CreateDate { get; set; }
        public int CreatorUserId { get; set; }
        public int Version { get; set; }

        private Contract()
        {
        }

        public Contract(DateTime? signingDate, DateTime? endDate, string number, string term, string employmentTerm,
             TaxType? taxType, AttachedFile file)
        {
            this.SigningDate = signingDate;
            this.EndDate = endDate;
            this.Number = number;
            this.Term = term;
            this.EmploymentTerm = employmentTerm;
            this.TaxType = taxType;

            if (file != null)
			{
                this.ContractFile = new ContractFile(file.Key, file.Hash, file.Size, file.Name, file.MimeType, file.DbId);
            }
        }

        public Contract(Contract contract)
            : this(contract.SigningDate, contract.EndDate, contract.Number, contract.Term, contract.EmploymentTerm,
                  contract.TaxType, contract.ContractFile)
        {
			foreach (var contactPerson in contract.Contacts)
			{
                this.Contacts.Add(new ContactPerson(contactPerson.Name, contactPerson.Email, contactPerson.PhoneNumber, contactPerson.Type));
			}
        }

        public void Update(DateTime? signingDate, DateTime? endDate, string number, string term, string employmentTerm,
            TaxType? taxType, AttachedFile file)
        {
            this.SigningDate = signingDate;
            this.EndDate = endDate;
            this.Number = number;
            this.Term = term;
            this.EmploymentTerm = employmentTerm;
            this.TaxType = taxType;

            if (file != null)
			{
                if (this.ContractFile == null)
                {
                    this.ContractFile = new ContractFile(file.Key, file.Hash, file.Size, file.Name, file.MimeType, file.DbId);
                }
                else
                {
                    this.ContractFile.Update(file.Key, file.Hash, file.Size, file.Name, file.MimeType, file.DbId);
                }
            }
        }

        public void UpdateContact(int id, string name, string phoneNumber, string email, ContactType type)
		{
            var contact = this.Contacts.SingleOrDefault(e => e.Id == id);

            if (contact != null)
			{
                contact.UpdateContactPerson(name, phoneNumber, email, type);
			}
		}
    }
}
