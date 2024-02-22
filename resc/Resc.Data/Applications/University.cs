using Resc.Data.Common.Interfaces;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using System;

namespace Resc.Data.Applications
{
    public class University : IEntity, IAuditable, IConcurrency
    {
        public int Id { get; private set; }

        public int? InstitutionId { get; private set; }
        public Institution Institution { get; private set; }

        public int? SpecialityListItemId { get; private set; }
        public SpecialityListItem SpecialityListItem { get; private set; }

        public string Rector { get; private set; }

        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatorUserId { get; set; }

        private University()
        {
        }

        public University(int? institutionId, int? specialityListItemId, string rector)
        {
            this.InstitutionId = institutionId;
            this.SpecialityListItemId = specialityListItemId;

            this.Rector = rector;
        }

        public University(University university)
            : this(university.InstitutionId, university.SpecialityListItemId, university.Rector)
        {
        }

        public void Update(int? institutionId, int? specialityListItemId, string rector)
        {
            this.InstitutionId = institutionId;
            this.SpecialityListItemId = specialityListItemId;
            this.Rector = rector;
        }
    }
}
