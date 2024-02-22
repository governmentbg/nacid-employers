using System.ComponentModel;

namespace Resc.Data.Applications.Enums
{
    public enum TaxType
    {
        [Description("Таксата е покрита изцяло")]
        Full = 1,

        [Description("Таксата е покрита частично")]
        Partially = 2,
    }
}
