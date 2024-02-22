using Resc.Data.Nomenclatures.Models;

namespace Resc.Data.Nomenclatures
{
    public class ResearchArea : Nomenclature
    {
        public string Code { get; set; }
        public int CodeNumber { get; set; }
        public string NameBg { get; set; }
        public string NameEng { get; set; }
        public int? ParentId { get; set; }
    }
}
