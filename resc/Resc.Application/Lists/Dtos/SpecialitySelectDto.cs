using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using System;
using System.Linq.Expressions;

namespace Resc.Application.Lists.Dtos
{
    public class SpecialitySelectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public NomenclatureDto<Speciality> Speciality { get; set; }
        public NomenclatureDto<ResearchArea> ResearchArea { get; set; }
        public NomenclatureDto<EducationalQualification> EducationalQualification { get; set; }
        public NomenclatureDto<EducationFormType> EducationFormType { get; set; }

        public static Expression<Func<SpecialityListItem, SpecialitySelectDto>> SelectExpression
            => e => new SpecialitySelectDto {
                Id = e.Id,
                Name = e.Speciality.Name,
                Speciality = new NomenclatureDto<Speciality> {
                    Id = e.Speciality.Id,
                    Name = e.Speciality.Name
                },
                ResearchArea = new NomenclatureDto<ResearchArea> {
                    Id = e.ResearchArea.Id,
                    Name = e.ResearchArea.Name
                },
                EducationalQualification = new NomenclatureDto<EducationalQualification> {
                    Id = e.EducationalQualification.Id,
                    Name = e.EducationalQualification.Name
                },
                EducationFormType = new NomenclatureDto<EducationFormType> {
                    Id = e.EducationFormType.Id,
                    Name = e.EducationFormType.Name
                },
            };
    }
}
