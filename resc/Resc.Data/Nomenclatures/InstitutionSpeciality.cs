using Resc.Data.Common.Interfaces;
using Resc.Data.Nomenclatures;

namespace Resc.Data.Nomenclatures
{
    public class InstitutionSpeciality : IEntity
    {
        public int Id { get; set; }
        public int? ViewOrder { get; set; }
        public bool IsActive { get; set; }
        public int ExternalId { get; set; }
        public int InstitutionId { get; set; }
        public int SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
        public int? EducationalFormId { get; set; }
        public EducationFormType EducationalForm { get; set; }
        public double? Duration { get; set; }
        public int? ResearchAreaId { get; set; }
        public ResearchArea ResearchArea { get; set; }
        //public virtual ICollection<InstitutionSpecialityLanguage> InstitutionSpecialityLanguages { get; set; } = new HashSet<InstitutionSpecialityLanguage>();

        public InstitutionSpeciality()
        {

        }

        public InstitutionSpeciality(int externalId, int institutionId, int specialityId, int? educationalFormId, double duration, bool isActive)
        {
            this.ExternalId = externalId;
            this.InstitutionId = institutionId;
            this.SpecialityId = specialityId;
            this.EducationalFormId = educationalFormId;
            this.Duration = duration;
            this.IsActive = isActive;
        }

        public InstitutionSpeciality(InstitutionSpeciality institutionSpeciality)
            : this(institutionSpeciality.Id, institutionSpeciality.InstitutionId, institutionSpeciality.SpecialityId,
                  institutionSpeciality.EducationalFormId.Value, institutionSpeciality.Duration.Value, institutionSpeciality.IsActive)
        {

        }

        public void Update(int externalId, int institutionId, int specialityId, int? educationalFormId, double duration, bool isActive)
        {
            this.ExternalId = externalId;
            this.InstitutionId = institutionId;
            this.SpecialityId = specialityId;
            this.EducationalFormId = educationalFormId;
            this.Duration = duration;
            this.IsActive = isActive;
        }
    }
}
