using Microsoft.AspNetCore.Mvc;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.InstitutionSpecialities.Services;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Application.Nomenclatures.Services;
using Resc.Data.Nomenclatures;
using Resc.Data.Users;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Nomenclatures
{
    public class EducationFormTypeController : BaseNomenclatureController<EducationFormType, NomenclatureDto<EducationFormType>, NomenclatureFilterDto<EducationFormType>>
    {
        public EducationFormTypeController(INomenclatureService<EducationFormType> service)
            : base(service)
        {

        }
    }

    public class InstitutionController : BaseNomenclatureController<Institution, NomenclatureDto<Institution>, NomenclatureFilterDto<Institution>>
    {
        private readonly InstitutionService institutionService;

        public InstitutionController(INomenclatureService<Institution> service, InstitutionService institutionService)
            : base(service)
        {
            this.institutionService = institutionService;
        }

        [HttpGet("University")]
        public async Task<IEnumerable<NomenclatureDto<Institution>>> GetUniversities([FromQuery] NomenclatureFilterDto<Institution> filter)
            => await this.institutionService.GetUniversities<NomenclatureFilterDto<Institution>, NomenclatureDto<Institution>>(filter);

        [HttpGet("InstitutionByName")]
        public async Task<NomenclatureDto<Institution>> GetSingleInstitution([FromQuery] NomenclatureFilterDto<Institution> filter, [FromQuery] string institutionName)
            => await this.institutionService.GetSingle<NomenclatureFilterDto<Institution>, NomenclatureDto<Institution>>(filter, institutionName);
    }

    public class SpecialityController : BaseNomenclatureController<Speciality, NomenclatureDto<Speciality>, NomenclatureFilterDto<Speciality>>
    {
        private readonly InstitutionSpecialitiesService institutionSpecialitiesService;

        public SpecialityController(INomenclatureService<Speciality> service, InstitutionSpecialitiesService institutionSpecialitiesService)
            : base(service)
        {
            this.institutionSpecialitiesService = institutionSpecialitiesService;
        }

        [HttpGet("InstitutionSpecialities")]
        public async Task<IEnumerable<SpecialityListItemDto>> GetInstitutionSpecialities([FromQuery] SpecialityFilterDto filter, CancellationToken cancellationToken)
            => await this.institutionSpecialitiesService.GetInstitutionSpecialities(filter, cancellationToken);
    }

    public class EducationalQualificationController
            : BaseNomenclatureController<EducationalQualification, NomenclatureDto<EducationalQualification>, NomenclatureFilterDto<EducationalQualification>>
    {
        public EducationalQualificationController(INomenclatureService<EducationalQualification> service)
            : base(service)
        {

        }
    }

    public class MinisterController
            : BaseNomenclatureController<Minister, NomenclatureDto<Minister>, NomenclatureFilterDto<Minister>>
    {
        public MinisterController(INomenclatureService<Minister> service)
            : base(service)
        {

        }
    }

    public class ResearchAreaController
            : BaseNomenclatureController<ResearchArea, NomenclatureDto<ResearchArea>, NomenclatureFilterDto<ResearchArea>>
    {
        public ResearchAreaController(INomenclatureService<ResearchArea> service)
            : base(service)
        {

        }
    }

    public class CityController
            : BaseNomenclatureController<City, NomenclatureDto<City>, NomenclatureFilterDto<City>>
    {
        public CityController(INomenclatureService<City> service)
            : base(service)
        {

        }
    }

    public class RoleController
        : BaseNomenclatureController<Role, RoleNomenclatureDto, NomenclatureFilterDto<Role>>
    {
        public RoleController(INomenclatureService<Role> service)
            : base(service)
        {
        }
    }

    public class TerminationReasonController
           : BaseNomenclatureController<TerminationReason, NomenclatureDto<TerminationReason>, NomenclatureFilterDto<TerminationReason>>
    {
        public TerminationReasonController(INomenclatureService<TerminationReason> service)
            : base(service)
        {

        }
    }
}
