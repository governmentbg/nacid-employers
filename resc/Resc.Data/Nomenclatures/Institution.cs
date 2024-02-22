using Resc.Data.Nomenclatures.Enums;
using Resc.Data.Nomenclatures.Models;
using System.Collections.Generic;

namespace Resc.Data.Nomenclatures
{
    public class Institution : Nomenclature
    {
        public int? InstitutionTypeId { get; set; }
        public int ExternalId { get; set; }
        public List<InstitutionSpeciality> InstitutionSpecialities { get; set; } = new List<InstitutionSpeciality>();
        public int RootId { get; set; }
        public int? ParentId { get; set; }
        public Level Level { get; set; }
        public int? InstitutionOwnershipTypeId { get; set; }

        public Institution()
        {

        }

        public Institution(string name, bool isActive, int? institutionTypeId, int externalId, int rootId, int? parentId, Level level)
        {
            this.Id = externalId;
            this.ExternalId = externalId;
            this.Name = name;
            this.IsActive = isActive;
            this.InstitutionTypeId = institutionTypeId;
            this.RootId = rootId;
            this.ParentId = parentId;
            this.Level = level;
        }

        public Institution(Institution institution)
            : this(institution.Name, institution.IsActive, institution.InstitutionTypeId.Value, institution.ExternalId, institution.RootId, institution.ParentId, institution.Level)
        {

        }

        public void Update(string name, bool isActive, int? institutionTypeId, int externalId, int rootId, int? parentId, Level level)
        {
            this.ExternalId = externalId;
            this.Name = name;
            this.IsActive = isActive;
            this.InstitutionTypeId = institutionTypeId;
            this.RootId = rootId;
            this.ParentId = parentId;
            this.Level = level;
        }
    }
}
