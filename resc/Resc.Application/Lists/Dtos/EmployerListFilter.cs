namespace Resc.Application.Lists.Dtos
{
    public class EmployerListFilter : BaseListFilter
    {
        public int? CompanyId { get; set; }
        public string Bulstat { get; set; }
        public string SpecialityName { get; set; }
    }
}
