using Resc.Application.Lists.Dtos;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Applications;
using Resc.Data.Nomenclatures;

namespace Resc.Application.Applications.Dtos.Create
{
    public class UniversityDto
    {
        public NomenclatureDto<Institution> Institution { get; set; }
        public SpecialitySelectDto SpecialityListItem { get; set; }
        public string Rector { get; set; }

        public University ToModel()
            => new University(this.Institution?.Id, this.SpecialityListItem?.Id, this.Rector);
    }
}
