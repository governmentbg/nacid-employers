using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Resc.Application.Common.Interfaces;
using Resc.Data.Applications.Register;
using Resc.Data.Nomenclatures;
using Resc.Infrastructure.Ems.Models;
using Resc.Data.Applications;

namespace Resc.Application.Ems.Converters
{
	public class EmsApplicationConverter : IEmsApplicationConverter
    {
        private readonly IAppDbContext context;

        public EmsApplicationConverter(IAppDbContext context)
        {
            this.context = context;
        }

        public EmsApplication ToEmsApplication(string electornicServiceUri, UniversityPart model, string regNumber, ContractFile file, bool hasParent)
        {
            var institution = this.context.Set<Institution>().Where(i => i.Id == model.Entity.InstitutionId).SingleOrDefault();

            var correspondent = new EmsCorrespondent {
                FirstName = model.Entity.FirstName,
                MiddleName = model.Entity.MiddleName,
                LastName = model.Entity.LastName,
                Name = institution.Name,

                CorrespondentContacts = new List<EmsCorrespondentContact>
                {
                    new EmsCorrespondentContact
                    {
                        Name = model.Entity.FirstName + " " + model.Entity.MiddleName + " " + model.Entity.LastName,
                        Email = model.Entity.Mail
                    }
                }
            };

            ICollection<ContractFile> attachedFiles = new List<ContractFile>();
            attachedFiles.Add(file);

            var emsRegisterNumber = regNumber.Split('/');
            var application = new EmsApplication {
                ElectronicServiceUri = electornicServiceUri,
                UseEmailForCorrespondence = false,
                Applicant = correspondent,
                StructuredData = JObject.FromObject(model),
                ParentDocNumber = hasParent ? regNumber : null,
                ExternalNumber = hasParent ? null : emsRegisterNumber[0],
                ExternalNumberDate = hasParent ? null : DateTime.Now.Date,
                AttachedFiles = attachedFiles,
                SkipConfirmationDoc = hasParent ? true : false,
                SkipConfirmationEmail = true
            };

            return application;
        }
    }
}
