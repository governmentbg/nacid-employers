namespace Resc.Application.Lists.Dtos
{
    public class SpecialityListFilter: BaseListFilter
    {
        public string SpecialityName { get; set; }
        public int? InstitutionId { get; set; }
        public int? ResearchAreaId { get; set; }
    }
}
